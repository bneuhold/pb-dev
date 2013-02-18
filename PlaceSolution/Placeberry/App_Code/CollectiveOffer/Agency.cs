using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Collective
{
    // ova klasa sadrzi sve atribute iz tablice Agency pa se lako moze koristiti i za druge module.
    public class Agency
    {
        public int Id { get; private set; }
        public string Name { get; private set; }            // not null
        public string Country { get; private set; }         // not null
        public string City { get; private set; }            // not null
        public string Address { get; private set; }         // not null
        public string ContactPhone { get; private set; }
        public string ContactEmail { get; private set; }
        public int? LogoId { get; private set; }
        public string UrlWebSite { get; private set; }
        public string ManagerId { get; private set; }
        public bool Private { get; private set; }
        public int LanguageId { get; private set; }
        public int Priority { get; private set; }

        public Agency(int id, string name, string country, string city, string address, string phone, string email,
            int? logoId, string urlWebSite, string managerId, bool priv, int langId, int priority)
        {
            this.Id = id;
            this.Name = name;
            this.Country = country;
            this.City = city;
            this.Address = address;
            this.ContactPhone = phone;
            this.ContactEmail = email;
            this.LogoId = logoId;
            this.UrlWebSite = urlWebSite;
            this.ManagerId = managerId;
            this.Private = priv;
            this.LanguageId = langId;
            this.Priority = priority;
        }

        public static List<Agency> ListAgencies()
        {
            List<Agency> lst = new List<Agency>();


            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListAgencies"
                })
                {
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Agency(
                                Convert.ToInt32(rdr["Id"]),
                                rdr["Name"].ToString(),
                                rdr["Country"].ToString(),
                                rdr["City"].ToString(),
                                rdr["Address"].ToString(),
                                rdr["ContactPhone"] != DBNull.Value ? rdr["ContactPhone"].ToString() : null,
                                rdr["ContactEmail"] != DBNull.Value ? rdr["ContactEmail"].ToString() : null,
                                rdr["LogoId"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["LogoId"]) : null,
                                rdr["UrlWebsite"] != DBNull.Value ? rdr["UrlWebsite"].ToString() : null,
                                rdr["ManagerId"] != DBNull.Value ? rdr["ManagerId"].ToString() : null,
                                Convert.ToBoolean(rdr["Private"]),
                                Convert.ToInt32(rdr["LangaugeId"]),
                                Convert.ToInt32(rdr["Priority"])
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