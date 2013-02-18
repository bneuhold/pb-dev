using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Placeberry.DAL;
using UltimateDC;
using PAB;
using AjaxControlToolkit;
using System.Text.RegularExpressions;


public partial class manage_Accommodation : System.Web.UI.Page
{
    string ACCOMIMAGE_UPLOAD_PATH = ConfigurationManager.AppSettings["AccommodationImagesPath"].ToString();
    const int EMPTY_VALUE = -1;
    const string EMPTY_TEXT = "";
    const int IMAGE_QUALITY = 100;
    const int IMAGE_MAX_H = 800;
    const int IMAGE_MAX_W = 600;

    UltimateDataContext dc;
    UltimateDC.Accommodation accommodation;
    UltimateDC.Agency agency;


    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        dc = new UltimateDataContext();

        string action = Request["action"];
        action = action == null ? string.Empty : action.ToLower();

        switch (action)
        {
            case "editaccomm":
                EditAccomm();
                break;
            case "newaccomm":
                NewAccomm();
                break;
            case "editimages":
                EditImages();
                break;
            case "editprices":
                EditPrices();
                break;
            case "entrysuccess":
                ReadAccomm();
                ltlStatus.Text = (string)HttpContext.Current.Items["statusmessage"];
                break;
            default:
                ReadAccomm();
                break;
        }

