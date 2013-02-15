using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;

public partial class manage_BookingAdmin : System.Web.UI.Page
{
    protected Booking _selectedBooking = null;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        cal1.DayRender += new DayRenderEventHandler(cal_DayRender);
        cal2.DayRender += new DayRenderEventHandler(cal_DayRender);
        cal3.DayRender += new DayRenderEventHandler(cal_DayRender);

        cal1.SelectionChanged += new EventHandler(cal_SelectionChanged);
        cal2.SelectionChanged += new EventHandler(cal_SelectionChanged);
        cal3.SelectionChanged += new EventHandler(cal_SelectionChanged);

        cal2.VisibleMonthChanged += new MonthChangedEventHandler(cal2_VisibleMonthChanged);

        lbDeleteBook.Click += new EventHandler(lbDeleteBook_Click);
    }

    protected override void InitializeCulture()
    {
        string lang = "hr";

        if (Request.QueryString["lang"] != null)
            lang = Request.QueryString["lang"].ToString();

        try
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
        }
        catch (Exception)
        {
            lang = "hr";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
        }
    }

    void lbDeleteBook_Click(object sender, EventArgs e)
    {
        _selectedBooking.Delete();
        Response.Redirect(Request.Url.AbsoluteUri);
    }

    void cal2_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        cal1.SelectedDates.Clear();
        cal2.SelectedDates.Clear();
        cal3.SelectedDates.Clear();

    }

    void cal_SelectionChanged(object sender, EventArgs e)
    {
        Calendar cal = sender as Calendar;

        //if(!cal.Equals(cal1))
        //    cal1.SelectedDates.Clear();
        //if (!cal.Equals(cal2))
        //    cal2.SelectedDates.Clear();
        //if (!cal.Equals(cal3))
        //    cal3.SelectedDates.Clear();


        // ruzno sta trazi po datumu, al ne mogu nikako u cal_DayRender postaviti neki atribut kao bookingId pa da ga ovdje dohvatim
        _selectedBooking = (from b in GetBookings()
                            where b.ContainsDay(cal.SelectedDate)
                            select b).FirstOrDefault();

        hfSelectedBookingId.Value = _selectedBooking.BookingId.ToString();

        cal1.SelectedDates.Clear();
        cal2.SelectedDates.Clear();
        cal3.SelectedDates.Clear();
    }

    void cal_DayRender(object sender, DayRenderEventArgs e)
    {
        e.Day.IsSelectable = false;
        e.Cell.BackColor = System.Drawing.Color.White;
        e.Cell.ToolTip = e.Day.Date.ToShortDateString();

        List<Booking> bookings = GetBookings();
        
        if (bookings != null)
        {
            foreach (Booking book in bookings)
            {
                if (book.ContainsDay(e.Day.Date))
                {
                    //e.Cell.BackColor = System.Drawing.Color.LightGray;
                    //e.Cell.ForeColor = System.Drawing.Color.Black;
                    e.Cell.ForeColor = book.Status == 'T' ? System.Drawing.ColorTranslator.FromHtml("#CC3300") : System.Drawing.ColorTranslator.FromHtml("#208000");

                    if (_selectedBooking != null && _selectedBooking.BookingId == book.BookingId)
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGray;  // System.Drawing.ColorTranslator.FromHtml("#EDEDED");
                        e.Day.IsSelectable = false;
                    }
                    else
                    {
                        e.Day.IsSelectable = true;
                    }
                }
            }
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UltimateDC.Agency agency = GetAgency();

            lblAgencyName.Text = agency.Name;

            var accommodations = agency.Accommodations.Select(i => new { i.Id, i.Name });

            if (accommodations.Any())
            {
                repAccommodation.DataSource = accommodations;
                repAccommodation.DataBind();
                repAccommodation.Visible = true;
            }
            else
            {
                repAccommodation.Visible = false;
            }

            phSelectedAccomm.Visible = false;

            int accommId;
            if(Int32.TryParse(Request.QueryString["accommid"], out accommId))
            {
                var selAccomm = (from p in accommodations
                                 where p.Id == accommId
                                 select p).SingleOrDefault();

                if (selAccomm != null)
                {
                    phSelectedAccomm.Visible = true;
                    lblSelAccommName.Text = selAccomm.Name;
                }
            }
        }

        int selBookId;
        if (Int32.TryParse(hfSelectedBookingId.Value, out selBookId))
        {
            _selectedBooking = (from b in GetBookings()
                                where b.BookingId == selBookId
                                select b).FirstOrDefault();
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.cal2.VisibleDate = DateTime.Today;
            this.cal1.ShowNextPrevMonth = false;
            this.cal3.ShowNextPrevMonth = false;

            hlCreateBook.NavigateUrl = "BookingAdminCreate.aspx?accommid=" + Request.QueryString["accommid"];
            phCreateBook.Visible = GetBookings() != null;
        }

        if(cal2.VisibleDate.Month == 1)
            this.cal1.VisibleDate = new DateTime(cal2.VisibleDate.Year - 1, 12, 1);
        else
            this.cal1.VisibleDate = new DateTime(cal2.VisibleDate.Year, cal2.VisibleDate.Month - 1, 1);

        if(cal2.VisibleDate.Month == 12)
            this.cal3.VisibleDate = new DateTime(cal2.VisibleDate.Year + 1, 1, 1);
        else
            this.cal3.VisibleDate = new DateTime(cal2.VisibleDate.Year, cal2.VisibleDate.Month + 1, 1);

        phBookingsCalendars.Visible = phNoBookingsMsg.Visible = phSelectedBooking.Visible = false;

        if (GetBookings() != null)  // znaci ako nije samo startna stranica bez accommid u querry stringu
        {

            phNoBookingsMsg.Visible = GetBookings().Count == 0;
            phBookingsCalendars.Visible = !phNoBookingsMsg.Visible;

            if (_selectedBooking != null)
            {
                phSelectedBooking.Visible = true;

                lblSelBookDateFrom.Text = _selectedBooking.DateFrom.ToShortDateString();
                lblSelBookDateTo.Text = _selectedBooking.DateTo.ToShortDateString();
                lblSelBookStatus.Text = _selectedBooking.Status.ToString();
                lblSelBookPriceBasic.Text = _selectedBooking.PriceBasic.ToString() + " " + _selectedBooking.CurrencyAbreavation;
                lblSelBookPricaSum.Text = _selectedBooking.PriceSum.ToString() + " " + _selectedBooking.CurrencyAbreavation;
                lblSelBookNumOfPersons.Text = _selectedBooking.NumOfPersons.ToString();
                lblSelBookNumOfBabies.Text = _selectedBooking.NumOfBabies.ToString();
                lblSelBookCreateDate.Text = _selectedBooking.DateCreated.ToShortDateString();
                lblSelBookAdminCreateName.Text = _selectedBooking.AdminUserName;
                lblLastUpdateDate.Text = _selectedBooking.LastUpdateDate.HasValue ? _selectedBooking.LastUpdateDate.Value.ToShortDateString() : String.Empty;

                lblUserFirstName.Text = _selectedBooking.User.FirstName;
                lblUserLastName.Text = _selectedBooking.User.LastName;
                lblUserEmail.Text = _selectedBooking.User.Email;
                lblUserPhone.Text = _selectedBooking.User.Phone;
                lblUserCountry.Text = _selectedBooking.User.Country;
                lblUserCity.Text = _selectedBooking.User.City;
                lblUserStreet.Text = _selectedBooking.User.Street;

                hlEditBook.NavigateUrl = "BookingAdminEdit.aspx?bookid=" + _selectedBooking.BookingId;
            }
        }
    }


    private UltimateDC.Agency _agency;

    protected UltimateDC.Agency GetAgency()
    {
        if (_agency == null)
        {
            UltimateDataContext dc = new UltimateDataContext();

            int agencyId = 0;

            if (Int32.TryParse(Request.QueryString["agencyid"], out agencyId))
            {
                bool isAdmin = User.IsInRole("Administrators");

                _agency = (from p in dc.Agencies
                           where p.Id == agencyId && (isAdmin || p.PlaceberryUser.aspnet_User.UserName == User.Identity.Name)
                           select p).SingleOrDefault();
            }

            if (_agency == null)
            {
                //Ne postoji ili trenutni korisnik nema prava
                Response.Redirect("/manage/customer.aspx");
            }
        }

        return _agency;
    }


    private List<Booking> _lstBookings = null;
    private bool AreBookingsSet = false;    // ne mogu se oslanjati na null jer accommid mozda nije poslan u querry stringu za naslovnu stranicu koja ima samo popis smjestaja

    private List<Booking> GetBookings()
    {
        if (!AreBookingsSet)
        {
            AreBookingsSet = true;

            int accommId = 0;
            if (Int32.TryParse(Request.QueryString["accommid"], out accommId))
            {
                _lstBookings = Booking.GetBookingsForAccommodation(accommId, false, null);
            }
        }

        // ako vrati null znaci da je samo startna stranica sa popisom smjestaja bez accommid u querry stringu
        return _lstBookings;
    }
}