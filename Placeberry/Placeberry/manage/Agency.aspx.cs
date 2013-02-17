using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using UltimateDC;
using System.Globalization;
using System.Web.Services;
using System.IO;
using System.Configuration;
using PAB;

public partial class Agency : System.Web.UI.Page
{
    string LOGO_UPLOAD_PATH = ConfigurationManager.AppSettings["AgencyLogoPath"].ToString();
    const int IMAGE_QUALITY = 100;
    const int IMAGE_MAX_H = 800;
    const int IMAGE_MAX_W = 600;


    UltimateDataContext dc;
    UltimateDC.Agency agency;

    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"];
        action = action == null ? string.Empty : action.ToLower();

        dc = new UltimateDataContext();

        switch (action)
        {
            case "newagency":
                NewAgency();
                break;
            case "editagency":
                EditAgency();
                break;
            case "entrysuccess":
                ReadAgency();
                ltlStatus.Text = (string)HttpContext.Current.Items["statusmessage"];
                break;
            default:
                ReadAgency();
                break;
        }
    }


    protected void ReadAgency()
    {
        mvwContainer.ActiveViewIndex = 0;

        FetchAgency();

        aEdit.HRef = string.Format("/manage/Agency.aspx?agencyid={0}&action=editagency", agency.Id);
        aNewAccommodation.HRef = string.Format("/manage/Accommodation.aspx?agencyid={0}&action=newaccomm", agency.Id);
        aBoking.HRef = string.Format("/manage/BookingAdmin.aspx?agencyid={0}", agency.Id);
        aAgencyPage.HRef = string.Format("/Agency.aspx?id={0}", agency.Id);
        // ovako je prije izgledalo. neko je vjerojatno na zivo mjenjao na serveru
        //aAgencyPage.HRef = Common.GenerateAgencyUrl(agency.AgencyUrlTags.FirstOrDefault().UrlTag);

        ltlAgency.Text = agency.Name;
        ltlPrivate.Text = agency.Private ? "Privatna osoba" : "Agencija";
        ltlCountry.Text = agency.Country;
        ltlCity.Text = agency.City;
        ltlStreet.Text = agency.Address;
        ltlContactPhone.Text = agency.ContactPhone;
        ltlEmail.Text = agency.ContactEmail;
        ltlUrlWebsite.Text = agency.UrlWebsite;
        ltlLanguage.Text = agency.Language.Title;

        if (agency.Image != null)
        {
            imgLogoR.Src = agency.Image.Src;
            imgLogoR.Alt = agency.Image.Alt;
            imgLogoR.Visible = true;
        }
        else
        {
            imgLogoR.Visible = false;
        }

        var accomodations = agency.Accommodations.Select(i => new { i.Id, i.Name });
        if (accomodations.Any())
        {
            repAccommodation.DataSource = accomodations;
            repAccommodation.DataBind();
            repAccommodation.Visible = true;
        }
        else
        {
            repAccommodation.Visible = false;
        }


    }
    protected void NewAgency()
    {
        mvwContainer.ActiveViewIndex = 1;

        hAgency.InnerText = "Unos agencije";
        imgLogoE.Visible = false;

        agency = new UltimateDC.Agency();
        agency.ManagerId = (Guid)Membership.GetUser().ProviderUserKey;

        dc.Agencies.InsertOnSubmit(agency);

        btnSave.CommandName = "new";

        if (!IsPostBack)
            ddlLanguage.SelectedValue = Common.GetLanguageId().ToString();
    }
    protected void EditAgency()
    {
        mvwContainer.ActiveViewIndex = 1;

        FetchAgency();

        hAgency.InnerText = "Uređivanje agencije";
        imgLogoE.Visible = false;

        cstvName.Visible = false;
        cstvName.Dispose();

        tbxName.Enabled = false;
        btnSave.CommandName = "edit";

        if (!IsPostBack)
        {
            tbxName.Text = agency.Name;
            tbxCountry.Text = agency.Country;
            tbxCity.Text = agency.City;
            tbxStreet.Text = agency.Address;
            tbxContactPhone.Text = agency.ContactPhone;
            tbxContactEmail.Text = agency.ContactEmail;
            tbxUrlWebsite.Text = agency.UrlWebsite;
            rbtnList.SelectedValue = agency.Private == true ? "p" : "a";
            ddlLanguage.SelectedValue = agency.LangaugeId.ToString();
            tbxDescription.Text = agency.AgencyDescriptions.Where(i => i.LanguageId == agency.LangaugeId).Select(i => i.Description).SingleOrDefault() ?? string.Empty;
        }

        if (agency.Image != null)
        {
            imgLogoE.Src = agency.Image.Src;
            imgLogoE.Alt = agency.Image.Alt;
            imgLogoE.Visible = true;

            rfvUrlLogo.Visible = false;
            rfvUrlLogo.Dispose();
        }
    }

    private void FetchAgency()
    {
        int agencyId = 0;
        int.TryParse(Request["agencyId"] == null ? string.Empty : Request["agencyId"], out agencyId);
        bool admin = User.IsInRole("Administrators");

        agency = (from p in dc.Agencies
                  where p.Id == agencyId && (admin || p.PlaceberryUser.aspnet_User.UserName == User.Identity.Name)
                  select p).SingleOrDefault();

        if (agency == null)
        {
            //Ne postoji ili trenutni korisnik nema prava
            Response.Redirect("/manage/customer.aspx");
        }
    }

    protected void btnSave_Command(object sender, CommandEventArgs e)
    {
        agency.Name = tbxName.Text.Trim();
        agency.Country = tbxCountry.Text.Trim();
        agency.City = tbxCity.Text.Trim();
        agency.Address = tbxStreet.Text.Trim();
        agency.ContactPhone = tbxContactPhone.Text.Trim();
        agency.ContactEmail = tbxContactEmail.Text.Trim();
        agency.UrlWebsite = tbxUrlWebsite.Text.Trim();
        agency.Private = rbtnList.SelectedValue == "p" ? true : false;
        agency.LangaugeId = int.Parse(ddlLanguage.SelectedValue);

        var currdesc = agency.AgencyDescriptions.Where(i => i.LanguageId == agency.LangaugeId).SingleOrDefault();
        if (currdesc != null)
        {
            currdesc.Description = tbxDescription.Text.Trim();
        }
        else
        {
            var description = new AgencyDescription
            {
                Agency = agency,
                LanguageId = agency.LangaugeId,
                Description = tbxDescription.Text.Trim()
            };
            dc.AgencyDescriptions.InsertOnSubmit(description);
        }

        Validate();
        if (IsValid)
        {
            switch (e.CommandName)
            {
                case "edit":

                    UploadAndSetLogo(agency);
                    dc.SubmitChanges();
                    //Ovo treba lokalizirati
                    HttpContext.Current.Items["statusmessage"] = "Spremljene promjene";
                    Server.Transfer(string.Format("/Manage/Agency.aspx?agencyId={0}&action=entrysuccess", agency.Id));
                    break;
                case "new":
                    dc.SubmitChanges();
                    UploadAndSetLogo(agency);
                    dc.SubmitChanges();
                    HttpContext.Current.Items["statusmessage"] = "Unos uspješan";
                    Server.Transfer(string.Format("/Manage/Agency.aspx?agencyId={0}&action=entrysuccess", agency.Id));
                    break;
                default:
                    break;
            }
        }

    }
    protected void cstvName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !CheckAgencyExists(args.Value);
    }

    public static bool CheckAgencyExists(string agency)
    {
        bool exists = false;
        agency = agency == null ? string.Empty : agency.Trim().ToLower();

        using (UltimateDataContext dc = new UltimateDataContext())
        {
            exists = dc.Agencies.Any(i => i.Name.ToLower() == agency);
        }

        return exists;

    }


    private void UploadAndSetLogo(UltimateDC.Agency agency)
    {
        if (fuLogo.HasFile)
        {
            HttpPostedFile file = fuLogo.PostedFile;
            if (Common.IsImage(file.ContentType))
            {
                string fileName = String.Format("logo_{0}_{1}{2}", agency.Name.Replace(' ', '_').Replace('+','-'), agency.Id, Path.GetExtension(file.FileName));
                string filePath = LOGO_UPLOAD_PATH.Last() == '/' ? LOGO_UPLOAD_PATH : LOGO_UPLOAD_PATH + "/";

                string srcLocation = filePath + fileName;
                string saveLocation = Server.MapPath(srcLocation);

                ImageResizer resizer = new ImageResizer();
                resizer.OutputFormat = ImageFormat.Jpeg;
                resizer.ImgQuality = IMAGE_QUALITY;
                resizer.MaxHeight = IMAGE_MAX_H;
                resizer.MaxWidth = IMAGE_MAX_W;

                byte[] imgRes = resizer.Resize(file);


                if (!Directory.Exists(saveLocation)) Directory.CreateDirectory(Path.GetDirectoryName(saveLocation));
                File.WriteAllBytes(saveLocation, imgRes);
                

                UltimateDC.Image logo;

                if (agency.Image == null)
                {
                    logo = new UltimateDC.Image();
                    dc.Images.InsertOnSubmit(logo);
                }
                else
                {
                    logo = agency.Image;
                    string oldSaveLocation = Server.MapPath(logo.Src);
                    if (oldSaveLocation != saveLocation && File.Exists(oldSaveLocation))
                        File.Delete(oldSaveLocation);
                }
                logo.Src = srcLocation;
                logo.Alt = String.Format("{0} logo", agency.Name);
                agency.Image = logo;
            }

        }
    }




}