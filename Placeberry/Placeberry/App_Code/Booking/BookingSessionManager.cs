using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for Booking
/// </summary>
public class BookingSessionManager
{
    public const int DEFAULT_CURRENCY_ID = 2;
    public const int DEF_MAX_NUM_OF_BABIES = 5;
    public const int DEF_MIN_NUM_OF_NIGHTS = 1;
    public const int DEF_MAX_NUM_OF_NIGHTS = 21;

    public const string BOOKING_SESSION_NAME = "CurrBooking"; 

    private const string ERR_MSG_INFO_INPUT = "ERR: Invalid data.";
    private const string ERR_MSG_PAYMENT = "ERR: Booking error.";
    private const string ERR_MSG_USER_EXISTS_IN_DATABASE = "Err: Email in use. Please login.";
    private const string ERR_MSG_WRONG_PROMOCODE = "ERR: Invalid Promo Code.";

    private const int DEFAULT_MIN_BOOKING_DAYS = 0; // 0 - bookira samo jedan dan (TimeSpan = 0)
    private const int DEFAULT_MAX_BOOKING_DAYS = 30;

    public int AccommId { get; private set; }
    public string AccommName { get; private set; }
    public bool IsPriceByPerson { get; private set; }
    public int AgencyId { get; private set; }
    public bool CheckMembership { get; private set; }

    public BookingStepType BookingStep { get; private set; }

    // DateSelectStep
    public DateTime? DateFrom { get; private set; }
    public DateTime? DateTo { get; private set; }
    public double? PriceBasic { get; private set; }
    public double? PriceSum { get; private set; }
    public int? CurrencyId { get; private set; }
    public int? NumOfPersons { get; private set; }
    public int? NumOfBabies { get; private set; }

    // InfoInputStep
    public BookingUser User { get; private set; }
    public string AdminUserName { get; private set; }

    // PaymentStep
    public PromoCode UsedPromoCode { get; private set; }

    public int? Id { get; private set; }    // id bookinga popunjava se tek na kraju kad se booking kreira (ako ce bit5 potreban uopce)

    public bool IsEmailSent { get; private set; }


    private List<BookingRule> _lstBookingRules;     // koristiti metodu, ne pozivati preko varijable
    private List<BookingPrice> _lstBookingPrices;   // koristiti metodu, ne pozivati preko varijable
    private List<Booking> _lstExistingBookins;      // koristiti metodu, ne pozivati preko varijable


