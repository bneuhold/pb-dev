using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;
using System.Threading;
using System.Globalization;

public partial class manage_BookingAdminCreate : System.Web.UI.Page
{
    private const string ERR_MSG_WRONG_PARAMETARS = "Error: Wrong booking parametars!";
    private const string ERR_MSG_ACCOMMODATION_NOT_FOUND = "Error: Accommodation not found in database!";
    private const string ERR_MSG_ACCESS_NOT_ALLOWED_OR_AGENCY_NOT_FOUND = "Error: Access not allowed or agency not found in database!";

    private BookingSessionManager _bookingSession;


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.calFrom.DayRender += new DayRenderEventHandler(cal_DayRender);
        this.lbNext.Click += new EventHandler(LbNext_Click);
        this.lbBack.Click += new EventHandler(lbBack_Click);
        this.calFrom.SelectionChanged += new EventHandler(cal_SelectionChanged);
        this.ddlNumOfNights.SelectedIndexChanged += new EventHandler(ddlNumOfNights_SelectedIndexChanged);
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
        if (GetBookingSession().AddPromoCode(tbPromoCode.Text.Trim(), out retMsg))
        {
            this.lblPriceSum.Text = GetBookingSession().PriceSum.ToString();
            this.lblPromoCodeErrorMsg.Text = string.Empty;
        }
        else
        {
            this.lblPromoCodeErrorMsg.Text = retMsg;
        }

