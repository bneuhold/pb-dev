using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for Booking
/// </summary>
public class Booking
{
    private const string UPD_ERR_WRONG_DATES = "ERR: Booking inside selected days already exists!";
    private const string UPD_ERR_BOOKING_DONT_EXISTS = "ERR: Booking dont exists in database.";
    private const string UPD_ERR_CURRENCY_DONT_SXISTS = "ERR: Selected currency dont exists in database.";
    private const string UPD_SUCESS = "Rezervacija uspješno izmjenjena.";

    public int BookingId { get; private set; }
    public int AccommodationId { get; private set; }
    public BookingUser User { get; private set; }
    public DateTime DateFrom { get; private set; }
    public DateTime DateTo { get; private set; }
    public char Status { get; private set; }
    public double PriceBasic { get; private set; }
    public double PriceSum { get; private set; }
    public int CurrencyId { get; private set; }
    public string CurrencyAbreavation { get; private set; }
    public int NumOfPersons { get; private set; }
    public int NumOfBabies { get; private set; }
    public int? PromoCodeId { get; private set; }
    public string AdminUserName { get; set; }
    public DateTime DateCreated { get; private set; }
    public DateTime? LastUpdateDate { get; private set; }

	private Booking(int bookingId, int accommId, BookingUser bookingUser, DateTime dateFrom, DateTime dateTo, char status, double priceBasic, double priceSum, int currencyId, string currencyAbrevation,
        int numOfPersons, int numOfBabies, int? promoCodeId, string adminUserName, DateTime dateCreated, DateTime? lastUpdateDate)
	{
        this.BookingId = bookingId;
        this.AccommodationId = accommId;
        this.User = bookingUser;
        this.DateFrom = dateFrom;
        this.DateTo = dateTo;
        this.Status = status;
        this.PriceBasic = priceBasic;
        this.PriceSum = priceSum;
        this.CurrencyId = currencyId;
        this.CurrencyAbreavation = currencyAbrevation;
        this.NumOfPersons = numOfPersons;
        this.NumOfBabies = numOfBabies;
        this.PromoCodeId = promoCodeId;
        this.AdminUserName = adminUserName;
        this.DateCreated = dateCreated;
        this.LastUpdateDate = lastUpdateDate;
	}


