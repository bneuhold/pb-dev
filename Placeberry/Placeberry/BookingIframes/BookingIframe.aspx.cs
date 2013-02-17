using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;
using System.Threading;
using System.Globalization;

public partial class BookingIframes_BookingIframe : System.Web.UI.Page
{
    private const string ERR_MSG_WRONG_PARAMETARS = "Error: Wrong booking parametars.";
    private const string ERR_MSG_ACCOMMODATION_NOT_FOUND = "Error: Accommodation not found in database.";

    private BookingSessionManager _booking;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.calFrom.DayRender += new DayRenderEventHandler(cal_DayRender);
        this.calTo.DayRender += new DayRenderEventHandler(cal_DayRender);
        this.lbNext.Click += new EventHandler(LbNext_Click);
        this.calFrom.SelectionChanged += new EventHandler(cal_SelectionChanged);
        this.calTo.SelectionChanged += new EventHandler(cal_SelectionChanged);
        this.ddlNumOfPersons.SelectedIndexChanged += new EventHandler(ddlNumOfPersons_SelectedIndexChanged);
        this.ddlNumOfBabies.SelectedIndexChanged += new EventHandler(ddlNumOfBabies_SelectedIndexChanged);
        this.lbAddPromoCode.Click += new EventHandler(lbAddPromoCode_Click);
    }

    protected override void InitializeCulture()
    {
        string lang = "hr";

        if (Request.QueryString["lang"] != null)
            lang = Request.QueryString["lang"].ToString();

        try
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
        }
        catch (Exception)
        {
            lang = "hr";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
        }
    }

    void lbAddPromoCode_Click(object sender, EventArgs e)
    {
        string retMsg = null;
        if (GetBooking().AddPromoCode(tbPromoCode.Text.Trim(), out retMsg))
        {
            this.lblPriceSum.Text = GetBooking().PriceSum.ToString();
            this.lblPromoCodeErrorMsg.Text = string.Empty;
        }
        else
        {
            this.lblPromoCodeErrorMsg.Text = retMsg;
        }

        this.lblPriceSum.Text = GetBooking().PriceSum.ToString();
    }

    void cal_SelectionChanged(object sender, EventArgs e)
    {
        this.lblDateMsg.Text = Resources.booking.From + ": " + (calFrom.SelectedDate >= DateTime.Now ? calFrom.SelectedDate.ToShortDateString() +
            " " + Resources.booking.To + ": " + (calTo.SelectedDate > DateTime.Now ? calTo.SelectedDate.ToShortDateString() : string.Empty) : string.Empty);
        this.lblErrorMsg.Text = String.Empty;
    }

    void ddlNumOfPersons_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.lblNumOfPersons.Text = ddlNumOfPersons.SelectedValue;
    }

    void ddlNumOfBabies_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.lblNumOfBabies.Text = ddlNumOfBabies.SelectedValue;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            phMainContent.Visible = true;
            phLoadError.Visible = false;

            UltimateDataContext dc = new UltimateDataContext();
            Accommodation accommodation;

            int advertId = 0;
            int accommId = 0;

            // booking ce se moc pokretati i preko oglasa i preko agencije (accommodationid)
            if (Int32.TryParse(Request.QueryString["advertid"], out advertId))
            {
                GetAdvertResult advert = dc.GetAdvert(advertId).Single<GetAdvertResult>();

                // nisam siguran da je ovo dobar uvjet!
                if (advert != null && advert.PlaceberryAdvert && advert.AccommodationId.HasValue)
                {
                    accommId = advert.AccommodationId.Value;
                }
            }
            else if (!Int32.TryParse(Request.QueryString["accommid"], out accommId))
            {
                // OVO JE SADA ZA NOVAKE!
                int itemId;
                if (Request.QueryString["lang"] != null)
                {
                    if (Request.QueryString["lang"].ToString() == "hr" && Int32.TryParse(Request.QueryString["itemid"], out itemId))
                    {
                        accommId = itemId - 2;
                    }
                    else if (Request.QueryString["lang"].ToString() == "en" && Int32.TryParse(Request.QueryString["itemid"], out itemId))
                    {
                        accommId = itemId + 8;
                    }
                }
            }

            if (accommId == 0)
            {
                phMainContent.Visible = false;
                phLoadError.Visible = true;
                lblLoadErrorMsg.Text = ERR_MSG_WRONG_PARAMETARS;
                return;
            }

            accommodation = (from a in dc.Accommodations
                             where a.Id == accommId
                             select a).SingleOrDefault();

            if (accommodation == null)
            {
                phMainContent.Visible = false;
                phLoadError.Visible = true;
                lblLoadErrorMsg.Text = ERR_MSG_ACCOMMODATION_NOT_FOUND;
                return;
            }

            // PROMO CODE
            bool includePromoCode = true;
            if (Boolean.TryParse(Request.QueryString["promocode"], out includePromoCode))
            {
                phPromoCode.Visible = includePromoCode;
            }

            int capMin = accommodation.CapacityMin.HasValue ? accommodation.CapacityMin.Value : 1;
            int capMax = accommodation.CapacityMax.HasValue ? accommodation.CapacityMax.Value : 1;

            for (int i = capMin; i <= capMax; ++i)
            {
                this.ddlNumOfPersons.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            for (int i = 0; i <= BookingSessionManager.DEF_MAX_NUM_OF_BABIES; ++i)
            {
                this.ddlNumOfBabies.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            this.lblPriceSum.Text = "0";
            this.lblNumOfPersons.Text = ddlNumOfPersons.SelectedValue;
            this.lblNumOfBabies.Text = ddlNumOfBabies.SelectedValue;

            bool priceByPerson = false; // HARDCODE!!! ovo ce se morat nekako postaviti!
            _booking = new BookingSessionManager(accommodation.Id, accommodation.Name, accommodation.AgencyId, priceByPerson, false);



            // ukoliko admin kreira i ovdje booking takoder ga spremiti
            if (User.IsInRole("Administrators") || accommodation.Agency.PlaceberryUser.aspnet_User.UserName == User.Identity.Name)
            {
                _booking.SetAdminUserName(User.Identity.Name);
            }

            HttpContext.Current.Session[BookingSessionManager.BOOKING_SESSION_NAME] = _booking;
            lbNext.Attributes.Add("onclick", "this.href='#';this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(lbNext, "").ToString());
        }
    }


    void cal_DayRender(object sender, DayRenderEventArgs e)
    {
        if (!GetBooking().CheckDayAvailability(e.Day.Date))
        {
            e.Day.IsSelectable = false;
            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFD9CC");
        }
        else if (e.Day.IsOtherMonth)
        {
            e.Day.IsSelectable = false;
        }
    }

    void LbNext_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            BookingSessionManager book = GetBooking();
            string errMsg = null;

            switch (book.BookingStep)
            {
                case BookingStepType.DateSelect:

                    book.CompleteDateSelectStep(calFrom.SelectedDate, calTo.SelectedDate,
                        Convert.ToInt32(ddlNumOfPersons.SelectedValue), Convert.ToInt32(ddlNumOfBabies.SelectedValue), BookingSessionManager.DEFAULT_CURRENCY_ID, true, out errMsg);
                    break;

                case BookingStepType.InfoInput:

                    book.CompleteInfoInputStep(tbFirstName.Text, tbLastName.Text, tbEmail.Text, tbPhone.Text, tbCountry.Text, tbCity.Text, tbStreet.Text, out errMsg);
                    break;

                case BookingStepType.Payment:

                    book.CompletePaymentStep(out errMsg);
                    break;

                default:
                    return;
            }

            if (!String.IsNullOrEmpty(errMsg))
            {
                this.lblErrorMsg.Text = errMsg;
            }
            else
            {
                this.lblDateMsg.Text = Resources.booking.From + ": " + (calFrom.SelectedDate >= DateTime.Now ? calFrom.SelectedDate.ToShortDateString() +
                    " " + Resources.booking.To + ": " + (calTo.SelectedDate > DateTime.Now ? calTo.SelectedDate.ToShortDateString() : string.Empty) : string.Empty);
                this.lblNumOfPersons.Text = ddlNumOfPersons.SelectedValue;
                this.lblNumOfBabies.Text = ddlNumOfBabies.SelectedValue;
            }
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (IsPostBack)
        {            
            BookingSessionManager book = GetBooking();

            switch (book.BookingStep)
            {
                case BookingStepType.DateSelect:

                    this.phStepDateSelect.Visible = true;
                    this.phStepInfoInput.Visible = false;
                    this.phStepPayment.Visible = false;
                    this.phStepComplete.Visible = false;

                    double priceSum = 0;
                    Dictionary<DateTime, double> daysWithPrices = GetBooking().GetBookingDaysWithPrices(calFrom.SelectedDate, calTo.SelectedDate, BookingSessionManager.DEFAULT_CURRENCY_ID);
                    if (daysWithPrices != null)
                    {
                        rptDaysWithPrices.DataSource = daysWithPrices;
                        rptDaysWithPrices.DataBind();

                        foreach (KeyValuePair<DateTime, double> kwp in daysWithPrices)
                        {
                            priceSum += kwp.Value;
                        }

                        //lblMsg.Text = string.Empty;
                    }
                    else
                    {
                        rptDaysWithPrices.DataSource = null;
                        rptDaysWithPrices.DataBind();
                    }

                    this.lblPriceSum.Text = GetBooking().IsPriceByPerson ? (priceSum * Convert.ToInt32(ddlNumOfPersons.SelectedValue)).ToString() : priceSum.ToString();
                    break;

                case BookingStepType.InfoInput:

                    this.phStepDateSelect.Visible = false;
                    this.phStepInfoInput.Visible = true;
                    this.phStepPayment.Visible = false;
                    this.phStepComplete.Visible = false;

                    this.lblPriceSum.Text = book.PriceSum.Value.ToString();
                    break;

                case BookingStepType.Payment:

                    this.phStepDateSelect.Visible = false;
                    this.phStepInfoInput.Visible = false;
                    this.phStepPayment.Visible = true;
                    this.phStepComplete.Visible = false;

                    this.lblPriceSum.Text = book.PriceSum.Value.ToString();
                    break;

                case BookingStepType.Complete:

                    this.phStepDateSelect.Visible = false;
                    this.phStepInfoInput.Visible = false;
                    this.phStepPayment.Visible = this.phBottom.Visible = false;
                    this.phStepComplete.Visible = true;
                    break;

                default:
                    return;
            }
        }
    }

    private BookingSessionManager GetBooking()
    {
        // booking se kreira u page_load kod geta stranice
        if (_booking == null)
        {
            _booking = HttpContext.Current.Session[BookingSessionManager.BOOKING_SESSION_NAME] as BookingSessionManager;

            // ukoliko je booking u sessionu null, znaci da je istekao session pa treba napraviti ponovni get stranice da se ponovo kreira
            if (_booking == null)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }

        return _booking;
    }
}