    public BookingSessionManager(int accommId, string accommName, int agencyId, bool priceByPerson, bool checkMembership)
    {
        this.AccommId = accommId;
        this.AgencyId = agencyId;
        this.AccommName = accommName;
        this.IsPriceByPerson = priceByPerson;
        this.CheckMembership = checkMembership;
        this.CurrencyId = DEFAULT_CURRENCY_ID;
        this.BookingStep = BookingStepType.DateSelect;
        this.IsEmailSent = false;
    }

    
    public bool CompleteDateSelectStep(DateTime dateFrom, DateTime dateTo, int numOfPersons, int numOfBabies, int currencyId, bool checkBookingRules, out string errMsg)
    {
        // da postavi vrijeme na 0:00:00
        this.DateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day);
        this.DateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day);
        DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        this.NumOfPersons = numOfPersons;
        this.NumOfBabies = numOfBabies;
        this.CurrencyId = currencyId;

        this.PriceSum = 0;
        this.PriceBasic = 0;

        Dictionary<DateTime, double> daysWithPrices = GetBookingDaysWithPrices(this.DateFrom.Value, this.DateTo.Value, this.CurrencyId.Value);
        if (daysWithPrices != null)
        {            
            foreach (KeyValuePair<DateTime, double> kwp in daysWithPrices)
            {
                this.PriceSum += kwp.Value;
            }
            if (this.IsPriceByPerson)
            {
                this.PriceSum *= this.NumOfPersons;
            }
            this.PriceBasic = this.PriceSum;
        }

        // neka osnovna validacija (provjera dali je pozvana SetDateSelectStep i da nisu poslani neki nebulozni podaci)
        if (this.DateFrom.Value < currDate || this.DateTo.Value < currDate || this.NumOfPersons.Value < 1)
        {
            errMsg = Resources.booking.ERR_MSG_DATE_SELECT;
            return false;
        }

        TimeSpan bookDaysTs = DateTo.Value - this.DateFrom.Value;

        if (bookDaysTs.Days < DEFAULT_MIN_BOOKING_DAYS || bookDaysTs.Days > DEFAULT_MAX_BOOKING_DAYS)
        {
            errMsg = Resources.booking.ERR_MSG_DATE_SELECT;
            return false;
        }
        
        // provjera dali je svaki dan od odabranih slobodan
        for (DateTime dt = this.DateFrom.Value; dt <= this.DateTo.Value; dt = dt.AddDays(1))
        {
            if (!CheckDayAvailability(dt))
            {
                errMsg = Resources.booking.ERR_MSG_DATE_SELECT;
                return false;
            }
        }

        // validacija booking rule-ova
        if (checkBookingRules)
        {
            foreach (BookingRule rule in GetBookingRules())
            {
                if (!rule.Validate(this.DateFrom.Value, this.DateTo.Value))
                {
                    errMsg = Resources.booking.ERR_MSG_DATE_SELECT;
                    return false;
                }
            }
        }

        this.BookingStep = BookingStepType.InfoInput;   // postaviti na iduci step
        errMsg = null;
        return true;

    }

    public bool CompleteInfoInputStep(string firstName, string lastName, string email, string phone, string country, string city, string street, out string errMsg)
    {
        this.User = new BookingUser(firstName, lastName, email, phone, country, city, street);

        if (!this.User.IsValid)
        {
            errMsg = ERR_MSG_INFO_INPUT;
            return false;
        }

        this.BookingStep = BookingStepType.Payment;   // postaviti na iduci step
        errMsg = null;
        return true;

    }

    public bool CompletePaymentStep(out string errMsg)
    {
        // prvo spremi booking u bazu
        string saveBookRetMsg = null;
        if (SaveBooking(out saveBookRetMsg))
        {
            this.BookingStep = BookingStepType.Complete;
            errMsg = null;
            return true;
        }
        else
        {
            errMsg = saveBookRetMsg != null ? saveBookRetMsg : ERR_MSG_PAYMENT;
            return false;
        }
    }

    public void BackStep()
    {
        switch (this.BookingStep)
        {
            case BookingStepType.Payment:
                this.BookingStep = BookingStepType.InfoInput;
                break;
            case BookingStepType.InfoInput:
                this.BookingStep = BookingStepType.DateSelect;
                break;
        }
    }

    // ukoliko admin radi booking u nekom trenutku postaviti ime admina da se zna da je admin radio booking
    public void SetAdminUserName(string adminUserName)
    {
        this.AdminUserName = adminUserName;
    }

    private bool SaveBooking(out string errMsg)
    {
        // provesti validaciju, dohvatiti userId i spremiti booking u bazu

        int bookingUserId = this.User.GetBookingUserId(this.CheckMembership);

        if (bookingUserId == -1)
        {
            errMsg = ERR_MSG_USER_EXISTS_IN_DATABASE;
            return false;
        }

        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "CreateBooking"
            })
            {
                cmd.Parameters.Add(new SqlParameter("@AccommodationId", SqlDbType.Int, 4) { Value = this.AccommId });
                cmd.Parameters.Add(new SqlParameter("@BookingUserId", SqlDbType.Int, 4) { Value = bookingUserId });
                cmd.Parameters.Add(new SqlParameter("@DateFrom", SqlDbType.DateTime, 8) { Value = this.DateFrom.Value });
                cmd.Parameters.Add(new SqlParameter("@DateTo", SqlDbType.DateTime, 8) { Value = this.DateTo.Value });
                cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Char, 1) { Value = 'T' }); // inicijalno se postavlja na 'T' - tentative. Dali bi ovo trebalo mijenjati ovisno o nacinu placanja?!
                cmd.Parameters.Add(new SqlParameter("@PriceBasic", SqlDbType.Decimal) { Value = this.PriceBasic.Value, Precision = 15, Scale = 2 });
                cmd.Parameters.Add(new SqlParameter("@PriceSum", SqlDbType.Decimal) { Value = this.PriceSum.Value, Precision = 15, Scale = 2 });
                cmd.Parameters.Add(new SqlParameter("@CurrencyId", SqlDbType.Int, 4) { Value = this.CurrencyId.Value });
                cmd.Parameters.Add(new SqlParameter("@NumOfPersons", SqlDbType.Int, 4) { Value = this.NumOfPersons.Value });
                cmd.Parameters.Add(new SqlParameter("@NumOfBabies", SqlDbType.Int, 4) { Value = this.NumOfBabies.Value });

                if (this.UsedPromoCode != null)
                    cmd.Parameters.Add(new SqlParameter("@PromoCodeId", SqlDbType.Int, 4) { Value = this.UsedPromoCode.Id });

                if(this.AdminUserName != null)
                    cmd.Parameters.Add(new SqlParameter("@AdminUserName", SqlDbType.NVarChar, 256) { Value = this.AdminUserName });

                SqlParameter parBookingId = new SqlParameter("@BookingId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parBookingId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                this.Id = Convert.ToInt32(parBookingId.Value);
                // vratiti ce -1 ukoliko slucajno u bazi rezervacija za ovaj smjestaj unutar zadanih datuma postoji
                if (this.Id.Value == -1)
                {
                    errMsg = Resources.booking.ErrMsgBookingNotAvailable;
                    return false;
                }
            }
        }

        errMsg = null;
        return true;
    }


    // Vraca dic sa datumom i cijenom za svaki pojedini odabrani dan.
    // ne koristi membere nego prima dateFrom i dateTo kao atribute zato da se moze pozvati i prije zavrsetka prvog stepa. Npr. samo kod promjene datuma ili broja osoba
    public Dictionary<DateTime, double> GetBookingDaysWithPrices(DateTime dateFrom, DateTime dateTo, int currencyId)
    {
        if (dateFrom <= DateTime.Now || dateTo <= DateTime.Now)
            return null;

        TimeSpan ts = dateTo - dateFrom;
        if (ts.Days < DEFAULT_MIN_BOOKING_DAYS || ts.Days > DEFAULT_MAX_BOOKING_DAYS)
            return null;


        // iz kesiranih cjenika dohvatiti samo cjenike za zadani currency
        var lstPrices = from a in GetBookingPrices()
                        where a.CurrencyId == currencyId
                        select a;

        Dictionary<DateTime, double> dicDaysWithPrices = new Dictionary<DateTime, double>();

        // za svaki dan mora proci kroz sve cjenike
        for (DateTime iDate = dateFrom; iDate <= dateTo; iDate = iDate.AddDays(1))
        {
            bool isDaySet = false;

            // ukoliko nade na cijenu u cjeniku koji se poklapa sa datumima postavit ce cijenu iz tog cjenika za taj dan
            // u suprotnome postavit ce cijenu sa prvog cjenika kojemu su datumi null
            foreach (BookingPrice bookpr in lstPrices)
            {
                if (!bookpr.DateStart.HasValue && !bookpr.DateEnd.HasValue)     // ako su oba datuma null tretirati kao neki cjelogodisnji cjenik
                {
                    // ovaj cjenik ima manji prioritet pa ne popunjava ukoliko je vec cijena za ovaj datum popunjea
                    if (!dicDaysWithPrices.ContainsKey(iDate))
                    {
                        dicDaysWithPrices.Add(iDate, bookpr.Value);
                    }
                    isDaySet = true;
                }
                else if (bookpr.DateStart.HasValue && bookpr.DateEnd.HasValue
                    && iDate >= bookpr.DateStart && iDate <= bookpr.DateEnd)  // ako oba datuma imaju vrijednost provjeriti pripada li ovaj dan ovom cjeniku
                {
                    // ovaj cjenik ima vci prioritet pa ukoliko vec ima cijenu za taj dan gazi je sa novom
                    if(dicDaysWithPrices.ContainsKey(iDate))
                    {
                        dicDaysWithPrices.Remove(iDate);
                    }
                    dicDaysWithPrices.Add(iDate, bookpr.Value);
                    isDaySet = true;
                    break;
                }
            }

            if (!isDaySet)  // znaci da nema cjenika pod koji ovaj dan spada. kak to shandleati?! trebalo bi neku poruku vratiti!
            {
                return null;
            }
        }

        return dicDaysWithPrices;
    }


    public bool CheckDayAvailability(DateTime dayDate)
    {
        // ovo se da izoptimizirati da se selectaju samo bookinzi koji imaju datefrom i date tu sa dayDate godinom i mjesecom! (dali treba to?)
        foreach (Booking book in GetExistingBookings())
        {
            if (book.ContainsDay(dayDate))
            {
                return false;
            }
        }

        return true;
    }

    public bool AddPromoCode(string code, out string errMsg)
    {
        errMsg = string.Empty;

        if (String.IsNullOrEmpty(code))
        {
            this.PriceSum = this.PriceBasic;
            return true;
        }

        if (!String.IsNullOrEmpty(code))
        {
            PromoCode promoCode = PromoCode.GetPromoCode(this.AgencyId, code);
            if (promoCode == null || promoCode.IsUsed)
            {
                this.PriceSum = this.PriceBasic;
                this.UsedPromoCode = null;
                errMsg = ERR_MSG_WRONG_PROMOCODE;
                return false;
            }

            this.UsedPromoCode = promoCode;
            this.PriceSum = promoCode.CalculatePrice(this.PriceSum.Value);

            return true;
        }

        return true;
    }

    private List<BookingRule> GetBookingRules()
    {
        if (_lstBookingRules == null)
        {
            _lstBookingRules = BookingRule.GetBookingRules(this.AccommId);
        }

        return _lstBookingRules;
    }

    private List<BookingPrice> GetBookingPrices()
    {
        if (_lstBookingPrices == null)
        {
            _lstBookingPrices = BookingPrice.GetPricesForBooking(this.AccommId);
        }

        return _lstBookingPrices;
    }

    private List<Booking> GetExistingBookings()
    {
        if (_lstExistingBookins == null)
        {
            _lstExistingBookins = Booking.GetBookingsForAccommodation(this.AccommId, true, null);
        }

        return _lstExistingBookins;
    }

    public bool SendMail(string twoLetterLang, out string exMsg)
    {      
        try
        {
            if (!this.IsEmailSent)
            {
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential("info@apartmaninovak.com", "infemlusr123");
                smtp.EnableSsl = true;

                using (System.Net.Mail.MailMessage messageClient = new System.Net.Mail.MailMessage())
                {
                    messageClient.To.Add(this.User.Email);

                    if (twoLetterLang == "hr")
                    {
                        messageClient.Subject = "Potvrda rezervacije";
                        messageClient.From = new System.Net.Mail.MailAddress("info@apartmaninovak.com");

                        string clMsg =
                            "Poštovani " + this.User.FirstName + " " + this.User.LastName +","
                            + Environment.NewLine + Environment.NewLine
                            + "zaprimili smo vašu rezervaciju smještaja u vili Novak, u razdoblju od " + this.DateFrom.Value.ToString("dd.MM.yyyy") + " do " + this.DateTo.Value.AddDays(1).ToString("dd.MM.yyyy") + "."
                            + Environment.NewLine + Environment.NewLine
                            + "Lijepo Vas molimo da obratite pažnju na slijedeće:"
                            + Environment.NewLine + Environment.NewLine
                            + "- Potrebno je uplatiti ili puni iznos ili polog u visini 200 EUR (u kunskoj protuvrijednosti) najkasnije 3 dana nakon napravljene rezervacije. Ukoliko ne primimo navedeni iznos, sustav će automatski reervaciju otkazati i morat ćete rezervirati ponovo."
                            + Environment.NewLine
                            + "- U slučaju naknadnog otkaza rezervacije, polog se ne vraća. Ukoliko je gost uplatio puni iznos rezervacije, vraća se 80% iznosa."
                            + Environment.NewLine
                            + "- Cijena ne uključuje boravišnu taksu koja se naplaćuje po dolasku, a iznosi 1 EUR po osobi, po danu (za djecu 12-18g. starosti ona iznosi 0.5 EUR)."
                            + Environment.NewLine + Environment.NewLine
                            + "U slučaju bilo kakvih pitanja ili problema slobodno nas kontaktirajte direktno na email vilanovak@apartmaninovak.com."
                            + Environment.NewLine + Environment.NewLine
                            + "Zahvaljujemo Vam na povjerenju i dobro nam došli!"
                            + Environment.NewLine + Environment.NewLine + Environment.NewLine
                            + "S poštovanjem,"
                            + Environment.NewLine
                            + "Apartmani Novak";

                        messageClient.Body = clMsg;
                    }
                    else
                    {
                        messageClient.Subject = "Reservation confirmation";
                        messageClient.From = new System.Net.Mail.MailAddress("info@apartmaninovak.com");

                        string clMsg =
                            "Dear " + this.User.FirstName + " " + this.User.LastName + ","
                            + Environment.NewLine + Environment.NewLine
                            + "we recived your apartment reservation in villa Novak, in period from " + this.DateFrom.Value.ToString("MM.dd.yyyy") + " to " + this.DateTo.Value.AddDays(1).ToString("MM.dd.yyyy") + "."
                            + Environment.NewLine + Environment.NewLine
                            + "Please bear in mind:"
                            + Environment.NewLine
                            + "- We need to receive the full amount or reservation deposit of 200 EUR no later than 3 days after you have made your booking. If we don't receive it, the system will cancel the reservation and you will have to book again."
                            + Environment.NewLine
                            + "- The deposit is non-refundable, in case of reservation cancellation. If you paid in the full amount  and wish to cancel your reservation, only 80% of the total amount is refundable."
                            + Environment.NewLine
                            + "- The price does not include sojourn tax which amounts to approximately 1 EUR per person per day (children from 12-18 years 0.5 EUR per person per day) and which is charged upon your arrival."
                            + Environment.NewLine + Environment.NewLine
                            + "In case of any problems feel free to contact us directly at vilanovak@apartmaninovak.com."
                            + Environment.NewLine + Environment.NewLine
                            + "Thank you and welcome!"
                            + Environment.NewLine + Environment.NewLine + Environment.NewLine
                            + "Best regards,"
                            + Environment.NewLine
                            + "Apartmani Novak";

                        messageClient.Body = clMsg;
                    }

                    smtp.Send(messageClient);
                }

                using (System.Net.Mail.MailMessage messageAdmin = new System.Net.Mail.MailMessage())
                {
                    messageAdmin.To.Add("vilanovak@apartmaninovak.com");
                    //messageAdmin.To.Add("domagoj.peceli@yahoo.com");
                    messageAdmin.To.Add("dpeceli@gmail.com");
                    messageAdmin.Subject = "Kreirana rezervacija";
                    messageAdmin.From = new System.Net.Mail.MailAddress("info@apartmaninovak.com");

                    string adMsg =
                        "Kreirana rezervacija za apartman " + this.AccommName + " u razdoblju od " + this.DateFrom.Value.ToString("dd.MM.yyyy") + " do " + this.DateTo.Value.AddDays(1).ToString("dd.MM.yyyy")
                        + Environment.NewLine + Environment.NewLine
                        + "Podaci o gostu:"
                        + Environment.NewLine
                        + "Ime: " + this.User.FirstName
                        + Environment.NewLine
                        + "Prezime: " + this.User.LastName
                        + Environment.NewLine
                        + "Email: " + this.User.Email
                        + Environment.NewLine
                        + "Telefon: " + (this.User.Phone != null ? this.User.Phone : string.Empty)
                        + Environment.NewLine
                        + "Država: " + (this.User.Country != null ? this.User.Country : string.Empty)
                        + Environment.NewLine
                        + "Grad: " + (this.User.City != null ? this.User.City : string.Empty)
                        + Environment.NewLine
                        + "Adresa: " + (this.User.Street != null ? this.User.Street : string.Empty)
                        + Environment.NewLine;

                    messageAdmin.Body = adMsg;

                    smtp.Send(messageAdmin);
                }

                this.IsEmailSent = true;
            }

            exMsg = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            exMsg = ex.ToString();
            return false;
        }
    }
}

public enum BookingStepType
{
    DateSelect,
    InfoInput,
    Payment,
    Complete
}
