using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class admin_OfferCreateEdit : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.btnSaveOffer.Click += new EventHandler(btnSaveOffer_Click);
        this.rptPlaces.ItemDataBound += new RepeaterItemEventHandler(rptPlaces_ItemDataBound);
    }

    void rptPlaces_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Collective.Place pl = e.Item.DataItem as Collective.Place;
            CheckBox cb = e.Item.FindControl("cbPlace") as CheckBox;
            cb.Attributes.Add("placeId", pl.Id.ToString());

            if (GetOffer() != null)
            {
                if (GetOfferPlaces().Where(p => p.Id == pl.Id).Any())
                {
                    cb.Checked = true;
                }
            }
        }
    }

    protected override void InitializeCulture()
    {
        var selectedCulture = PutovalicaUtil.GetCurrentCulture();
        System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedCulture.Name);
    }

    private bool fillOfferValues = true;

    void btnSaveOffer_Click(object sender, EventArgs e)
    {
        int? agencyId = null;
        int collectiveCategoryId = Int32.Parse(ddlCategories.SelectedValue);
        string name = tbName.Text;
        double priceReal;
        double priceSave;
        double price;
        int currencyId = Int32.Parse(ddlCurrency.SelectedValue);
        Collective.OfferStatusType status = (Collective.OfferStatusType)Char.Parse(ddlStatus.SelectedValue);
        double? discount = null;
        DateTime dateStart;
        DateTime dateEnd;
        DateTime dateCouponStart;
        DateTime dateCouponEnd;
        int? numberOfPersons = null;
        int? numberOfCouponsPerUser = null;
        string offerType = String.IsNullOrEmpty(tbOfferType.Text) == false ? tbOfferType.Text : null;
        double? longitude = null;
        double? latitude = null;
        int minBoughtCount;
        int maxBoughtCount;
        int priority;
        bool active = cbActive.Checked;

        int agid;
        if (Int32.TryParse(ddlAgencies.SelectedValue, out agid))
        {
            agencyId = (int?)agid;
        }

        this.lblErrMsg.Text = string.Empty;
        this.lblMsgSuccess.Text = string.Empty;

        // cijene
        if (!Double.TryParse(tbPriceReal.Text.Replace('.', ','), out priceReal))
        {
            this.lblErrMsg.Text = "Pogrešno unesena \"Prava cijena\".";
            fillOfferValues = false;
            return;
        }
        if (!Double.TryParse(tbPriceSave.Text.Replace('.', ','), out priceSave))
        {
            this.lblErrMsg.Text = "Pogrešno unesena \"Ušteda\".";
            fillOfferValues = false;
            return;
        }
        if (!Double.TryParse(tbPrice.Text.Replace('.', ','), out price))
        {
            this.lblErrMsg.Text = "Pogrešno unesena \"Cijena\".";
            fillOfferValues = false;
            return;
        }
        if (!String.IsNullOrEmpty(tbDiscount.Text))
        {
            double dis;
            if (!Double.TryParse(tbDiscount.Text.Replace('.', ','), out dis))
            {
                this.lblErrMsg.Text = "Pogrešno unesen \"Popust\".";
                fillOfferValues = false;
                return;
            }
            discount = (double?)dis;
        }

        // odabrani datumi
        if (!DateTime.TryParse(tbDateStart.Text, out dateStart))
        {
            this.lblErrMsg.Text = "Pogrešno unesen \"Datum početka ponude\".";
            fillOfferValues = false;
            return;
        }

        if (!DateTime.TryParse(tbDateEnd.Text, out dateEnd))
        {
            this.lblErrMsg.Text = "Pogrešno unesen \"Datum završetka ponude\".";
            fillOfferValues = false;
            return;
        }
        if (!DateTime.TryParse(tbDateCouponStart.Text, out dateCouponStart))
        {
            this.lblErrMsg.Text = "Pogrešno unesen \"Datum početka kupona\".";
            fillOfferValues = false;
            return;
        }
        if (!DateTime.TryParse(tbDateCouponEnd.Text, out dateCouponEnd))
        {
            this.lblErrMsg.Text = "Pogrešno unesen \"Datum završetka kupona\".";
            fillOfferValues = false;
            return;
        }

        // broj osoba
        int numOfPer;
        if (!String.IsNullOrEmpty(tbNumberOfPersons.Text))
        {
            if (!Int32.TryParse(tbNumberOfPersons.Text, out numOfPer))
            {
                this.lblErrMsg.Text = "Pogrešno unesen \"Broj osoba\".";
                fillOfferValues = false;
                return;
            }
            numberOfPersons = (int?)numOfPer;
        }
        int numOfCoupPerPer;
        if (!String.IsNullOrEmpty(tbNumberOfCouponsPerUser.Text))
        {
            if (!Int32.TryParse(tbNumberOfCouponsPerUser.Text, out numOfCoupPerPer))
            {
                this.lblErrMsg.Text = "Pogrešno unesen \"Broj kupona po osobi\".";
                fillOfferValues = false;
                return;
            }
            numberOfCouponsPerUser = (int?)numOfCoupPerPer;
        }

        // longitude, latitude
        if (!String.IsNullOrEmpty(tbLongitude.Text))
        {
            double lng;
            if (!Double.TryParse(tbLongitude.Text.Replace('.', ','), out lng))
            {
                this.lblErrMsg.Text = "Pogrešno unesen \"Longitude\".";
                fillOfferValues = false;
                return;
            }
            longitude = (double?)lng;
        }
        if (!String.IsNullOrEmpty(tbLatitude.Text))
        {
            double lat;
            if (!Double.TryParse(tbLatitude.Text.Replace('.', ','), out lat))
            {
                this.lblErrMsg.Text = "Pogrešno unesen \"Latitude\".";
                fillOfferValues = false;
                return;
            }
            latitude = (double?)lat;
        }
        // min max bought count
        if (!Int32.TryParse(tbMinBoughtCount.Text, out minBoughtCount))
        {
            this.lblErrMsg.Text = "Pogrešno uneseno \"Minimalano kupona za prodaju\".";
            fillOfferValues = false;
            return;
        }
        if (!Int32.TryParse(tbMaxBoughtCount.Text, out maxBoughtCount))
        {
            this.lblErrMsg.Text = "Pogrešno uneseno \"Maksimalno kupona za prodaju\".";
            fillOfferValues = false;
            return;
        }

        // priority
        if (String.IsNullOrEmpty(tbPriority.Text))
        {
            priority = 0;
        }
        else if (!Int32.TryParse(tbPriority.Text, out priority))
        {
            this.lblErrMsg.Text = "Pogrešno unesen \"Prioritet\".";
            fillOfferValues = false;
            return;
        }

        if (GetOffer() == null)
        {
            Collective.Offer offer = Collective.Offer.CreateOffer(null, agencyId, collectiveCategoryId, name, priceReal, priceSave, price, currencyId, status, discount,
                dateStart, dateEnd, dateCouponStart, dateCouponEnd, numberOfPersons, numberOfCouponsPerUser, minBoughtCount, maxBoughtCount, offerType, longitude, latitude, priority, active, GetPlacesIds());
            Response.Redirect(Request.Url.AbsoluteUri.Substring(0, (Request.Url.AbsoluteUri.IndexOf(".aspx") + ".aspx".Length)) + "?offerid=" + offer.OfferId.ToString() + "&msg=success");
            //Response.Redirect("http://" + Request.Url.Authority + "/manage/CollectiveOfferList.aspx");
        }
        else
        {
            _offer = Collective.Offer.UpdateOffer(GetOffer().OfferId, null, agencyId, collectiveCategoryId, name, priceReal, priceSave, price, currencyId, status, discount,
                dateStart, dateEnd, dateCouponStart, dateCouponEnd, numberOfPersons, numberOfCouponsPerUser, minBoughtCount, maxBoughtCount, offerType, longitude, latitude, priority, active, GetPlacesIds());

            this.lblMsgSuccess.Text = "Izmjene uspješno spremljene.";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["msg"]) && Request.QueryString["msg"] == "success")
            {
                this.lblMsgSuccess.Text = "Ponuda uspješno kreirana.";
            }

            // Agencije DropDown
            this.ddlAgencies.Items.Add(new ListItem("...", String.Empty));
            foreach (Collective.Agency ag in GetAgencies())
            {
                this.ddlAgencies.Items.Add(new ListItem(ag.Name, ag.Id.ToString()));
            }

            // Kategorije DropDown
            foreach (Collective.Category cat in GetCategories())
            {
                this.ddlCategories.Items.Add(new ListItem(cat.Name, cat.Id.ToString()));
            }

            // Valuta DropDown
            foreach (Collective.Currency curr in GetCurrencies())
            {
                this.ddlCurrency.Items.Add(new ListItem(curr.Title, curr.Id.ToString()));
            }

            // Status: I-inactive, A-active, C-completed, F-failed (iz descriptiona u bazi)
            foreach (Collective.OfferStatusType os in Enum.GetValues(typeof(Collective.OfferStatusType)))
            {
                this.ddlStatus.Items.Add(new ListItem(os.ToString(), ((char)os).ToString()));
            }

            // Places
            this.rptPlaces.DataSource = GetAllPlaces();
            this.rptPlaces.DataBind();
        }
        else
        {
            this.lblMsgSuccess.Text = string.Empty;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        Collective.Offer offer = GetOffer();

        if (offer != null)
        {
            this.lblOperationTitle.Text = "Uređivanje ponude";
            this.btnSaveOffer.Text = "Spremi promjene";
            this.trId.Visible = this.trClickCount.Visible = this.trBoughtCount.Visible = true;
            this.tbId.Text = offer.OfferId.ToString();

            if (fillOfferValues)
            {
                this.tbName.Text = offer.OfferName;
                if (offer.AgencyId.HasValue)
                {
                    this.ddlAgencies.SelectedValue = offer.AgencyId.Value.ToString();
                }
                this.ddlCategories.SelectedValue = offer.CollectiveCategoryId.ToString();
                this.tbPriceReal.Text = offer.PriceReal.ToString();
                this.tbPriceSave.Text = offer.PriceSave.ToString();
                this.tbPrice.Text = offer.Price.ToString();
                this.tbDiscount.Text = offer.Discount.HasValue ? offer.Discount.Value.ToString() : string.Empty;
                this.ddlCurrency.SelectedValue = offer.CurrencyId.ToString();
                this.ddlStatus.SelectedValue = ((char)offer.OfferStatus).ToString();
                this.tbDateStart.Text = offer.DateStart.ToString("dd.MM.yyyy");
                this.tbDateEnd.Text = offer.DateEnd.ToString("dd.MM.yyyy");
                this.tbDateCouponStart.Text = offer.DateCouponStart.ToString("dd.MM.yyyy");
                this.tbDateCouponEnd.Text = offer.DateCouponEnd.ToString("dd.MM.yyyy");
                this.tbNumberOfPersons.Text = offer.NumberOfPersons.HasValue ? offer.NumberOfPersons.Value.ToString() : string.Empty;
                this.tbNumberOfCouponsPerUser.Text = offer.NumberOfCouponsPerUser.HasValue ? offer.NumberOfCouponsPerUser.Value.ToString() : string.Empty;
                this.tbOfferType.Text = !String.IsNullOrEmpty(offer.OfferType) ? offer.OfferType : string.Empty;
                this.tbLongitude.Text = offer.Longitude.HasValue ? offer.Longitude.Value.ToString() : string.Empty;
                this.tbLatitude.Text = offer.Latitude.HasValue ? offer.Latitude.Value.ToString() : string.Empty;
                this.tbMinBoughtCount.Text = offer.MinBoughtCount.ToString();
                this.tbMaxBoughtCount.Text = offer.MaxBoughtCount.ToString();
                this.tbClickCount.Text = offer.ClickCount.ToString();
                this.tbBoughtCount.Text = offer.BoughtCount.ToString();
                this.tbPriority.Text = offer.Priority.ToString();
                this.cbActive.Checked = offer.Active;
            }
        }
        else
        {
            this.lblOperationTitle.Text = "Kreiranje ponude";
            this.btnSaveOffer.Text = "Kreiraj ponudu";
            this.trId.Visible = this.trClickCount.Visible = this.trBoughtCount.Visible = false;
        }
    }

    private List<int> GetPlacesIds()
    {
        List<int> lst = new List<int>();

        foreach (Control ctrl in this.rptPlaces.Controls)
        {
            if (ctrl.GetType() == typeof(RepeaterItem))
            {
                RepeaterItem repIt = (RepeaterItem)ctrl;
                if (repIt.ItemType == ListItemType.Item || repIt.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox cbPlace = repIt.FindControl("cbPlace") as CheckBox;
                    if (cbPlace.Checked)
                    {
                        lst.Add(Int32.Parse(cbPlace.Attributes["placeId"]));
                    }
                }
            }
        }

        return lst;
    }

    private Collective.Offer _offer;
    private bool _isOfferSet = false;
    private List<Collective.Currency> _lstCur = null;
    private List<Collective.Agency> _lstAg = null;
    private List<Collective.Category> _lstCats = null;
    private List<Collective.Place> _lstPlaces;
    private List<Collective.Place> _lstOfferPlaces = null;

    private Collective.Offer GetOffer()
    {
        if (!_isOfferSet)
        {
            _isOfferSet = true;

            int offerId;
            if (Int32.TryParse(Request.QueryString["offerid"], out offerId))
            {
                _offer = Collective.Offer.GetOffer(offerId, null);
            }
        }

        return _offer;
    }

    private List<Collective.Currency> GetCurrencies()
    {
        if (_lstCur == null)
        {
            _lstCur = Collective.Currency.ListCurrencies();
        }

        return _lstCur;
    }

    private List<Collective.Agency> GetAgencies()
    {
        if (_lstAg == null)
        {
            _lstAg = Collective.Agency.ListAgencies();
        }

        return _lstAg;
    }

    private List<Collective.Category> GetCategories()
    {
        if (_lstCats == null)
        {
            _lstCats = Collective.Category.ListCategoriesForAdmin(null);
        }

        return _lstCats;
    }

    private List<Collective.Place> GetAllPlaces()
    {
        if (_lstPlaces == null)
            _lstPlaces = Collective.Place.ListPlaces(null);

        return _lstPlaces;
    }

    private List<Collective.Place> GetOfferPlaces()
    {
        if (_lstOfferPlaces == null)
            _lstOfferPlaces = Collective.Place.ListOfferPlaces(GetOffer().OfferId);

        return _lstOfferPlaces;
    }
}