        this.lblPriceSum.Text = GetBookingSession().PriceSum.ToString();
    }

    void cal_SelectionChanged(object sender, EventArgs e)
    {
        SetDateMsg();
    }

    void ddlNumOfNights_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDateMsg();
    }


    private DateTime GetDateTo()
    {
        // kod racunanja cijena i postavljanja booking stepa dateTo ce se racunati kao posljednji dan nocenja
        return calFrom.SelectedDate.AddDays(Convert.ToDouble(ddlNumOfNights.SelectedValue) - 1);
    }

    private void SetDateMsg()
    {
        if (calFrom.SelectedDate <= DateTime.Now)
        {
            this.lblDateMsg.Text = string.Empty;
            return;
        }

        // kod ispisa poruke do kad je rezervacije date to ce se prikazivati ko dan iza posljednjeg dana nocenja (dan odlaska)
        DateTime dateTo = GetDateTo().AddDays(1);

        this.lblDateMsg.Text = Resources.booking.From + ": " + calFrom.SelectedDate.ToShortDateString() +
            " " + Resources.booking.To + ": " + dateTo.ToShortDateString();
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

            int accommId = 0;
            
            if(!Int32.TryParse(Request.QueryString["accommid"], out accommId))
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

            // kako je ovo booking sa admina provjeriti prava
            bool isAdmin = User.IsInRole("Administrators");
            
            UltimateDC.Agency agency = (from p in dc.Agencies
                        where p.Id == accommodation.AgencyId && (isAdmin || p.PlaceberryUser.aspnet_User.UserName == User.Identity.Name) // svaka agencija ima samo jednog usera pa ako nije admin provjeriti dali je user od pripadajuce agencije
                       select p).SingleOrDefault();

            if (agency == null)
            {
                phMainContent.Visible = hlReturn.Visible = false;                
                phLoadError.Visible = true;
                lblLoadErrorMsg.Text = ERR_MSG_ACCESS_NOT_ALLOWED_OR_AGENCY_NOT_FOUND;
                return;
            }

            lblAgencyName.Text = accommodation.Agency.Name == null ? string.Empty : accommodation.Agency.Name;
            lblAccommName.Text = accommodation.Name == null ? string.Empty : accommodation.Name;

            for (int i = BookingSessionManager.DEF_MIN_NUM_OF_NIGHTS; i <= BookingSessionManager.DEF_MAX_NUM_OF_NIGHTS; ++i)
            {
                this.ddlNumOfNights.Items.Add(new ListItem(i.ToString(), i.ToString()));
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
            this.phPriceByDay.Visible = false;
            this.phNoPrices.Visible = false;

            // OVO JOS DODATI
            hlReturn.NavigateUrl = "bookingadmin.aspx?agencyId=" + agency.Id + "&accommid=" + accommodation.Id;

            bool priceByPerson = false; // ovo se mora nekako postaviti
            _bookingSession = new BookingSessionManager(accommodation.Id, accommodation.Name, agency.Id, priceByPerson, false);    // stavi checkmembership na false jer admin radi rezervaciju za nekoga ko moze ili ne mora bit clan placeberrya

            _bookingSession.SetAdminUserName(User.Identity.Name);  // spremiti admina koji kreira booking

            HttpContext.Current.Session[BookingSessionManager.BOOKING_SESSION_NAME] = _bookingSession;
        }
    }


    void cal_DayRender(object sender, DayRenderEventArgs e)
    {
        // napraviti lokalne datetimeove da mi vrijeme bude postavljeno na 0:00:00
        DateTime calDate = new DateTime(e.Day.Date.Year, e.Day.Date.Month, e.Day.Date.Day);
        DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        if (!GetBookingSession().CheckDayAvailability(calDate))
        {
            e.Day.IsSelectable = false;
            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFD9CC");
        }
        else if (e.Day.IsOtherMonth || calDate < currDate)
        {
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#999999");
        }
    }

    void LbNext_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            BookingSessionManager book = GetBookingSession();
            string errMsg = null;

            switch (book.BookingStep)
            {
                case BookingStepType.DateSelect:

                    DateTime dateTo = GetDateTo();

                    book.CompleteDateSelectStep(calFrom.SelectedDate, dateTo,
                        Convert.ToInt32(ddlNumOfPersons.SelectedValue), Convert.ToInt32(ddlNumOfBabies.SelectedValue), BookingSessionManager.DEFAULT_CURRENCY_ID, false, out errMsg);
                    break;

                case BookingStepType.InfoInput:

                    book.CompleteInfoInputStep(tbFirstName.Text, tbLastName.Text, tbEmail.Text, tbPhone.Text, tbCountry.Text, tbCity.Text, tbStreet.Text, out errMsg);
                    break;

                case BookingStepType.Payment:

                    book.CompletePaymentStep(out errMsg);
                    // kreiranje rezervacija iz admina ne salje mailove, treba li uopce?!
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
                SetDateMsg();
                this.lblNumOfPersons.Text = ddlNumOfPersons.SelectedValue;
                this.lblNumOfBabies.Text = ddlNumOfBabies.SelectedValue;
            }
        }
    }

    void lbBack_Click(object sender, EventArgs e)
    {
        GetBookingSession().BackStep();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            // smije dohvacati booking samo ako je postback, u suprotnom ako ga nije uspio u getu dohvatiti moze se zavrtiti u beskonacnu petlju!
            BookingSessionManager book = GetBookingSession();

            switch (book.BookingStep)
            {
                case BookingStepType.DateSelect:

                    this.phStepDateSelect.Visible = true;
                    this.phStepInfoInput.Visible = false;
                    this.phStepPayment.Visible = false;
                    this.phStepComplete.Visible = false;

                    double priceSum = 0;

                    DateTime dateTo = GetDateTo();
                    Dictionary<DateTime, double> daysWithPrices = GetBookingSession().GetBookingDaysWithPrices(calFrom.SelectedDate, dateTo, BookingSessionManager.DEFAULT_CURRENCY_ID);
                    if (daysWithPrices != null)
                    {
                        phPriceByDay.Visible = true;
                        rptDaysWithPrices.DataSource = daysWithPrices;
                        rptDaysWithPrices.DataBind();

                        foreach (KeyValuePair<DateTime, double> kwp in daysWithPrices)
                        {
                            priceSum += kwp.Value;
                        }

                        phNoPrices.Visible = false;
                    }
                    else
                    {
                        if (calFrom.SelectedDate > DateTime.Now)
                        {
                            phNoPrices.Visible = true;
                        }

                        phPriceByDay.Visible = false;
                        rptDaysWithPrices.DataSource = null;
                        rptDaysWithPrices.DataBind();
                    }

                    this.lblPriceSum.Text = GetBookingSession().IsPriceByPerson ? (priceSum * Convert.ToInt32(ddlNumOfPersons.SelectedValue)).ToString() : priceSum.ToString();
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

            lbBack.Visible = book.BookingStep != BookingStepType.DateSelect; 
        }
    }

    private BookingSessionManager GetBookingSession()
    {
        // booking se kreira u page_load kod geta stranice
        if (_bookingSession == null)
        {
            _bookingSession = HttpContext.Current.Session[BookingSessionManager.BOOKING_SESSION_NAME] as BookingSessionManager;

            // ukoliko je booking u sessionu null, znaci da je istekao session pa treba napraviti ponovni get stranice da se ponovo kreira
            if (_bookingSession == null)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }

        return _bookingSession;
    }
}