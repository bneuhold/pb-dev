using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;
using System.Globalization;

public partial class Agency : System.Web.UI.Page
{
    UltimateDataContext dc;
    UltimateDC.Agency agency;

    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);

        this.Title = string.Empty;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int languageId = Common.GetLanguageId();
        dc = new UltimateDataContext();

        FetchAgency();

        ContactAgency1.AgencyEmail = agency.ContactEmail;
        ContactAgency1.AgencyName = agency.Name;

        if (!IsPostBack)
        {
            this.Title = String.Format(Resources.placeberry.Agency_Title, agency.Name);
            ltlAdress.Text = String.Format("{0}, {1}, {2}", agency.Address, agency.City, agency.Country);
            ltlAgencyTitle.Text = agency.Name;
            ltlEmail.Text = agency.ContactEmail;
            ltlPhones.Text = agency.ContactPhone;

            string agencyUrl = string.Empty;
            var agencyUrlTag = agency.AgencyUrlTags.SingleOrDefault();
            if (agencyUrlTag == null)
            {
                agencyUrl = "/Agency.aspx?id=" + agency.Id;
            }
            else
            {
                agencyUrl = Common.GenerateAgencyUrl(agencyUrlTag.UrlTag);
            }
            aAgencyLogo.HRef = agencyUrl;
            aAgencyName.HRef = agencyUrl;

            aAgencyName.InnerText = agency.Name;

            if (agency.AgencyDescriptions.Any())
            {
                var opisi = agency.AgencyDescriptions;
                ltlDescription.Text = opisi.Where(i => i.LanguageId == languageId).Select(i => i.Description).SingleOrDefault()
                    ?? opisi.Where(i => i.LanguageId == agency.LangaugeId).Select(i => i.Description).SingleOrDefault()
                    ?? opisi.Take(1).Select(i => i.Description).SingleOrDefault();
            }
            else
            {
                ltlDescription.Text = string.Empty;
            }

            if (agency.Image != null)
            {
                imgLogo.Src = "/thumb.aspx?src=" + agency.Image.Src + "&mw=185&mh=160&crop=1";
                imgLogo.Alt = agency.Image.Alt;
            }
            else
            {
                imgLogo.Src = "/resources/noimg.jpg";
                imgLogo.Alt = "";
            }
            aWebsite.HRef = agency.UrlWebsite ?? "#";
            aWebsite.InnerText = agency.UrlWebsite ?? string.Empty;

            RefeshAgencyAds();
        }
    }

    private void FetchAgency()
    {
        string agencyTag = Common.FixQueryStringUrlTag(Request.QueryString["agencytag"] ?? string.Empty);

        agency = (from p in dc.AgencyUrlTags
                  where p.UrlTag == agencyTag
                  select p.Agency).SingleOrDefault();

        if (agency == null)
        {
            //Ne postoji
            Response.Redirect("/", true);
        }
    }

    private void RefeshAgencyAds()
    {
        AgencyAdverts.Adverts.DataSource = dc.GetAgencyAdverts(agency.Id, Common.GetLanguageId());
        AgencyAdverts.Adverts.DataBind();
    }

    protected void btnContactSubmit_Click(object sender, EventArgs e)
    {

    }
}