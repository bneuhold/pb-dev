using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace QueryRanking
{
    public class Advert
    {
        // Real stuff.

        public int id;

        public int geoUID = -1;
        public int accUID = -1;

        public double priceFrom;
        public double priceTo;
        public int currencyId;

        public DateTime dateFrom = new DateTime(0);
        public DateTime dateTo = new DateTime(0);

        public int capacityMin;
        public int capacityMax;

        public int languageId;

        // Eye candy.

        public String country;
        public String region;
        public String island;
        public String city;
        public String accommodation;
        public String title = "";
        public String description = "";

        public AdvertScore score;

        public override String ToString()
        {
            return String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}", 
                id, geoUID, priceFrom, priceTo, currencyId, dateFrom.Equals(new DateTime(0)) ? 0 : 1, dateFrom.Month, dateFrom, dateTo, capacityMin, capacityMax);
        }
    }

    public class AdvertList
    {
        Dictionary<int, Advert> adverts;

        public AdvertList()
        {
            adverts = new Dictionary<int, Advert>();
        }

        public void InitFromDB()
        {
            try
            {
                SqlConnection conn = Utility.GetDefaultConnection();
                conn.Open();

                String strcmd = @"SELECT a.Id, a.Price, a.PriceTo, a.CurrencyId, a.DateFrom, a.DateTo, a.CapacityMin, 
                                        a.CapacityMax, a.LanguageId, i.AccommodationTermId AccTypeId, i.GeoplaceId GeoplaceId
                                    FROM Advert a LEFT JOIN SearchAdvertInfo i ON a.Id = i.AdvertId 
                                    WHERE (Country LIKE 'Hrvatska' OR Country LIKE 'Croatia')
                                        AND LanguageId IN (1, 2)";
                //                                        , AccommodationType, Title, Description, Country, Region, Island, City 

                
                SqlCommand sqlcmd = new SqlCommand(strcmd, conn);
                SqlDataReader reader = sqlcmd.ExecuteReader();

                while (reader.Read())
                {
                    Advert advert = new Advert();

                    advert.id = Int32.Parse(reader["Id"].ToString());

                    advert.geoUID = MyTryParseInt(reader["GeoplaceId"].ToString());
                    advert.accUID = MyTryParseInt(reader["AccTypeId"].ToString());

                    advert.priceFrom = MyTryParseDouble(reader["Price"].ToString());
                    advert.priceTo = MyTryParseDouble(reader["PriceTo"].ToString());
                    advert.currencyId = MyTryParseInt(reader["CurrencyId"].ToString());

                    advert.dateFrom = MyTryParseDateTime(reader["DateFrom"].ToString());
                    advert.dateTo = MyTryParseDateTime(reader["DateTo"].ToString());

                    advert.capacityMin = MyTryParseInt(reader["CapacityMin"].ToString());
                    advert.capacityMax = MyTryParseInt(reader["CapacityMax"].ToString());

                    advert.languageId = MyTryParseInt(reader["LanguageId"].ToString());

                    adverts[advert.id] = advert;

                    //if (advert.id == 97044)
                    //{
                    //    Console.WriteLine("bam");
                    //}

                    // *** FOLLOWING IS NOT NECESSARY FOR SEARCHING ***
                    // Some of it actualy updates the database and should be moved somewhere (where batch-like code is).

                    //advert.accommodation = reader["AccommodationType"].ToString();
                    //advert.title = reader["Title"].ToString();
                    //advert.description = reader["Description"].ToString();
                    //advert.country = reader["Country"].ToString();
                    //advert.region = reader["Region"].ToString();
                    //advert.island = reader["Island"].ToString();
                    //advert.city = reader["City"].ToString();
                }

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Dictionary<int, Advert> GetAllAdverts()
        {
            return adverts;
        }

        public Advert GetAdvert(int uid)
        {
            return adverts[uid];
        }

        private Int32 MyTryParseInt(String source, Int32 defaultValue = -1)
        {
            return "".Equals(source) ? defaultValue : Int32.Parse(source);
        }

        private Double MyTryParseDouble(String source, Double defaultValue = -1.0)
        {
            return "".Equals(source) ? defaultValue : Double.Parse(source);
        }

        private DateTime MyTryParseDateTime(String source)
        {
            return "".Equals(source) ? new DateTime(0) : DateTime.Parse(source);
        }

    }
}
