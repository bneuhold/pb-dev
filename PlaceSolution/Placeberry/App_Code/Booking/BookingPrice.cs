using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for BookingPrice
/// </summary>
public class BookingPrice
{
    public DateTime? DateStart { get; private set; }
    public DateTime? DateEnd { get; private set; }
    public double Value { get; private set; }
    public int CurrencyId { get; private set; }


	private BookingPrice(DateTime? dateStart, DateTime? dateEnd, double value, int currencyId)
	{
        this.DateStart = dateStart;
        this.DateEnd = dateEnd;
        this.Value = value;
        this.CurrencyId = currencyId;
	}


    public static List<BookingPrice> GetPricesForBooking(int accommodationId)
    {
        List<BookingPrice> lst = new List<BookingPrice>();

        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetPricesForBooking"
            })
            {
                cmd.Parameters.Add(new SqlParameter("@AccommodationId", SqlDbType.Int, 4) { Value = accommodationId });

                con.Open();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        lst.Add(new BookingPrice(
                            rdr["DateStart"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["DateStart"]) : null,
                            rdr["DateEnd"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["DateEnd"]) : null,
                            Convert.ToDouble(rdr["Value"]),
                            Convert.ToInt32(rdr["CurrencyId"])
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