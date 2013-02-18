using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;
using System.Globalization;
using System.Text.RegularExpressions;

public partial class AgencyListing : System.Web.UI.Page
{

    UltimateDataContext dc;
    UltimateDC.Accommodation accommodation;

    protected string Latitude = string.Empty;
    protected string Longitude = string.Empty;
    protected string Address = string.Empty;
    protected string City = string.Empty;
    protected string Country = string.Empty;
    protected string AccommodationName = String.Empty;


    protected override void InitializeCulture()
    {
        var selectedCulture = Common.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);

        this.Title = string.Empty;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        dc = new UltimateDataContext();

        FetchAccommodation();

        ContactAgency1.AgencyEmail = accommodation.Agency.ContactEmail;
        ContactAgency1.AgencyName = accommodation.Agency.Name;

        if (!IsPostBack)
        {
            SetJavascriptVariables();

            int languageId = Common.GetLanguageId();
            var agency = accommodation.Agency;

            this.Title = String.Format(Resources.placeberry.AgencyListing_Title, agency.Name + " - " + accommodation.Name);

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
            ltlAgencyAdress.Text = String.Format("{0}, {1}, {2}", agency.Address, agency.City, agency.Country);
            ltlAgencyEmail.Text = agency.ContactEmail;
            ltlAgencyPhones.Text = agency.ContactPhone;
            if (agency.Image != null)
            {
                imgAgencyLogo.Src = "/thumb.aspx?src=" + agency.Image.Src + "&mw=185&mh=160&crop=1";
                imgAgencyLogo.Alt = agency.Image.Alt;
            }
            else
            {
                imgAgencyLogo.Src = "/resources/noimg.jpg";
                imgAgencyLogo.Alt = "";
            }
            aAgencyWebsite.HRef = agency.UrlWebsite ?? "#";
            aAgencyWebsite.InnerText = agency.UrlWebsite ?? string.Empty;

            ltlListingTitle.Text = accommodation.Name;
            ltlListingType.Text = dc.GetTraslation(accommodation.TypeId, languageId);
            ltlListingCapacity.Text = Common.FormatCapacity(accommodation.CapacityMin, accommodation.CapacityMax);


            var descriptions = from p in accommodation.AccommodationDescriptions
                               where p.LanguageId == languageId || p.LanguageId == accommodation.Agency.LangaugeId
                               select p;
            string translation = descriptions.Where(i => i.LanguageId == languageId).Select(i => i.Description).SingleOrDefault() ?? descriptions.Where(i => i.LanguageId == agency.LangaugeId).Select(i => i.Description).SingleOrDefault();
            ltlListingDescription.Text = translation ?? string.Empty;

            string location = dc.GetTraslation(accommodation.AccommodationCity.UltimateTableId, languageId);
            var parents = (from p in dc.GetParents(null, accommodation.AccommodationCity.UltimateTableId, languageId)
                           orderby p.Lvl
                           select p.ParentTitle).ToArray();
            if (parents.Any())
            {
                location += ", " + string.Join(", ", parents);
            }
            ltlListingLocation.Text = location;

            RefeshGallery();
            RefreshPrices();
        }
    }

    private void FetchAccommodation()
    {
        int languageId = Common.GetLanguageId();

        string accommTag = Common.FixQueryStringUrlTag(Request.QueryString["accommtag"] ?? string.Empty);
        string agencyTag = Common.FixQueryStringUrlTag(Request.QueryString["agencytag"] ?? string.Empty);

        accommodation = (from p in dc.AccommodationUrlTags
                         join q in dc.AgencyUrlTags on p.AgencyId equals q.AgencyId
                         where q.UrlTag == agencyTag && p.UrlTag == accommTag && p.LanguageId == languageId
                         select p.Accommodation).SingleOrDefault();

        if (accommodation == null)
        {
            //Ne postoji
            Response.Redirect("/", true);
        }
    }

    private void RefeshGallery()
    {
        var images = accommodation.AccommodationImages;
        if (images.Any())
        {
            repListingGallery.DataSource = images.Select(i => i.Image);
            repListingGallery.DataBind();
        }
    }
    private void RefreshPrices()
    {
        ltlPrice.Text = string.Empty;

        var prices = accommodation.AccommodationPrices;
        if (prices.Any())
        {
            repListingPrices.DataSource = prices;
            repListingPrices.DataBind();

            var priceswithdates = from p in prices
                                  where p.DateStart != null && p.DateEnd != null
                                  select p;

            if (priceswithdates.Any())
            {
                var pricenow = (from p in priceswithdates
                                where DateTime.Today >= p.DateStart && DateTime.Today <= p.DateEnd
                                orderby p.Id descending
                                select p).Take(1).SingleOrDefault();
                if (pricenow != null)
                {
                    ltlPrice.Text = String.Format("{0:0.} {1}", pricenow.Value, pricenow.Currency.Symbol);
                    ltlPriceDescription.Text = Resources.placeberry.Vacation_PerDay;
                }
                else
                {
                    var yearprice = (from p in prices
                                     where p.DateStart == null && p.DateEnd == null
                                     orderby p.Id descending
                                     select p).Take(1).SingleOrDefault();
                    if (yearprice != null)
                    {
                        ltlPrice.Text = String.Format("{0:0.} {1}", yearprice.Value, yearprice.Currency.Symbol);
                        ltlPriceDescription.Text = Resources.placeberry.Vacation_PerDay;
                    }
                }
            }
            else
            {
                var yearprice = (from p in prices
                                 where p.DateStart == null && p.DateEnd == null
                                 orderby p.Id descending
                                 select p).Take(1).SingleOrDefault();
                if (yearprice != null)
                {
                    ltlPrice.Text = String.Format("{0:0.} {1}", yearprice.Value, yearprice.Currency.Symbol);
                    ltlPriceDescription.Text = Resources.placeberry.Vacation_PerDay;
                }
            }
        }
    }

    private void SetJavascriptVariables()
    {
        Latitude = accommodation.Latitude.HasValue ? accommodation.Latitude.Value.ToString("###.######", CultureInfo.InvariantCulture) : string.Empty;
        Longitude = accommodation.Longitude.HasValue ? accommodation.Longitude.Value.ToString("###.######", CultureInfo.InvariantCulture) : string.Empty;
        Address = accommodation.Address ?? string.Empty;
        Country = accommodation.AccommodationCity.Country ?? string.Empty;
        City = accommodation.AccommodationCity.City ?? string.Empty;
        AccommodationName = accommodation.Name ?? string.Empty;


    }

    protected string FormatPriceDate(object dateStart, object dateEnd)
    {
        string result = string.Empty;

        bool start = dateStart != null;
        bool end = dateEnd != null;


        if (start && end)
        {
            result = String.Format("{0} - {1}", ((DateTime)dateStart).ToShortDateString(), ((DateTime)dateEnd).ToShortDateString());
        }
        else if (!start && !end)
        {
            DateTime dateS = new DateTime(DateTime.Today.Year, 1, 1);
            DateTime dateE = new DateTime(DateTime.Today.Year, 12, 31);
            result = String.Format("{0} - {1}", dateS.ToShortDateString(), dateE.ToShortDateString());
        }

        return result;
    }



}