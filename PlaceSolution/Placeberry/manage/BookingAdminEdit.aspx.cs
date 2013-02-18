using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;
using System.Threading;
using System.Globalization;

public partial class manage_BookingAdminEdit : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        calFrom.DayRender += new DayRenderEventHandler(cal_DayRender);
        lbSave.Click += new EventHandler(lbSave_Click);
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

    void lbSave_Click(object sender, EventArgs e)
    {
        // neka osnovna provjera da nisu bas nebulozno odabrani datumi
        DateTime dateTo = GetDateTo();
        TimeSpan bookDaysTs = dateTo - calFrom.SelectedDate;
        if (bookDaysTs.Days < 0)
        {
            lblRetMsg.ForeColor = System.Drawing.Color.Red;
            lblRetMsg.Text = Resources.booking.ERR_MSG_DATE_SELECT;
            return;
        }

        int currBookingId = Convert.ToInt32(hfBookingId.Value);

        // provjera dali je svaki dan od odabranih slobodan
        for (DateTime dt = calFrom.SelectedDate; dt <= dateTo; dt = dt.AddDays(1))
        {
            foreach (Booking book in GetAccommBookings())
            {
                if (book.BookingId != currBookingId && book.ContainsDay(dt))
                {
                    lblRetMsg.ForeColor = System.Drawing.Color.Red;
                    lblRetMsg.Text = Resources.booking.ERR_MSG_DATE_SELECT;
                    return;
                }
            }
        }


        string retMsg = null;
        Booking newBook = Booking.UpdateBooking(currBookingId, calFrom.SelectedDate, dateTo, Convert.ToChar(ddlStatus.SelectedValue),
            Convert.ToDouble(tbPriceBasic.Text), Convert.ToDouble(tbPriceSum.Text),
            Convert.ToInt32(ddlCurrency.SelectedValue), Convert.ToInt32(ddlNumOfPersons.SelectedValue), Convert.ToInt32(ddlNumOfBabies.SelectedValue), out retMsg);

        lblRetMsg.ForeColor = newBook == null ? System.Drawing.Color.Red : System.Drawing.Color.Green;
        lblRetMsg.Text = retMsg;

        // resetirati listu bookinga jer se poziva dayRender nakon ovoga pa ce se dani trenutno izmjenjeni booking dohvacati iz stare liste
        _lstBookings = null;
        IsLstBookingsSet = false;
    }


    void cal_DayRender(object sender, DayRenderEventArgs e)
    {
        e.Cell.ToolTip = e.Day.Date.ToShortDateString();

        if (e.Day.IsSelected)
            return;

        int currBookingId;
        if (GetAccommBookings() != null && Int32.TryParse(hfBookingId.Value, out currBookingId))
        {
            foreach (Booking book in GetAccommBookings())
            {
                if (book.ContainsDay(e.Day.Date))
                {
                    if (currBookingId == book.BookingId)
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGray;
                        return;
                    }

                    e.Day.IsSelectable = false;
                    e.Cell.ForeColor = book.Status == 'T' ? System.Drawing.ColorTranslator.FromHtml("#CC3300") : System.Drawing.ColorTranslator.FromHtml("#208000");
                }
            }
        }

        if (e.Day.IsOtherMonth)
        {
            e.Day.IsSelectable = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int bookingId;
            if (!Int32.TryParse(Request.QueryString["bookid"], out bookingId))
            {
                this.phMainContent.Visible = false;
                this.phError.Visible = true;
                this.ltErrMsg.Text = "Error: Missing bookid parametar!";
                return;
                //Response.Redirect("/manage/customer.aspx");
            }

            Booking booking = Booking.GetBooking(bookingId);

            if (booking == null)
            {
                this.phMainContent.Visible = false;
                this.phError.Visible = true;
                this.ltErrMsg.Text = "Error: Booking not found in database!";
                return;
                //Response.Redirect("/manage/customer.aspx");
            }

            UltimateDataContext dc = new UltimateDataContext();

            UltimateDC.Accommodation accommodation = (from a in dc.Accommodations
                                                      where a.Id == booking.AccommodationId
                                                      select a).SingleOrDefault();

            if (accommodation == null)
            {
                this.phMainContent.Visible = false;
                this.phError.Visible = true;
                this.ltErrMsg.Text = "Error: Accommodation not found in database!";
                return;
                //Response.Redirect("/manage/customer.aspx");
            }

            // kako je ovo booking sa admina provjeriti prava
            bool isAdmin = User.IsInRole("Administrators");

            UltimateDC.Agency agency = (from p in dc.Agencies
                                        where p.Id == accommodation.AgencyId && (isAdmin || p.PlaceberryUser.aspnet_User.UserName == User.Identity.Name) // svaka agencija ima samo jednog usera pa ako nije admin provjeriti dali je user od pripadajuce agencije
                                        select p).SingleOrDefault();

            if (agency == null)
            {
                this.phMainContent.Visible = false;
                this.phError.Visible = true;
                this.ltErrMsg.Text = "Error: Access not allowed!";
                return;
                //Response.Redirect("/manage/customer.aspx");
            }


            this.hfAccommodationId.Value = accommodation.Id.ToString();
            this.hfBookingId.Value = bookingId.ToString();
            
            ddlStatus.Items.Add(new ListItem("T", "T"));
            ddlStatus.Items.Add(new ListItem("O", "O"));
            ddlStatus.SelectedValue = booking.Status.ToString();

            tbPriceBasic.Text = booking.PriceBasic.ToString();
            tbPriceSum.Text = booking.PriceSum.ToString();

            var currencies = from c in dc.Currencies
                             select c;

            foreach (var c in currencies)
            {
                this.ddlCurrency.Items.Add(new ListItem(c.Abbrevation, c.Id.ToString()));
            }

            this.ddlCurrency.SelectedValue = booking.CurrencyId.ToString();

            calFrom.VisibleDate = booking.DateFrom;
            calFrom.SelectedDate = booking.DateFrom;

            for (int i = BookingSessionManager.DEF_MIN_NUM_OF_NIGHTS; i <= BookingSessionManager.DEF_MAX_NUM_OF_NIGHTS; ++i)
            {
                this.ddlNumOfNights.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            this.ddlNumOfNights.SelectedValue = ((booking.DateTo - booking.DateFrom).Days + 1).ToString();

            int capMin = accommodation.CapacityMin.HasValue ? accommodation.CapacityMin.Value : 1;
            int capMax = accommodation.CapacityMax.HasValue ? accommodation.CapacityMax.Value : 1;

            for (int i = capMin; i <= capMax; ++i)
            {
                this.ddlNumOfPersons.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            ddlNumOfPersons.SelectedValue = booking.NumOfPersons.ToString();

            for (int i = 0; i <= BookingSessionManager.DEF_MAX_NUM_OF_BABIES; ++i)
            {
                this.ddlNumOfBabies.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlNumOfBabies.SelectedValue = booking.NumOfBabies.ToString();

            lblCreateDate.Text = booking.DateCreated.ToShortDateString();
            lblAdminCreateName.Text = booking.AdminUserName != null ? booking.AdminUserName : String.Empty;
            lblLastUpdateDate.Text = booking.LastUpdateDate.HasValue ? booking.LastUpdateDate.Value.ToShortDateString() : String.Empty;

            lblUserFirstName.Text = booking.User.FirstName;
            lblUserLastName.Text = booking.User.LastName;
            lblUserEmail.Text = booking.User.Email;
            lblUserPhone.Text = booking.User.Phone;
            lblUserCountry.Text = booking.User.Country;
            lblUserCity.Text = booking.User.City;
            lblUserStreet.Text = booking.User.Street;

            hlBack.NavigateUrl = "/manage/bookingadmin.aspx?agencyId=" + accommodation.AgencyId.ToString() + "&accommid=" + accommodation.Id;
        }

        lblRetMsg.Text = " ";
    }

    private List<Booking> _lstBookings = null;
    private bool IsLstBookingsSet = false;

    private List<Booking> GetAccommBookings()
    {
        if (!IsLstBookingsSet)
        {
            IsLstBookingsSet = true;

            _lstBookings = Booking.GetBookingsForAccommodation(Convert.ToInt32(hfAccommodationId.Value), false, null);
        }
        return _lstBookings;
    }

    private DateTime GetDateTo()
    {
        // kod racunanja cijena i postavljanja booking stepa dateTo ce se racunati kao posljednji dan nocenja
        return calFrom.SelectedDate.AddDays(Convert.ToDouble(ddlNumOfNights.SelectedValue) - 1);
    }
}