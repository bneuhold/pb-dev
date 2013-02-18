using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Collective
{
    // ova klasa sadrzi sve atribute iz tablice Currency pa se lako moze koristiti i za druge module.
    public class Currency
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Abbrevation { get; private set; }
        public string Regex { get; private set; }
        public string Symbol { get; private set; }

        public Currency(int id, string title, string abbrevation, string regex, string symbol)
        {
            this.Id = id;
            this.Title = title;
            this.Abbrevation = abbrevation;
            this.Regex = regex;
            this.Symbol = symbol;
        }

        public static List<Currency> ListCurrencies()
        {
            List<Currency> lst = new List<Currency>();

            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListCurrencies"
                })
                {
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Currency(
                                Convert.ToInt32(rdr["Id"]),
                                rdr["Title"].ToString(),
                                rdr["Abbrevation"].ToString(),
                                rdr["Regex"].ToString(),
                                rdr["Symbol"].ToString()
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
}