        if (!IsPostBack && accommodation != null)
        {
            aAccomm.HRef = String.Format("/manage/accommodation.aspx?accommid={0}", accommodation.Id);
            aAccomm.InnerText = accommodation.Name;
        }
    }


    private void ReadAccomm()
    {
        mvwAccommodation.ActiveViewIndex = 0;
        ViewState.Clear();

        FetchAccommodation();

        aEdit.HRef = String.Format("/manage/accommodation.aspx?accommid={0}&action=editaccomm", accommodation.Id);
        aImages.HRef = String.Format("/manage/accommodation.aspx?accommid={0}&action=editimages", accommodation.Id);
        aPrices.HRef = String.Format("/manage/accommodation.aspx?accommid={0}&action=editprices", accommodation.Id);

        var acccity = accommodation.AccommodationCity;

        //aAccomm.HRef = String.Format("/manage/accommodation.aspx?accommid={0}", accommodation.Id);
        //aAccomm.InnerText = accommodation.Name;
        ltlAgency.Text = accommodation.Agency.Name;
        ltlType.Text = dc.GetTraslation(accommodation.TypeId, accommodation.Agency.LangaugeId);

        btnPublishAccomAdvert.Enabled = !accommodation.IsPublished || accommodation.IsDirty;
        btnPublishAccomAdvert.Text = accommodation.IsPublished ? "Re-publish" : "Publish";

        if (!acccity.PendingApproval && acccity.UltimateTableId != null)
        {
            var parents = from p in dc.GetParents(null, acccity.UltimateTableId, accommodation.Agency.LangaugeId)
                          orderby p.Lvl ascending
                          select p.ParentTitle;
            ltlCityRegionCountry.Text = dc.GetTraslation(acccity.UltimateTableId, accommodation.Agency.LangaugeId) + ", " + string.Join(", ", parents.ToArray());
        }
        else
        {
            ltlCityRegionCountry.Text = acccity.City + (string.IsNullOrEmpty(acccity.Subregion) ? "" : ", " + acccity.Subregion) + (string.IsNullOrEmpty(acccity.Region) ? "" : ", " + acccity.Region) + ", " + acccity.Country;
        }

    }

    private void EditAccomm()
    {
        mvwAccommodation.ActiveViewIndex = 1;
        mvwDescriptions.ActiveViewIndex = 1;

        FetchAccommodation();

        btnSave.Text = "Spremi promjene";
        btnSave.CommandName = "saveedit";
        hNewEdit.InnerText = "Uređivanje smještaja";

        ltlCountry.Visible = false;
        ddlCountry.Visible = false;
        mvwCityEntry.Visible = false;

        if (!IsPostBack)
        {
            tbxName.Text = accommodation.Name;
            tbxCapacity.Text = accommodation.CapacityMin.HasValue && accommodation.CapacityMax.HasValue ? accommodation.CapacityMin.Value + "-" + accommodation.CapacityMax.Value : accommodation.CapacityMax.HasValue ? accommodation.CapacityMax.Value.ToString() : string.Empty;
            chbxPets.Checked = accommodation.Pets;
            tbxAdress.Text = accommodation.Address;

            var emptyItem = new { Id = EMPTY_VALUE, Title = EMPTY_TEXT };
            var countries = (from p in dc.UltimateTables
                             where (ObjectType)p.ObjectTypeId == ObjectType.COUNTRY
                             orderby p.Title
                             select new
                             {
                                 Id = p.Id,
                                 Title = p.Title
                             }).ToList();

            ddlCountry.DataSource = countries;
            ddlCountry.DataValueField = "Id";
            ddlCountry.DataTextField = "Title";
            ddlCountry.DataBind();

            ddlCountry.SelectedValue = accommodation.AccommodationCity.Country;

            var types = (from p in dc.UltimateTables
                         where (ObjectType)p.ObjectTypeId == ObjectType.ACCOMM
                         orderby p.Title
                         select new
                         {
                             Id = p.Id,
                             Title = p.Title
                         }).ToList();

            ddlType.DataSource = types;
            ddlType.DataValueField = "Id";
            ddlType.DataTextField = "Title";
            ddlType.DataBind();

            ddlType.SelectedValue = accommodation.TypeId.ToString();

            RefreshDescriptions();
        }

    }

    private void NewAccomm()
    {
        mvwAccommodation.ActiveViewIndex = 1;
        mvwDescriptions.ActiveViewIndex = 0;

        FetchAgency();

        aAccomm.Visible = false;
        btnSave.Text = "Submit";
        btnSave.CommandName = "savenew";
        hNewEdit.InnerText = "Unos smještaja";

        string cityentrymode = (string)ViewState["cityentrymode"];
        if (string.IsNullOrEmpty(cityentrymode))
        {
            cityentrymode = "auto";
            ViewState["cityentrymode"] = "auto";
        }

        if (!IsPostBack)
        {
            var emptyItem = new { Id = EMPTY_VALUE, Title = EMPTY_TEXT };
            var countries = (from p in dc.UltimateTables
                             where (ObjectType)p.ObjectTypeId == ObjectType.COUNTRY
                             orderby p.Title
                             select new
                             {
                                 Id = p.Id,
                                 Title = p.Title
                             }).ToList();

            countries.Insert(0, emptyItem);
            ddlCountry.DataSource = countries;
            ddlCountry.DataValueField = "Id";
            ddlCountry.DataTextField = "Title";
            ddlCountry.DataBind();
            ddlCountry.SelectedIndex = 0;

            var types = (from p in dc.UltimateTables
                         where (ObjectType)p.ObjectTypeId == ObjectType.ACCOMM
                         orderby p.Title
                         select new
                         {
                             Id = p.Id,
                             Title = p.Title
                         }).ToList();

            types.Insert(0, emptyItem);
            ddlType.DataSource = types;
            ddlType.DataValueField = "Id";
            ddlType.DataTextField = "Title";
            ddlType.DataBind();
            ddlType.SelectedIndex = 0;

            rbtnDefaultDescLanguage.Checked = true;
            rbtnDefaultDescLanguage.Text = agency.Language.Title;


            if (cityentrymode == "auto")
            {
                mvwCityEntry.ActiveViewIndex = 0;

                var cities = (from p in dc.GetParentCities(int.Parse(ddlCountry.SelectedValue), agency.LangaugeId)
                              select new
                              {
                                  Id = p.ChildId.Value,
                                  Title = String.Format("{0}, {1}", p.ChildTitle, p.ParentTitle)
                              }).ToList();

                cities.Insert(0, emptyItem);
                ddlCityRegion.DataSource = cities;
                ddlCityRegion.DataValueField = "Id";
                ddlCityRegion.DataTextField = "Title";
                ddlCityRegion.DataBind();
                ddlCityRegion.SelectedIndex = 0;

            }
            else if (cityentrymode == "manual")
            {
                mvwCityEntry.ActiveViewIndex = 1;
            }
        }

    }

    private void EditImages()
    {
        mvwAccommodation.ActiveViewIndex = 2;

        FetchAccommodation();

        pnlImageEdit.Visible = false;

        if (!IsPostBack)
        {
            RefreshImages();
        }

    }

    private void EditPrices()
    {
        mvwAccommodation.ActiveViewIndex = 3;

        FetchAccommodation();

        if (!IsPostBack)
        {
            RefreshPrices();

            var currency = from p in dc.Currencies
                           orderby p.Title
                           select new
                           {
                               Id = p.Id,
                               Title = p.Title
                           };

            ddlCurrency.DataSource = currency;
            ddlCurrency.DataValueField = "Id";
            ddlCurrency.DataTextField = "Title";
            ddlCurrency.DataBind();

            pnlPriceEdit.Visible = false;
        }

        tbxPriceEditStart.Attributes.Add("readonly", "readonly");
        tbxPriceEditEnd.Attributes.Add("readonly", "readonly");
    }


    private void FetchAccommodation()
    {
        int accommId = 0;
        int.TryParse(Request["accommid"] == null ? string.Empty : Request["accommid"], out accommId);
        bool admin = User.IsInRole("Administrators");

        accommodation = (from p in dc.Accommodations
                         where p.Id == accommId && (admin || p.Agency.PlaceberryUser.aspnet_User.UserName == User.Identity.Name)
                         select p).SingleOrDefault();

        if (accommodation == null)
        {
            //Ne postoji ili trenutni korisnik nema prava
            Response.Redirect("/manage/customer.aspx");
        }

        aBackToAgency.HRef = String.Format("/manage/Agency.aspx?agencyid={0}", accommodation.AgencyId);
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

        aBackToAgency.HRef = String.Format("/manage/Agency.aspx?agencyid={0}", agency.Id);
    }


    protected void lbtnEditDeleteImage_Command(object sender, CommandEventArgs e)
    {
        int imageId = int.Parse((string)e.CommandArgument);

        if (e.CommandName == "imagedelete")
        {
            var accimage = dc.AccommodationImages.Where(i => i.ImageId == imageId && i.AccommodationId == accommodation.Id).SingleOrDefault();

            string imagepath = Server.MapPath(accimage.Image.Src);

            dc.AccommodationImages.DeleteOnSubmit(accimage);
            dc.Images.DeleteOnSubmit(accimage.Image);

            dc.SubmitChanges();

            if (File.Exists(imagepath)) File.Delete(imagepath);

            RefreshImages();
        }

        else if (e.CommandName == "imageedit")
        {
            pnlImageEdit.Visible = true;

            var image = dc.Images.Where(i => i.Id == imageId).SingleOrDefault();

            tbxImageEditTitle.Text = image.Title;
            tbxImageEditDescription.Text = image.Description;

            btnImageEditSave.CommandArgument = image.Id.ToString();

            RefreshImages();
        }

    }

    protected void lbtnManualEntry_Click(object sender, EventArgs e)
    {
        mvwCityEntry.ActiveViewIndex = 1;

        ViewState["cityentrymode"] = "manual";

        ddlCountry_SelectedIndexChanged(null, null);
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        int countryId = int.Parse(ddlCountry.SelectedValue);

        string cityentrymode = (string)ViewState["cityentrymode"];

        if (cityentrymode == "auto")
        {
            var emptyItem = new { Id = EMPTY_VALUE, Title = EMPTY_TEXT };
            var cities = (from p in dc.GetParentCities(countryId, agency.LangaugeId)
                          select new
                          {
                              Id = p.ChildId.Value,
                              Title = String.Format("{0}, {1}", p.ChildTitle, p.ParentTitle)
                          }).ToList();

            cities.Insert(0, emptyItem);
            ddlCityRegion.DataSource = cities;
            ddlCityRegion.DataValueField = "Id";
            ddlCityRegion.DataTextField = "Title";
            ddlCityRegion.DataBind();

        }
        else if (cityentrymode == "manual")
        {
            ResetCombobox(cbxRegion);
            ResetCombobox(cbxSubregion);
            ResetCombobox(cbxIsland);
            ResetCombobox(cbxCity);

            FillRegions(countryId);
            FillSubregions(countryId);
            FillIslands(countryId);
            FillCities(countryId);
        }
    }
    protected void cbxRegion_TextChanged(object sender, EventArgs e)
    {
        int regionId = EMPTY_VALUE;
        int.TryParse(cbxRegion.SelectedValue, out regionId);

        ResetCombobox(cbxSubregion);
        ResetCombobox(cbxIsland);
        ResetCombobox(cbxCity);

        if (regionId != EMPTY_VALUE)
        {
            FillSubregions(regionId);
            FillIslands(regionId);
            FillCities(regionId);
        }
    }
    protected void cbxSubregion_TextChanged(object sender, EventArgs e)
    {
        int subregionId = EMPTY_VALUE;
        int.TryParse(cbxSubregion.SelectedValue, out subregionId);

        ResetCombobox(cbxIsland);
        ResetCombobox(cbxCity);

        if (subregionId != EMPTY_VALUE)
        {
            FillIslands(subregionId);
            FillCities(subregionId);
        }
    }
    protected void cbxIsland_TextChanged(object sender, EventArgs e)
    {
        int islandId = EMPTY_VALUE;
        int.TryParse(cbxIsland.SelectedValue, out islandId);

        ResetCombobox(cbxCity);

        if (islandId != EMPTY_VALUE)
        {
            FillCities(islandId);
        }
    }
    protected void cbxCity_TextChanged(object sender, EventArgs e)
    {

    }

    private void ResetCombobox(ComboBox cbx)
    {
        cbx.DataValueField = "Id";
        cbx.DataTextField = "Title";
        cbx.Items.Clear();
        cbx.SelectedIndex = -1;
    }

    private void FillRegions(int parentId)
    {
        var emptyItem = new { Id = EMPTY_VALUE, Title = EMPTY_TEXT };
        var regions = (from p in dc.GetParentChildsByType(parentId, (int)ObjectType.REGION, agency.LangaugeId)
                       orderby p.ChildTitle
                       select new
                       {
                           Id = p.ChildId.Value,
                           Title = p.ChildTitle
                       }).ToList();

        regions.Insert(0, emptyItem);
        cbxRegion.DataSource = regions;
        cbxRegion.DataValueField = "Id";
        cbxRegion.DataTextField = "Title";
        cbxRegion.DataBind();
    }
    private void FillSubregions(int parentId)
    {
        var emptyItem = new { Id = EMPTY_VALUE, Title = EMPTY_TEXT };
        var subregions = (from p in dc.GetParentChildsByType(parentId, (int)ObjectType.SUBREGION, agency.LangaugeId)
                          orderby p.ChildTitle
                          select new
                          {
                              Id = p.ChildId.Value,
                              Title = p.ChildTitle
                          }).ToList();

        subregions.Insert(0, emptyItem);
        cbxSubregion.DataSource = subregions;
        cbxSubregion.DataValueField = "Id";
        cbxSubregion.DataTextField = "Title";
        cbxSubregion.DataBind();
    }
    private void FillIslands(int parentId)
    {
        var emptyItem = new { Id = EMPTY_VALUE, Title = EMPTY_TEXT };
        var islands = (from p in dc.GetParentChildsByType(parentId, (int)ObjectType.ISLAND, agency.LangaugeId)
                       orderby p.ChildTitle
                       select new
                       {
                           Id = p.ChildId.Value,
                           Title = p.ChildTitle
                       }).ToList();

        islands.Insert(0, emptyItem);
        cbxIsland.DataSource = islands;
        cbxIsland.DataValueField = "Id";
        cbxIsland.DataTextField = "Title";
        cbxIsland.DataBind();
    }
    private void FillCities(int parentId)
    {
        var emptyItem = new { Id = EMPTY_VALUE, Title = EMPTY_TEXT };
        var cities = (from p in dc.GetParentCities(parentId, agency.LangaugeId)
                      select new
                      {
                          Id = p.ChildId.Value,
                          Title = p.ChildTitle
                      }).ToList();

        cities.Insert(0, emptyItem);
        cbxCity.DataSource = cities;
        cbxCity.DataValueField = "Id";
        cbxCity.DataTextField = "Title";
        cbxCity.DataBind();
    }


    protected void btnSave_Command(object sender, CommandEventArgs e)
    {
        string cityentrymode = (string)ViewState["cityentrymode"];

        bool validtest = false;
        Validate("newedit");
        validtest = IsValid;
        if (cityentrymode == "auto")
        {
            Validate("autocity");
            validtest &= IsValid;
        }
        else if (cityentrymode == "manual")
        {
            Validate("manualcity");
            validtest &= IsValid;
        }

        if (validtest)
        {
            switch (e.CommandName)
            {
                case "savenew":
                    accommodation = new UltimateDC.Accommodation();

                    AccommodationCity acccity = new AccommodationCity();
                    dc.AccommodationCities.InsertOnSubmit(acccity);
                    accommodation.AccommodationCity = acccity;
                    AccommodationDescription accdesc = new AccommodationDescription();
                    dc.AccommodationDescriptions.InsertOnSubmit(accdesc);
                    accdesc.Accommodation = accommodation;

                    accdesc.Description = tbxNewDefaultLangDescription.Text.Trim();
                    accdesc.LanguageId = agency.LangaugeId;

                    accommodation.Address = tbxAdress.Text.Trim();
                    accommodation.Agency = agency;
                    accommodation.Name = tbxName.Text.Trim();
                    accommodation.TypeId = int.Parse(ddlType.SelectedValue);
                    accommodation.Active = true;
                    accommodation.IsPublished = false;
                    accommodation.IsDirty = false;

                    accommodation.Pets = chbxPets.Checked;

                    Regex reg1 = new Regex("^([1-9][0-9]*)$");
                    Regex reg2 = new Regex("^(?<min>[1-9][0-9]*)\\s*-\\s*(?<max>[1-9][0-9]*)$");
                    Regex reg3 = new Regex("^(?<min>[1-9][0-9]*)\\s*\\+\\s*(?<max>[1-9][0-9]*)$");
                    string capmin = tbxCapacity.Text.Trim();
                    string capmax = tbxCapacity.Text.Trim();
                    if (reg1.IsMatch(capmax))
                    {
                        accommodation.CapacityMax = int.Parse(capmax);
                    }
                    else if (reg2.IsMatch(capmax))
                    {
                        Match match = reg2.Match(capmin);
                        capmin = match.Groups["min"].Value;

                        match = reg2.Match(capmax);
                        capmax = match.Groups["max"].Value;

                        accommodation.CapacityMin = int.Parse(capmin);
                        accommodation.CapacityMax = int.Parse(capmax);
                    }
                    else if (reg3.IsMatch(capmax))
                    {
                        Match match = reg3.Match(capmin);
                        capmin = match.Groups["min"].Value;

                        match = reg3.Match(capmax);
                        capmax = match.Groups["max"].Value;

                        accommodation.CapacityMin = int.Parse(capmin);
                        accommodation.CapacityMax = int.Parse(capmin) + int.Parse(capmax);
                    }

                    if (cityentrymode == "auto")
                    {
                        int cityId = int.Parse(ddlCityRegion.SelectedValue);

                        string city = dc.GetTraslation(cityId, agency.LangaugeId);
                        var cityparents = dc.GetParents(null, cityId, agency.LangaugeId).ToList();

                        string country = cityparents.Where(i => i.ObjectTypeCode == "COUNTRY").Select(i => i.ParentTitle).SingleOrDefault() ?? string.Empty;
                        string region = cityparents.Where(i => i.ObjectTypeCode == "REGION").Select(i => i.ParentTitle).SingleOrDefault() ?? string.Empty;
                        string subregion = cityparents.Where(i => i.ObjectTypeCode == "SUBREGION").Select(i => i.ParentTitle).SingleOrDefault() ?? string.Empty;
                        string island = cityparents.Where(i => i.ObjectTypeCode == "ISLAND").Select(i => i.ParentTitle).SingleOrDefault() ?? string.Empty;

                        acccity.City = city;
                        acccity.Country = country;
                        acccity.Region = region;
                        acccity.Subregion = subregion;
                        acccity.Island = island;
                        acccity.UltimateTableId = cityId;
                    }
                    else if (cityentrymode == "manual")
                    {
                        acccity.Country = ddlCountry.SelectedItem.Text;
                        acccity.Region = cbxRegion.SelectedItem.Text;
                        acccity.Subregion = cbxSubregion.SelectedItem.Text;
                        acccity.Island = cbxIsland.SelectedItem.Text;
                        acccity.City = cbxCity.SelectedItem.Text;

                        int cityid = 0;
                        int.TryParse(cbxCity.SelectedValue, out cityid);
                        acccity.UltimateTableId = cityid == 0 ? (int?)null : cityid;
                        acccity.PendingApproval = cityid == 0 ? true : false;
                    }

                    dc.SubmitChanges();

                    HttpContext.Current.Items["statusmessage"] = "Uspješan unos!";
                    Server.Transfer(String.Format("/manage/accommodation.aspx?accommid={0}&action=entrysuccess", accommodation.Id));

                    break;
                case "saveedit":

                    accommodation.Name = tbxName.Text;
                    accommodation.TypeId = int.Parse(ddlType.SelectedValue);
                    accommodation.Pets = chbxPets.Checked;
                    accommodation.Address = tbxAdress.Text.Trim();


                    reg1 = new Regex("^([1-9][0-9]*)$");
                    reg2 = new Regex("^(?<min>[1-9][0-9]*)\\s*-\\s*(?<max>[1-9][0-9]*)$");
                    reg3 = new Regex("^(?<min>[1-9][0-9]*)\\s*\\+\\s*(?<max>[1-9][0-9]*)$");

                    capmin = tbxCapacity.Text.Trim();
                    capmax = tbxCapacity.Text.Trim();
                    if (reg1.IsMatch(capmax))
                    {
                        accommodation.CapacityMax = int.Parse(capmax);
                    }
                    else if (reg2.IsMatch(capmax))
                    {
                        Match match = reg2.Match(capmin);
                        capmin = match.Groups["min"].Value;

                        match = reg2.Match(capmax);
                        capmax = match.Groups["max"].Value;

                        accommodation.CapacityMin = int.Parse(capmin);
                        accommodation.CapacityMax = int.Parse(capmax);
                    }
                    else if (reg3.IsMatch(capmax))
                    {
                        Match match = reg3.Match(capmin);
                        capmin = match.Groups["min"].Value;

                        match = reg3.Match(capmax);
                        capmax = match.Groups["max"].Value;

                        accommodation.CapacityMin = int.Parse(capmin);
                        accommodation.CapacityMax = int.Parse(capmin) + int.Parse(capmax);
                    }

                    if (accommodation.IsPublished) accommodation.IsDirty = true;

                    dc.SubmitChanges();

                    HttpContext.Current.Items["statusmessage"] = "Uspješna promjena!";
                    Server.Transfer(String.Format("/manage/accommodation.aspx?accommid={0}&action=entrysuccess", accommodation.Id));

                    break;
                default:
                    break;
            }
        }

    }

    protected void lbtnDeleteDescription_Command(object sender, CommandEventArgs e)
    {

        if (e.CommandName == "deletedescription")
        {
            string argument = (string)e.CommandArgument;
            var split = argument.Split(',');
            int accommid = int.Parse(split[0]);
            int languageid = int.Parse(split[1]);

            if (accommodation.AccommodationDescriptions.Count() > 1)
            {
                var description = dc.AccommodationDescriptions.Where(i => i.AccommodationId == accommid && i.LanguageId == languageid).SingleOrDefault();

                dc.AccommodationDescriptions.DeleteOnSubmit(description);
                dc.SubmitChanges();

                RefreshDescriptions();
            }
            else
            {
                ltlStatus.Text = "Mora ostati jedan opis";
            }
        }
    }

    protected void btnAddDescription_Click(object sender, EventArgs e)
    {
        Validate("adddescription");

        if (IsValid)
        {
            AccommodationDescription description = new AccommodationDescription();
            dc.AccommodationDescriptions.InsertOnSubmit(description);

            description.Accommodation = accommodation;
            description.Description = tbxAddDescription.Text.Trim();
            description.LanguageId = int.Parse(ddlAddDescLangs.SelectedValue);

            dc.SubmitChanges();

            RefreshDescriptions();
        }

    }

    protected void btnImageEditSave_Command(object sender, CommandEventArgs e)
    {
        Validate("imageeditsave");

        if (IsValid)
        {
            if (e.CommandName == "imageeditsave")
            {
                int imageId = int.Parse((string)e.CommandArgument);

                var image = (from p in dc.Images
                             where p.Id == imageId
                             select p).SingleOrDefault();

                image.Title = tbxImageEditTitle.Text.Trim();
                image.Description = tbxImageEditDescription.Text.Trim();

                dc.SubmitChanges();

                tbxImageEditDescription.Text = string.Empty;
                tbxImageEditTitle.Text = string.Empty;

                pnlImageEdit.Visible = false;

                RefreshImages();
            }
        }
    }

    protected void btnUploadAccommImage_Click(object sender, EventArgs e)
    {
        object sesobj = Session["imageuploadpreview"];
        Session.Remove("imageuploadpreview");
        Validate("uploadimage");
        if (IsValid && sesobj != null)
        {
            byte[] buffer = (byte[])sesobj;
            MemoryStream ms = new MemoryStream(buffer);
            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);

            var cropinfo = (from p in hfCrop.Value.Split(',')
                            select int.Parse(p)).ToArray();

            bmp = bmp.Clone(new System.Drawing.Rectangle(cropinfo[0], cropinfo[1], cropinfo[2], cropinfo[3]), bmp.PixelFormat);

            ImageResizer resizer = new ImageResizer();
            resizer.OutputFormat = ImageFormat.Jpeg;
            resizer.ImgQuality = IMAGE_QUALITY;
            resizer.MaxHeight = IMAGE_MAX_H;
            resizer.MaxWidth = IMAGE_MAX_W;

            byte[] imgRes = resizer.Resize(bmp);

            bmp.Dispose();

            string title = tbxAddAccomImageTitle.Text.Trim();
            string description = tbxAddAccomImageDescription.Text.Trim();

            string fileName = String.Format("accomm_{0}_image_{1}.jpg", accommodation.Name.Replace(' ', '_').Replace('+','-'), "{0}");
            string filePath = ACCOMIMAGE_UPLOAD_PATH.Last() == '/' ? ACCOMIMAGE_UPLOAD_PATH : ACCOMIMAGE_UPLOAD_PATH + "/";

            string srcLocation = filePath + fileName;

            UltimateDC.Image image = new UltimateDC.Image() { Src = "", Alt = "" };
            dc.Images.InsertOnSubmit(image);

            //Nakon submitchanges se pojavi id u image, a to mi treba za filename i src atribut
            dc.SubmitChanges();
            srcLocation = string.Format(srcLocation, image.Id);

            image.Src = string.Format(srcLocation, image.Id);
            image.Alt = accommodation.Name + " " + title;
            image.Title = title;
            image.Description = description;

            AccommodationImage accommImage = new AccommodationImage();
            dc.AccommodationImages.InsertOnSubmit(accommImage);
            accommImage.Image = image;
            accommImage.Accommodation = accommodation;

            dc.SubmitChanges();

            string saveLocation = Server.MapPath(srcLocation);
            if (!System.IO.Directory.Exists(saveLocation)) System.IO.Directory.CreateDirectory(Path.GetDirectoryName(saveLocation));

            File.WriteAllBytes(saveLocation, imgRes);

            imgThumb.Visible = false;

            RefreshImages();
        }

    }

    protected void btnUploadAccommImagePreview_Click(object sender, EventArgs e)
    {
        if (fuAddAccommImage.HasFile)
        {
            var file = fuAddAccommImage.PostedFile;
            string tip = file.ContentType;

            if (tip == "image/jpeg" || tip == "image/gif" || tip == "image/png")
            {
                BinaryReader br = new BinaryReader(file.InputStream);
                byte[] buffer = br.ReadBytes(file.ContentLength);
                Session.Add("imageuploadpreview", buffer);

                imgThumb.Src = String.Format("/manage/picpreview.aspx?what={0}&content={1}", "imageuploadpreview", Server.UrlEncode(tip));
                imgThumb.Alt = "preview";

                hfCrop.Value = string.Empty;
                imgThumb.Visible = true;
                rfvAddAccommImage.Visible = false;
                rfvAddAccommImage.Enabled = false;
            }
        }
    }

    protected void aDelete_Click(object sender, EventArgs e)
    {
        int agencyId = accommodation.AgencyId;

        dc.Accommodations.DeleteOnSubmit(accommodation);
        dc.SubmitChanges();

        Response.Redirect(String.Format("/manage/Agency.aspx?agencyid={0}", agencyId));
    }


    protected void PriceEdit_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "priceedit")
        {
            int priceId = int.Parse((string)e.CommandArgument);

            var price = dc.AccommodationPrices.Where(i => i.Id == priceId).SingleOrDefault();

            tbxPriceEditStart.Text = price.DateStart.HasValue ? FormatDate(price.DateStart.Value) : string.Empty;
            tbxPriceEditEnd.Text = price.DateEnd.HasValue ? FormatDate(price.DateEnd.Value) : string.Empty;
            tbxPriceEditName.Text = price.Name;
            tbxPriceEditValue.Text = price.Value.ToString("0.00", CultureInfo.InvariantCulture);
            ddlCurrency.SelectedValue = price.CurrencyId.ToString();

            pnlPriceEdit.Visible = true;

            btnPriceEditSave.CommandName = "pricesaveedit";
            btnPriceEditSave.CommandArgument = price.Id.ToString();
            btnPriceEditDelete.CommandName = "pricedelete";
            btnPriceEditDelete.CommandArgument = priceId.ToString();
            btnPriceEditDelete.Visible = true;
        }
        else if (e.CommandName == "pricenew")
        {
            pnlPriceEdit.Visible = true;

            tbxPriceEditName.Text = string.Empty;
            tbxPriceEditStart.Text = string.Empty;
            tbxPriceEditEnd.Text = string.Empty;
            tbxPriceEditValue.Text = string.Empty;

            btnPriceEditSave.CommandName = "pricesavenew";
            btnPriceEditDelete.Visible = false;

        }

    }

    protected void PriceSubmit_Command(object sender, CommandEventArgs e)
    {
        AccommodationPrice price = null;
        if (e.CommandName == "pricedelete")
        {
            int priceid = int.Parse((string)e.CommandArgument);

            price = dc.AccommodationPrices.Where(i => i.Id == priceid).SingleOrDefault();

            dc.AccommodationPrices.DeleteOnSubmit(price);

            if (accommodation.IsPublished) accommodation.IsDirty = true;

            dc.SubmitChanges();

            RefreshPrices();
        }
        else if (e.CommandName == "pricesavenew")
        {
            bool valid = false;
            Validate("priceedit");
            valid = IsValid;
            if (rbtnPriceCalendar.Checked)
            {
                Validate("priceeditcal");
                valid &= IsValid;
            }

            if (valid)
            {
                price = new AccommodationPrice();
                dc.AccommodationPrices.InsertOnSubmit(price);

                price.Accommodation = accommodation;
                price.Name = tbxPriceEditName.Text;
                price.Value = decimal.Parse(tbxPriceEditValue.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                price.CurrencyId = int.Parse(ddlCurrency.SelectedValue);

                if (rbtnPriceCalendar.Checked)
                {
                    price.DateStart = tbxPriceEditStart.Text.Trim() == string.Empty ? (DateTime?)null : DateTime.ParseExact(tbxPriceEditStart.Text, "dd.MM.yyyy.", CultureInfo.InvariantCulture);
                    price.DateEnd = tbxPriceEditEnd.Text.Trim() == string.Empty ? (DateTime?)null : DateTime.ParseExact(tbxPriceEditEnd.Text, "dd.MM.yyyy.", CultureInfo.InvariantCulture);
                }
                else if (rbtnPriceWholeYear.Checked)
                {
                    price.DateStart = null;
                    price.DateEnd = null;
                }

                if (accommodation.IsPublished) accommodation.IsDirty = true;

                dc.SubmitChanges();

                pnlPriceEdit.Visible = false;
                ltlStatus.Text = "Unos uspješan!";

                RefreshPrices();
            }
        }
        else if (e.CommandName == "pricesaveedit")
        {
            bool valid = false;
            Validate("priceedit");
            valid = IsValid;
            if (rbtnPriceCalendar.Checked)
            {
                Validate("priceeditcal");
                valid &= IsValid;
            }

            if (valid)
            {

                int priceid = int.Parse((string)e.CommandArgument);

                price = dc.AccommodationPrices.Where(i => i.Id == priceid).SingleOrDefault();

                price.Accommodation = accommodation;
                price.Name = tbxPriceEditName.Text;
                price.DateStart = tbxPriceEditStart.Text.Trim() == string.Empty ? (DateTime?)null : DateTime.Parse(tbxPriceEditStart.Text);
                price.DateEnd = tbxPriceEditEnd.Text.Trim() == string.Empty ? (DateTime?)null : DateTime.Parse(tbxPriceEditEnd.Text);
                price.Value = decimal.Parse(tbxPriceEditValue.Text);
                price.CurrencyId = int.Parse(ddlCurrency.SelectedValue);

                if (rbtnPriceCalendar.Checked)
                {
                    price.DateStart = tbxPriceEditStart.Text.Trim() == string.Empty ? (DateTime?)null : DateTime.ParseExact(tbxPriceEditStart.Text, "dd.MM.yyyy.", CultureInfo.InvariantCulture);
                    price.DateEnd = tbxPriceEditEnd.Text.Trim() == string.Empty ? (DateTime?)null : DateTime.ParseExact(tbxPriceEditEnd.Text, "dd.MM.yyyy.", CultureInfo.InvariantCulture);
                }
                else if (rbtnPriceWholeYear.Checked)
                {
                    price.DateStart = null;
                    price.DateEnd = null;
                }

                if (accommodation.IsPublished) accommodation.IsDirty = true;

                dc.SubmitChanges();

                pnlPriceEdit.Visible = false;
                ltlStatus.Text = "Promjene spremljene!";

                RefreshPrices();
            }
        }

    }


    private void RefreshDescriptions()
    {
        var descriptions = accommodation.AccommodationDescriptions;

        repDescriptions.DataSource = descriptions;
        repDescriptions.DataBind();

        var adddesclanguages = from p in dc.Languages
                               where !descriptions.Select(i => i.LanguageId).Contains(p.Id)
                               select new
                               {
                                   Id = p.Id,
                                   Title = p.Title
                               };

        ddlAddDescLangs.DataSource = adddesclanguages;
        ddlAddDescLangs.DataValueField = "Id";
        ddlAddDescLangs.DataTextField = "Title";
        ddlAddDescLangs.DataBind();
    }
    private void RefreshImages()
    {
        if (accommodation.AccommodationImages.Any())
        {
            repAccommodationImages.DataSource = accommodation.AccommodationImages.Select(i => i.Image);
            repAccommodationImages.DataBind();
            repAccommodationImages.Visible = true;
        }
        else
        {
            repAccommodationImages.Visible = false;
        }
    }
    private void RefreshPrices()
    {
        if (accommodation.AccommodationPrices.Any())
        {
            repPrices.DataSource = accommodation.AccommodationPrices.OrderBy(i => i.DateStart);
            repPrices.DataBind();
            repPrices.Visible = true;
        }
        else
        {
            repPrices.Visible = false;
        }
    }

    protected string FormatDate(DateTime date)
    {
        return date.ToString("dd.MM.yyyy.", CultureInfo.InvariantCulture);
    }
    protected string FormatDate(DateTime? start, DateTime? end)
    {
        string result = string.Empty;

        if (start.HasValue && end.HasValue)
        {
            result = String.Format("{0} - {1}", FormatDate(start.Value), FormatDate(end.Value));
        }
        else
        {
            result = "Cijela godina";
        }
        return result;
    }


    protected void btnPublishAccomAdvert_Click(object sender, EventArgs e)
    {
        if (!accommodation.AccommodationPrices.Any())
        {
            Common.JavascripAlert("Ne možete objaviti oglas dok ne unesete barem jednu cijenu!", this);
        }
        else if (!accommodation.AccommodationImages.Any())
        {
            Common.JavascripAlert("Ne možete objaviti oglas dok ne unesete barem jednu sliku!", this);
        }
        else
        {
            dc.ProcessAccommodationAdvert(accommodation.Id);
            HttpContext.Current.Items["statusmessage"] = "Uspješan publish!";
            Server.Transfer(String.Format("/manage/accommodation.aspx?accommid={0}&action=entrysuccess", accommodation.Id));
        }

    }
}