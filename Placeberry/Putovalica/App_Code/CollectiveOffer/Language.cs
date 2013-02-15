using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Collective
{
    // ova klasa sadrzi sve atribute iz tablice Language pa se lako moze koristiti i za druge module.
    public class Language
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Abbrevation { get; private set; }
        public string Regex { get; private set; }
        public bool Active { get; private set; }

        private Language(int id, string title, string abbrevation, string regex, bool active)
        {
            this.Id = id;
            this.Title = title;
            this.Abbrevation = abbrevation;
            this.Regex = regex;
            this.Active = active;
        }

        public static List<Language> ListLanguages(bool? active)
        {
            List<Language> lst = new List<Language>();

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListLanguages"
                })
                {
                    if (active.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active.Value });
                    }

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Language(
                                Convert.ToInt32(rdr["Id"]),
                                rdr["Title"].ToString(),
                                rdr["Abbrevation"].ToString(),
                                rdr["Regex"].ToString(),
                                Convert.ToBoolean(rdr["Active"])
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