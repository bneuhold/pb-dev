using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for BookingRule
/// </summary>
public class BookingRule
{
    public int Id { get; private set; }
    public DateTime? RuleStartDate { get; private set; }
    public DateTime? RuleEndDate { get; private set; }
    public bool IgnoreYear { get; private set; }
    public int? MinNumOfBookingDays { get; private set; }
    public int? MaxNumOfBookingDays { get; private set; }
    public DayOfWeek? StartBookingDay { get; private set; }
    public DayOfWeek? EndBookingDay { get; private set; }


    private BookingRule(int id, DateTime? ruleStartDate, DateTime? ruleEndDate, bool ignoreYear, int? minNumOfBookDays, int? maxNumOfBookDays, string startBookingDayName, string endBookingDayName)
	{
        this.RuleStartDate = ruleStartDate;
        this.RuleEndDate = ruleEndDate;
        this.IgnoreYear = ignoreYear;
        this.MinNumOfBookingDays = minNumOfBookDays;
        this.MaxNumOfBookingDays = maxNumOfBookDays;

        try
        {
            this.StartBookingDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), startBookingDayName, true);
        }
        catch
        {
            this.StartBookingDay = null;
        }
        try
        {
            this.EndBookingDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), endBookingDayName, true);
        }
        catch
        {
            this.EndBookingDay = null;
        }
    }

    public static List<BookingRule> GetBookingRules(int accommodationId)
    {
        List<BookingRule> lst = new List<BookingRule>();

        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetBookingRules"
            })
            {
                cmd.Parameters.Add(new SqlParameter("@AccommodationId", SqlDbType.Int, 4) { Value = accommodationId });

                con.Open();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        lst.Add(new BookingRule(
                            Convert.ToInt32(rdr["Id"]),
                            rdr["RuleStartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["RuleStartDate"]) : null,
                            rdr["RuleEndDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["RuleEndDate"]) : null,
                            Convert.ToBoolean(rdr["IgnoreYear"]),
                            rdr["MinBookingDays"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["MinBookingDays"]) : null,
                            rdr["MaxBookingDays"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["MaxBookingDays"]) : null,
                            rdr["StartBookingDayName"] != DBNull.Value ? rdr["StartBookingDayName"].ToString() : null,
                            rdr["EndBookingDayName"] != DBNull.Value ? rdr["EndBookingDayName"].ToString() : null
                            ));
                    }

                    rdr.Close();
                }

                con.Close();
            }
        }

        return lst;
    }

    public bool Validate(DateTime dateFrom, DateTime dateTo)
    {
        // napraviti nove datume za svaki slucaj da se vrijeme postavi na 0:00
        dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day);
        dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day);

        // provjera dali booking zahvaca zadane rule datume

        if (this.RuleStartDate.HasValue && this.RuleEndDate.HasValue)
        {
            // radi ovog ignore year ova komplikacija!
            DateTime dtRuleStart_forFrom = new DateTime(this.IgnoreYear ? dateFrom.Year : this.RuleStartDate.Value.Year, this.RuleStartDate.Value.Month, this.RuleStartDate.Value.Day);
            DateTime dtRuleEnd_forFrom = new DateTime(this.IgnoreYear ? dateFrom.Year : this.RuleEndDate.Value.Year, this.RuleEndDate.Value.Month, this.RuleEndDate.Value.Day);

            DateTime dtRuleStart_forTo = new DateTime(this.IgnoreYear ? dateTo.Year : this.RuleStartDate.Value.Year, this.RuleStartDate.Value.Month, this.RuleStartDate.Value.Day);
            DateTime dtRuleEnd_forTo = new DateTime(this.IgnoreYear ? dateTo.Year : this.RuleEndDate.Value.Year, this.RuleEndDate.Value.Month, this.RuleEndDate.Value.Day);

            // dan do kojeg rule vrijedi nije ukljucen!
            if (!((dateFrom >= dtRuleStart_forFrom && dateFrom < dtRuleEnd_forFrom) || (dateTo >= dtRuleStart_forTo && dateTo < dtRuleEnd_forTo)))
            {
                // ukoliko su datumi postavljeni, al booking ne upada medu njih vratiti da je proso validaciju sto se ovog rule-a tice jer ne spada pod njega
                return true;
            }            
        }

        // provjera dali je broj dana mimo dozvoljenog

        TimeSpan bookDaysTs = (DateTime)dateTo - (DateTime)dateFrom;

        int numOfDays = bookDaysTs.Days + 1;    // +1 jer je i zadnji dan ukljucen!

        if ((this.MinNumOfBookingDays.HasValue && numOfDays < this.MinNumOfBookingDays.Value)
            || (this.MaxNumOfBookingDays.HasValue && numOfDays > this.MaxNumOfBookingDays.Value))
        {
            // ukoliko je ukupan broj dana rezervacije manji od dozvoljene minimalne ili veci od dozvoljene maximalne vratiti false
            return false;
        }

        // provjera dali su zadovoljeni pocetni i zavrsni dan

        if ((this.StartBookingDay.HasValue && !dateFrom.DayOfWeek.Equals(this.StartBookingDay.Value))
            || (this.EndBookingDay.HasValue && !dateTo.DayOfWeek.Equals(this.EndBookingDay.Value)))
        {
            return false;
        }

        return true;
    }
}