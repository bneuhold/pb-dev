﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Collective
{
    public class Place
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public string MetaDesc { get; private set; }
        public string MetaKeywords { get; private set; }
        public string UrlTag { get; private set; }

        public Place()
        {
        }

        private Place(int id, string title, string desc, bool active, string metaDesc, string metaKW, string urlTag)
        {
            this.Id = id;
            this.Title = title;
            this.Description = desc;
            this.Active = active;
            this.MetaDesc = metaDesc;
            this.MetaKeywords = metaKW;
            this.UrlTag = urlTag;
        }

        public static Place.CreateUpdateResult CreatePlace(string title, string desc, bool active, string metaDesc, string metaKW, string urlTag)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_CreatePlace"
                })
                {
                    SqlParameter parRetId = new SqlParameter("@Id", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parRetId);
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 250) { Value = title });
                    cmd.Parameters.Add(new SqlParameter("@Desc", SqlDbType.NVarChar, 1000) { Value = desc });
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active });
                    cmd.Parameters.Add(new SqlParameter("@MetaDesc", SqlDbType.NVarChar, 1000) { Value = metaDesc });
                    cmd.Parameters.Add(new SqlParameter("@MetaKeywords", SqlDbType.NVarChar, 1000) { Value = metaKW });
                    cmd.Parameters.Add(new SqlParameter("@UrlTag", SqlDbType.NVarChar, 250) { Value = urlTag });

                    SqlParameter parRetValue = new SqlParameter("@ReturnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return (Place.CreateUpdateResult)Convert.ToInt32(parRetValue.Value);
                }
            }
        }

        public static Place.CreateUpdateResult UpdatePlace(int id, string title, string desc, bool active, string metaDesc, string metaKW, string urlTag)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_UpdatePlace"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = id });
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 250) { Value = title });
                    cmd.Parameters.Add(new SqlParameter("@Desc", SqlDbType.NVarChar, 1000) { Value = desc });
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active });
                    cmd.Parameters.Add(new SqlParameter("@MetaDesc", SqlDbType.NVarChar, 1000) { Value = metaDesc });
                    cmd.Parameters.Add(new SqlParameter("@MetaKeywords", SqlDbType.NVarChar, 1000) { Value = metaKW });
                    cmd.Parameters.Add(new SqlParameter("@UrlTag", SqlDbType.NVarChar, 250) { Value = urlTag });

                    SqlParameter parRetValue = new SqlParameter("@ReturnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return (Place.CreateUpdateResult)Convert.ToInt32(parRetValue.Value);
                }
            }
        }

        public static void DeletePlace(int id)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_DeletePlace"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = id });                    

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }


        public static List<Place> ListPlaces(bool? active)
        {
            List<Place> lst = new List<Place>();

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListPlaces"
                })
                {
                    if(active.HasValue)
                    {
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active.Value });
                    }

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Place(
                                Convert.ToInt32(rdr["Id"]),
                                rdr["Title"].ToString(),
                                rdr["Description"].ToString(),
                                Convert.ToBoolean(rdr["Active"]),
                                rdr["MetaDesc"] != DBNull.Value ? rdr["MetaDesc"].ToString() : null,
                                rdr["MetaKeywords"] != DBNull.Value ? rdr["MetaKeywords"].ToString() : null,
                                rdr["UrlTag"].ToString()
                                ));
                        }

                        rdr.Close();
                    }
                    con.Close();
                }
            }
            return lst;
        }

        public static List<Place> ListOfferPlaces(int offerId)
        {
            List<Place> lst = new List<Place>();

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListOfferPlaces"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@OfferId", SqlDbType.Int, 4) { Value = offerId });

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Place(
                                Convert.ToInt32(rdr["Id"]),
                                rdr["Title"].ToString(),
                                rdr["Description"].ToString(),
                                Convert.ToBoolean(rdr["Active"]),
                                rdr["MetaDesc"] != DBNull.Value ? rdr["MetaDesc"].ToString() : null,
                                rdr["MetaKeywords"] != DBNull.Value ? rdr["MetaKeywords"].ToString() : null,
                                rdr["UrlTag"].ToString()
                                ));
                        }

                        rdr.Close();
                    }
                    con.Close();
                }
            }
            return lst;
        }

        public enum CreateUpdateResult
        {
            Success = 1,
            TagExistsForOtherPlace = 2
        }
    }
}