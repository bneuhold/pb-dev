using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;

public partial class BookingClient : System.Web.UI.Page
{
    private BookingSessionManager _booking;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.calFrom.DayRender += new DayRenderEventHandler(calFrom_DayRender);
        this.calTo.DayRender += new DayRenderEventHandler(calTo_DayRender);
        this.btnNext.Click += new EventHandler(BtnNext_Click);
        this.calFrom.SelectionChanged += new EventHandler(cal_SelectionChanged);
        this.calTo.SelectionChanged += new EventHandler(cal_SelectionChanged);
        this.ddlNumOfPersons.SelectedIndexChanged += new EventHandler(ddlNumOfPersons_SelectedIndexChanged);
        this.btnAddPromoCode.Click += new EventHandler(btnAddPromoCode_Click);
    }

    void btnAddPromoCode_Click(object sender, EventArgs e)
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
        this.lblDateMsg.Text = "Od: " + calFrom.SelectedDate.ToShortDateString() + ", do: " + calTo.SelectedDate.ToShortDateString() + " za " + ddlNumOfPersons.SelectedValue + " osoba.";
        this.lblErrorMsg.Text = String.Empty;
    }

    void ddlNumOfPersons_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.lblDateMsg.Text = "Od: " + calFrom.SelectedDate.ToShortDateString() + ", do: " + calTo.SelectedDate.ToShortDateString() + " za " + ddlNumOfPersons.SelectedValue + " osoba.";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
            else
            {
                Int32.TryParse(Request.QueryString["accommid"], out accommId);
            }

            if (accommId == 0)
            {
                Response.Redirect("/");
            }

            accommodation = (from a in dc.Accommodations
                             where a.Id == accommId
                             select a).SingleOrDefault();

            if (accommodation == null)
            {
                Response.Redirect("/");
            }

            lblAgencyName.Text = accommodation.Agency.Name == null ? string.Empty : accommodation.Agency.Name;
            lblAccommName.Text = accommodation.Name == null ? string.Empty : accommodation.Name;

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

            bool priceByPerson = false; // ovo se mora nekako postaviti
            _booking = new BookingSessionManager(accommodation.Id, accommodation.Name, accommodation.AgencyId, priceByPerson, true);

            // ukoliko admin kreira i ovdje booking takoder ga spremiti
            if (User.IsInRole("Administrators") || accommodation.Agency.PlaceberryUser.aspnet_User.UserName == User.Identity.Name)
            {
                _booking.SetAdminUserName(User.Identity.Name);
            }

            HttpContext.Current.Session[BookingSessionManager.BOOKING_SESSION_NAME] = _booking;
        }
    }


    void calTo_DayRender(object sender, DayRenderEventArgs e)
    {
        if (!GetBooking().CheckDayAvailability(e.Day.Date))
        {
            e.Day.IsSelectable = false;
            e.Cell.BackColor = System.Drawing.Color.LightGray;
        }
        else if (e.Day.IsOtherMonth)
        {
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = System.Drawing.Color.Gray;
        }
    }

    void calFrom_DayRender(object sender, DayRenderEventArgs e)
    {
        if (!GetBooking().CheckDayAvailability(e.Day.Date))
        {
            e.Day.IsSelectable = false;
            e.Cell.BackColor = System.Drawing.Color.LightGray;
        }
        else if (e.Day.IsOtherMonth)
        {
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = System.Drawing.Color.Gray;
        }
    }

    void BtnNext_Click(object sender, EventArgs e)
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
                this.lblDateMsg.Text = "Od: " + calFrom.SelectedDate.ToShortDateString() + ", do: " + calTo.SelectedDate.ToShortDateString() + " za " + ddlNumOfPersons.SelectedValue + " osoba.";
            }
        }
    }

    protected void CalFrom_SelectionChanged(object sender, EventArgs e)
    {
        
    }

    protected void CalTo_SelectionChanged(object sender, EventArgs e)
    {
        
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if(IsPostBack)
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