    public static List<Booking> GetBookingsForAccommodation(int accommodationId, bool onlyNew, char? status)
    {
        List<Booking> lst = new List<Booking>();

        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetBookingsForAccommodation"
            })
            {
                cmd.Parameters.Add(new SqlParameter("@AccommodationId", SqlDbType.Int, 4) { Value = accommodationId });
                cmd.Parameters.Add(new SqlParameter("@OnlyNew", SqlDbType.Bit, 1) { Value = onlyNew }); // flag dali da dohvati sve bookinge ili samo vece od danasnjeg dana
                if (status.HasValue)
                {
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Char, 1) { Value = status.Value });
                }

                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        lst.Add(new Booking(
                            Convert.ToInt32(rdr["Id"]),
                            Convert.ToInt32(rdr["AccommodationId"]),
                            new BookingUser(Convert.ToInt32(rdr["BookingUserId"])),
                            Convert.ToDateTime(rdr["DateFrom"]),
                            Convert.ToDateTime(rdr["DateTo"]),
                            Convert.ToChar(rdr["Status"]),
                            Convert.ToDouble(rdr["PriceBasic"]),
                            Convert.ToDouble(rdr["PriceSum"]),
                            Convert.ToInt32(rdr["CurrencyId"]),
                            rdr["Abbrevation"].ToString(),
                            Convert.ToInt32(rdr["NumOfPersons"]),
                            Convert.ToInt32(rdr["NumOfBabies"]),
                            rdr["PromoCodeId"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["PromoCodeId"]) : null,
                            rdr["AdminUserName"] != DBNull.Value ? rdr["AdminUserName"].ToString() : null,
                            Convert.ToDateTime(rdr["DateCreated"]),
                            rdr["LastUpdateDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["LastUpdateDate"]) : null
                            ));
                    }

                    rdr.Close();
                }
                con.Close();
            }
        }

        return lst;
    }

    public static Booking GetBooking(int bookingId)
    {
        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetBooking"
            })
            {
                cmd.Parameters.Add(new SqlParameter("@BookingId", SqlDbType.Int, 4) { Value = bookingId });

                SqlParameter parBookingUserId = new SqlParameter("@BookingUserId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parBookingUserId);
                SqlParameter parAccommId = new SqlParameter("@AccommodationId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parAccommId);
                SqlParameter parDateFrom = new SqlParameter("@DateFrom", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parDateFrom);
                SqlParameter parDateTo = new SqlParameter("@DateTo", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parDateTo);
                SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.Char, 1) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parStatus);
                SqlParameter parPriceBasic = new SqlParameter("@PriceBasic", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 15, Scale = 2 };
                cmd.Parameters.Add(parPriceBasic);
                SqlParameter parPriceSum = new SqlParameter("@PriceSum", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 15, Scale = 2 };
                cmd.Parameters.Add(parPriceSum);
                SqlParameter parCurrencyId = new SqlParameter("@CurrencyId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parCurrencyId);
                SqlParameter parAbbrevation = new SqlParameter("@Abbrevation", SqlDbType.VarChar, 5) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parAbbrevation);
                SqlParameter parNumOfPersons = new SqlParameter("@NumOfPersons", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parNumOfPersons);
                SqlParameter parNumOfBabies = new SqlParameter("@NumOfBabies", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parNumOfBabies);
                SqlParameter parPromoCodeId = new SqlParameter("@PromoCodeId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parPromoCodeId);
                SqlParameter parDateCreated = new SqlParameter("@DateCreated", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parDateCreated);
                SqlParameter parLastUpdateDate = new SqlParameter("@LastUpdateDate", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parLastUpdateDate);
                SqlParameter parAdminUserName = new SqlParameter("@AdminUserName", SqlDbType.NVarChar, 256) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parAdminUserName);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                if (parAccommId.Value == DBNull.Value)   // znaci da ne postoji ovaj booking
                    return null;

                return new Booking(
                    bookingId,
                    Convert.ToInt32(parAccommId.Value),
                    new BookingUser(Convert.ToInt32(parBookingUserId.Value)),
                    Convert.ToDateTime(parDateFrom.Value),
                    Convert.ToDateTime(parDateTo.Value),
                    Convert.ToChar(parStatus.Value),
                    Convert.ToDouble(parPriceBasic.Value),
                    Convert.ToDouble(parPriceSum.Value),
                    Convert.ToInt32(parCurrencyId.Value),
                    parAbbrevation.Value.ToString(),
                    Convert.ToInt32(parNumOfPersons.Value),
                    Convert.ToInt32(parNumOfBabies.Value),
                    parPromoCodeId.Value != DBNull.Value ? (int?)Convert.ToInt32(parPromoCodeId.Value) : null,
                    parAdminUserName.Value != DBNull.Value ? parAdminUserName.Value.ToString() : null,
                    Convert.ToDateTime(parDateCreated.Value),
                    parLastUpdateDate.Value != DBNull.Value ? (DateTime?)Convert.ToDateTime(parLastUpdateDate.Value) : null
                    );
            }
        }
    }

    public static Booking UpdateBooking(int bookingId, DateTime dateFrom, DateTime dateTo, char status, double priceBasic, double priceSum, int currencyId, int numOfPersons, int numOfBabies, out string errMsg)
    {
        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "UpdateBooking"
            })
            {
                SqlParameter parBookingId = new SqlParameter("@BookingId", SqlDbType.Int, 4) { Value = bookingId };
                cmd.Parameters.Add(parBookingId);
                SqlParameter parAccommId = new SqlParameter("@AccommodationId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parAccommId);
                SqlParameter parBookingUserId = new SqlParameter("@BookingUserId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parBookingUserId);

                cmd.Parameters.Add(new SqlParameter("@DateFrom", SqlDbType.DateTime, 8) { Value = dateFrom });
                cmd.Parameters.Add(new SqlParameter("@DateTo", SqlDbType.DateTime, 8) { Value = dateTo });
                cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Char, 1) { Value = status });
                cmd.Parameters.Add(new SqlParameter("@PriceBasic", SqlDbType.Decimal) { Value = priceBasic, Precision = 15, Scale = 2 });
                cmd.Parameters.Add(new SqlParameter("@PriceSum", SqlDbType.Decimal) { Value = priceSum, Precision = 15, Scale = 2 });
                cmd.Parameters.Add(new SqlParameter("@CurrencyId", SqlDbType.Int, 4) { Value = currencyId });
             
                SqlParameter parAbbrevation = new SqlParameter("@Abbrevation", SqlDbType.VarChar, 5) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parAbbrevation);

                cmd.Parameters.Add(new SqlParameter("@NumOfPersons", SqlDbType.Int, 4) { Value = numOfPersons });
                cmd.Parameters.Add(new SqlParameter("@NumOfBabies", SqlDbType.Int, 4) { Value = numOfBabies });

                SqlParameter parPromoCodeId = new SqlParameter("@PromoCodeId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parPromoCodeId);
                SqlParameter parDateCreated = new SqlParameter("@DateCreated", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parDateCreated);
                SqlParameter parLastUpdateDate = new SqlParameter("@LastUpdateDate", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parLastUpdateDate);
                SqlParameter parAdminUserName = new SqlParameter("@AdminUserName", SqlDbType.NVarChar, 256) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parAdminUserName);

                SqlParameter parRetValue = new SqlParameter("@ReturnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
                cmd.Parameters.Add(parRetValue);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                int retVal = Convert.ToInt32(parRetValue.Value);

                switch (retVal)
                {
                    case -1:
                        errMsg = UPD_ERR_WRONG_DATES;
                        return null;
                    case -2:
                        errMsg = UPD_ERR_CURRENCY_DONT_SXISTS;
                        return null;
                    case -3:
                        errMsg = UPD_ERR_BOOKING_DONT_EXISTS;
                        return null;
                    default:
                        errMsg = UPD_SUCESS;
                        return new Booking(
                            bookingId,
                            Convert.ToInt32(parAccommId.Value),
                            new BookingUser(Convert.ToInt32(parBookingUserId.Value)),
                            dateFrom,
                            dateTo,
                            status,
                            priceBasic,
                            priceSum,
                            currencyId,
                            parAbbrevation.Value.ToString(),
                            numOfPersons,
                            numOfBabies,
                            parPromoCodeId.Value != DBNull.Value ? (int?)Convert.ToInt32(parPromoCodeId.Value) : null,
                            parAdminUserName.Value != DBNull.Value ? parAdminUserName.Value.ToString() : null,
                            Convert.ToDateTime(parDateCreated.Value),
                            (DateTime?)Convert.ToDateTime(parLastUpdateDate.Value)
                            );
                }
            }
        }
 
    }

    public bool ContainsDay(DateTime dayDate)
    {
        return dayDate >= this.DateFrom && dayDate <= this.DateTo;
    }

    public void Delete()
    {
        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "DeleteBooking"
            })
            {
                cmd.Parameters.Add(new SqlParameter("@BookingId", SqlDbType.Int, 4) { Value = this.BookingId });

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public static List<Booking> GetAllBookingsForAgency(int agencyId, bool onlyNew, char? status)
    {
        List<Booking> lst = new List<Booking>();

        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetAllBookingsForAgency"
            })
            {
                cmd.Parameters.Add(new SqlParameter("@AgencyId", SqlDbType.Int, 4) { Value = agencyId });
                cmd.Parameters.Add(new SqlParameter("@OnlyNew", SqlDbType.Bit, 1) { Value = onlyNew }); // flag dali da dohvati sve bookinge ili samo vece od danasnjeg dana
                if (status.HasValue)
                {
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Char, 1) { Value = status.Value });
                }

                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        lst.Add(new Booking(
                            Convert.ToInt32(rdr["Id"]),
                            Convert.ToInt32(rdr["AccommodationId"]),
                            new BookingUser(Convert.ToInt32(rdr["BookingUserId"])),
                            Convert.ToDateTime(rdr["DateFrom"]),
                            Convert.ToDateTime(rdr["DateTo"]),
                            Convert.ToChar(rdr["Status"]),
                            Convert.ToDouble(rdr["PriceBasic"]),
                            Convert.ToDouble(rdr["PriceSum"]),
                            Convert.ToInt32(rdr["CurrencyId"]),
                            rdr["Abbrevation"].ToString(),
                            Convert.ToInt32(rdr["NumOfPersons"]),
                            Convert.ToInt32(rdr["NumOfBabies"]),
                            rdr["PromoCodeId"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["PromoCodeId"]) : null,
                            rdr["AdminUserName"] != DBNull.Value ? rdr["AdminUserName"].ToString() : null,
                            Convert.ToDateTime(rdr["DateCreated"]),
                            rdr["LastUpdateDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["LastUpdateDate"]) : null
                            ));
                    }

                    rdr.Close();
                }
                con.Close();
            }
        }

        return lst;
    }
}