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
        public int LangaugeId { get; private set; }
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
            this.LangaugeId = langId;
            this.Priority = priority;
        }

        public static List<Agency> ListAgencies()
        {
            List<Agency> lst = new List<Agency>();


            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
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

        public static Collective.Agency GetAgency(int agencyId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetAgency"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = agencyId });

                    SqlParameter parName = new SqlParameter("@Name", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parCountry = new SqlParameter("@Country", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parCity = new SqlParameter("@City", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parContactPhone = new SqlParameter("@ContactPhone", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parContactEmail = new SqlParameter("@ContactEmail", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parLogoId = new SqlParameter("@LogoId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parUrlWebSite = new SqlParameter("@UrlWebSite", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parManagerId = new SqlParameter("@ManagerId", SqlDbType.UniqueIdentifier, 16) { Direction = ParameterDirection.Output };
                    SqlParameter parPrivate = new SqlParameter("@Private", SqlDbType.Bit, 1) { Direction = ParameterDirection.Output };
                    SqlParameter parLanguageId = new SqlParameter("@LangaugeId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parPriority = new SqlParameter("@Priority", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(parName);
                    cmd.Parameters.Add(parCountry);
                    cmd.Parameters.Add(parCity);
                    cmd.Parameters.Add(parAddress);
                    cmd.Parameters.Add(parContactPhone);
                    cmd.Parameters.Add(parContactEmail);
                    cmd.Parameters.Add(parLogoId);
                    cmd.Parameters.Add(parUrlWebSite);
                    cmd.Parameters.Add(parManagerId);
                    cmd.Parameters.Add(parPrivate);
                    cmd.Parameters.Add(parLanguageId);
                    cmd.Parameters.Add(parPriority);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if(parName.Value == DBNull.Value)
                        return null;

                    return new Collective.Agency(
                        agencyId,
                        parName.Value.ToString(),
                        parCountry.Value.ToString(),
                        parCity.Value.ToString(), parAddress.Value.ToString(),
                        parContactPhone.Value != DBNull.Value ? parContactPhone.Value.ToString() : null,
                        parContactEmail.Value != DBNull.Value ? parContactEmail.Value.ToString() : null,
                        parLogoId.Value != DBNull.Value ? (int?)Convert.ToInt32(parLogoId.Value) : null,
                        parUrlWebSite.Value != DBNull.Value ? parUrlWebSite.Value.ToString() : null,
                        parManagerId.Value != DBNull.Value ? parManagerId.Value.ToString() : null,
                        Convert.ToBoolean(parPrivate.Value),
                        Convert.ToInt32(parLanguageId.Value),
                        Convert.ToInt32(parPriority.Value)
                        );
                }
            }
        }
    }
}