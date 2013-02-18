using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Placeberry.DAL;
using UltimateDC;
using AjaxControlToolkit;

public partial class AdvertRawItem : System.Web.UI.Page
{
    UltimateDataContext dc;

    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        dc = new UltimateDataContext();

        if (!IsPostBack)
        {
            var countrys = from p in dc.UltimateTables
                           where (ObjectType)p.ObjectTypeId == ObjectType.COUNTRY
                           orderby p.Title ascending
                           select p.Title;

            cbxCountry.DataSource = countrys;
            cbxCountry.DataBind();

            var languages = (from p in dc.Languages
                             orderby p.Title ascending
                             select new { Id = p.Id, Title = p.Title }).ToList();
            languages.Insert(0, new { Id = 0, Title = string.Empty });

            ddlLanguage.DataSource = languages;
            ddlLanguage.DataTextField = "Title";
            ddlLanguage.DataValueField = "Id";
            ddlLanguage.DataBind();

            if (string.IsNullOrEmpty(Request["id"]))
            {
                int agencyId;
                if (!int.TryParse(Request["agencyId"], out agencyId))
                    Response.Redirect("/Manage/Customer.aspx");

                litNaslov.Text = "Novi zapis";
            }
            else
            {
                litNaslov.Text = "Uređivanje zapisa";

                int advertId;
                if (!int.TryParse(Request["id"], out advertId))
                    Response.Redirect("/Manage/Customer.aspx");

                LoadAdvert(advertId);
            }
        }
        else
        {
            Validate();
        }


    }

    private void LoadAdvert(int advertId)
    {
        var resultSet = GetAdvertRaw.Execute(advertId, (Guid)Membership.GetUser().ProviderUserKey);

        if (resultSet.Count == 0) // ne postoji ili korisnik nema prava
        {
            Response.Redirect("/Manage/Customer.aspx");
        }

        var x = resultSet[0];

        txtAccommSubType.Text = x.AccommSubType;
        txtAccommType.Text = x.AccommType;
        txtActivities.Text = x.Activities;
        txtAdvertCode.Text = x.AdvertCode;
        txtBeach.Text = x.Beach;
        txtBeachDistanceM.Text = x.BeachDistanceM;
        txtDateDesc.Text = x.DateDesc;
        txtDaysNum.Text = x.DaysNum;
        txtDescription.Text = x.Description;
        txtFacilities.Text = x.Facilities;
        txtGroupSubType.Text = x.GroupSubType;
        txtGroupType.Text = x.GroupType;
        txtInfoDesc.Text = x.InfoDesc;
        ddlLanguage.SelectedValue = x.LanguageId.HasValue ? x.LanguageId.Value.ToString() : string.Empty;
        txtLocationDesc.Text = x.LocationDesc;
        txtPetsDesc.Text = x.PetsDesc;
        txtPictureUrl.Text = x.PictureUrl;
        txtPriceDesc.Text = x.PriceDesc;
        txtPriceFrom.Text = x.PriceFrom;
        txtPriceOld.Text = x.PriceOld;
        txtSource.Text = x.Source;
        txtSourceCategory.Text = x.SourceCategory;
        txtStars.Text = x.Stars;
        txtTitle.Text = x.Title;
        txtUrlLink.Text = x.UrlLink;
        txtVacationType.Text = x.VacationType;
        txtDistanceFromCentreM.Text = x.DistanceFromCentreM;

        txtDate1.Text = x.Date1;
        txtDate2.Text = x.Date2;


        var regions = from p in dc.UltimateTableRelations
                      let parent = from a in dc.UltimateTableRelations
                                   where p.Child.Title == x.Region
                                   select p.Parent
                      where parent.Contains(p.Parent) && (ObjectType)p.Child.ObjectTypeId == ObjectType.REGION
                      orderby p.Child.Title
                      select p.Child.Title;
        var subregions = from p in dc.UltimateTableRelations
                         let parent = from a in dc.UltimateTableRelations
                                      where a.Child.Title == x.Subregion
                                      select a.Parent
                         where parent.Contains(p.Parent) && (ObjectType)p.Child.ObjectTypeId == ObjectType.SUBREGION
                         orderby p.Child.Title
                         select p.Child.Title;
        var islands = from p in dc.UltimateTableRelations
                      let parent = from a in dc.UltimateTableRelations
                                   where a.Child.Title == x.Island
                                   select a.Parent
                      where parent.Contains(p.Parent) && (ObjectType)p.Child.ObjectTypeId == ObjectType.ISLAND
                      orderby p.Child.Title
                      select p.Child.Title;
        var cities = from p in dc.UltimateTableRelations
                     let parent = from a in dc.UltimateTableRelations
                                  where a.Child.Title == x.City
                                  select a.Parent
                     where parent.Contains(p.Parent) && (ObjectType)p.Child.ObjectTypeId == ObjectType.CITY
                     orderby p.Child.Title
                     select p.Child.Title;

        cbxRegion.DataSource = regions;
        cbxSubregion.DataSource = subregions;
        cbxIsland.DataSource = islands;
        cbxCity.DataSource = cities;

        cbxRegion.DataBind();
        cbxSubregion.DataBind();
        cbxIsland.DataBind();
        cbxCity.DataBind();

        cbxCountry.Text = x.Country;
        cbxRegion.Text = x.Region;
        cbxSubregion.Text = x.Subregion;
        cbxIsland.Text = x.Island;
        cbxCity.Text = x.City;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Validate();
        if (IsValid)
        {
            int advertId;
            int agencyId;
            if (int.TryParse(Request["id"], out advertId))
            {
                UpdateAdvertRaw.Execute(advertId,
                                        txtSource.Text,
                                        ddlLanguage.SelectedItem.Text,
                                        txtGroupType.Text,
                                        txtGroupSubType.Text,
                                        txtSourceCategory.Text,
                                        txtTitle.Text,
                                        txtAccommType.Text,
                                        txtAccommSubType.Text,
                                        txtVacationType.Text,
                                        txtAdvertCode.Text,
                                        txtUrlLink.Text,
                                        txtPictureUrl.Text,
                                        txtStars.Text,
                                        txtLocationDesc.Text,
                                        cbxCountry.Text,
                                        cbxRegion.Text,
                                        cbxSubregion.Text,
                                        cbxIsland.Text,
                                        cbxCity.Text,
                                        txtPriceOld.Text,
                                        txtPriceFrom.Text,
                                        txtPriceDesc.Text,
                                        txtDate1.Text,
                                        txtDate2.Text,
                                        txtDateDesc.Text,
                                        txtDaysNum.Text,
                                        txtDescription.Text,
                                        txtActivities.Text,
                                        txtFacilities.Text,
                                        txtBeach.Text,
                                        txtBeachDistanceM.Text,
                                        txtDistanceFromCentreM.Text,
                                        txtPetsDesc.Text,
                                        txtInfoDesc.Text);

                litInfo.Text = "Unos ažuriran.";

            }
            else if (int.TryParse(Request["agencyId"], out agencyId))
            {
                InsertAdvertRaw.Execute(agencyId,
                                        null,
                                        int.Parse(ddlLanguage.SelectedValue),
                                        null,
                                        "U",
                                        true,
                                        txtSource.Text,
                                        ddlLanguage.SelectedItem.Text,
                                        txtGroupType.Text,
                                        txtGroupSubType.Text,
                                        txtSourceCategory.Text,
                                        txtTitle.Text,
                                        txtAccommType.Text,
                                        txtAccommSubType.Text,
                                        txtVacationType.Text,
                                        txtAdvertCode.Text,
                                        txtUrlLink.Text,
                                        txtPictureUrl.Text,
                                        txtStars.Text,
                                        txtLocationDesc.Text,
                                        cbxCountry.Text,
                                        cbxRegion.Text,
                                        cbxSubregion.Text,
                                        cbxIsland.Text,
                                        cbxCity.Text,
                                        txtPriceOld.Text,
                                        txtPriceFrom.Text,
                                        txtPriceDesc.Text,
                                        txtDate1.Text,
                                        txtDate2.Text,
                                        txtDateDesc.Text,
                                        txtDaysNum.Text,
                                        txtDescription.Text,
                                        txtActivities.Text,
                                        txtFacilities.Text,
                                        txtBeach.Text,
                                        txtBeachDistanceM.Text,
                                        txtDistanceFromCentreM.Text,
                                        txtPetsDesc.Text,
                                        txtInfoDesc.Text);

                txtSource.Text =
                    txtGroupType.Text =
                    txtGroupSubType.Text =
                    txtSourceCategory.Text =
                    txtTitle.Text =
                    txtAccommType.Text =
                    txtAccommSubType.Text =
                    txtVacationType.Text =
                    txtAdvertCode.Text =
                    txtUrlLink.Text =
                    txtPictureUrl.Text =
                    txtStars.Text =
                    txtLocationDesc.Text =
                    txtPriceOld.Text =
                    txtPriceFrom.Text =
                    txtPriceDesc.Text =
                    txtDate1.Text =
                    txtDate2.Text =
                    txtDateDesc.Text =
                    txtDaysNum.Text =
                    txtDescription.Text =
                    txtActivities.Text =
                    txtFacilities.Text =
                    txtBeach.Text =
                    txtBeachDistanceM.Text =
                    txtDistanceFromCentreM.Text =
                    txtPetsDesc.Text =
                    txtInfoDesc.Text = "";

                litInfo.Text = "Novi unos uspiješno dodan.";

            }
        }
    }
    protected void btnPovratak_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Manage/AdvertRaw.aspx");
    }

    protected void cbxCountry_TextChanged(object sender, EventArgs e)
    {
        string country = cbxCountry.Text;

        ResetCombobox(cbxRegion);
        ResetCombobox(cbxSubregion);
        ResetCombobox(cbxIsland);
        ResetCombobox(cbxCity);

        if (!string.IsNullOrEmpty(country))
        {
            var regions = (from p in dc.UltimateTableRelations
                           where p.Parent.Title.ToLower() == country.ToLower() &&
                                 (ObjectType)p.Child.ObjectTypeId == ObjectType.REGION
                           orderby p.Child.Title ascending
                           select p.Child.Title).ToList();

            regions.Insert(0, string.Empty);
            cbxRegion.DataSource = regions;
            cbxRegion.DataBind();
        }
    }
    protected void cbxRegion_TextChanged(object sender, EventArgs e)
    {
        string region = cbxRegion.Text;

        ResetCombobox(cbxSubregion);
        ResetCombobox(cbxIsland);
        ResetCombobox(cbxCity);

        if (!string.IsNullOrEmpty(region))
        {
            region = region.Trim().ToLower();

            var query = (from p in dc.UltimateTableRelations
                         where p.Parent.Title.ToLower() == region
                         orderby p.Child.Title ascending
                         select new
                         {
                             Title = p.Child.Title,
                             ObjectTypeId = p.Child.ObjectTypeId
                         }).ToList();

            var subregions = (from p in query
                              where (ObjectType)p.ObjectTypeId == ObjectType.SUBREGION
                              select p.Title).ToList();
            var islands = (from p in query
                           where (ObjectType)p.ObjectTypeId == ObjectType.ISLAND
                           select p.Title).ToList();
            var citys = (from p in query
                         where (ObjectType)p.ObjectTypeId == ObjectType.CITY
                         select p.Title).ToList();

            subregions.Insert(0, string.Empty);
            islands.Insert(0, string.Empty);
            citys.Insert(0, string.Empty);

            cbxSubregion.DataSource = subregions;
            cbxIsland.DataSource = islands;
            cbxCity.DataSource = citys;

            cbxSubregion.DataBind();
            cbxIsland.DataBind();
            cbxCity.DataBind();
        }
    }
    protected void cbxSubregion_TextChanged(object sender, EventArgs e)
    {
        string subregion = cbxSubregion.Text;

        ResetCombobox(cbxIsland);
        ResetCombobox(cbxCity);

        if (!string.IsNullOrEmpty(subregion))
        {
            subregion = subregion.Trim().ToLower();

            var query = (from p in dc.UltimateTableRelations
                         where p.Parent.Title.ToLower() == subregion
                         orderby p.Child.Title ascending
                         select new
                         {
                             Title = p.Child.Title,
                             ObjectTypeId = p.Child.ObjectTypeId
                         }).ToList();

            var islands = (from p in query
                           where (ObjectType)p.ObjectTypeId == ObjectType.ISLAND
                           select p.Title).ToList();
            var citys = (from p in query
                         where (ObjectType)p.ObjectTypeId == ObjectType.CITY
                         select p.Title).ToList();

            islands.Insert(0, string.Empty);
            citys.Insert(0, string.Empty);

            cbxIsland.DataSource = islands;
            cbxCity.DataSource = citys;

            cbxIsland.DataBind();
            cbxCity.DataBind();
        }
    }
    protected void cbxIsland_TextChanged(object sender, EventArgs e)
    {
        string island = cbxIsland.Text;

        ResetCombobox(cbxCity);

        if (!string.IsNullOrEmpty(island))
        {
            island = island.Trim().ToLower();

            var query = (from p in dc.UltimateTableRelations
                         where p.Parent.Title.ToLower() == island
                         orderby p.Child.Title ascending
                         select new
                         {
                             Title = p.Child.Title,
                             ObjectTypeId = p.Child.ObjectTypeId
                         }).ToList();

            var citys = (from p in query
                         where (ObjectType)p.ObjectTypeId == ObjectType.CITY
                         select p.Title).ToList();

            citys.Insert(0, string.Empty);
            cbxCity.DataSource = citys;
            cbxCity.DataBind();
        }
    }
    protected void cbxCity_TextChanged(object sender, EventArgs e)
    {
    }

    private void ResetCombobox(ComboBox cbx)
    {
        cbx.Items.Clear();
        cbx.Text = string.Empty;
        cbx.SelectedIndex = -1;
    }

}