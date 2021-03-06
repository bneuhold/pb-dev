﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Collective
{
    public class OfferTranslation
    {
        public int OfferId { get; private set; }
        public int LanguageId { get; private set; }
        public string Title { get; private set; }
        public string ContentShort { get; private set; }
        public string ContentText { get; private set; }
        public string ReservationText { get; private set; }


        public OfferTranslation() { }   // dummy za grid

        private OfferTranslation(int offerId, int langId, string title, string contShort, string contText, string resText)
        {
            this.OfferId = offerId;
            this.LanguageId = langId;
            this.Title = title;
            this.ContentShort = contShort;
            this.ContentText = contText;
            this.ReservationText = resText;
        }

        public static OfferTranslation CreateOfferTranslaton(int offerId, int langId, string title, string contShort, string contText, string resText)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_CreateOfferTranslation"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CollectiveOfferId", SqlDbType.Int, 4) { Value = offerId });
                    cmd.Parameters.Add(new SqlParameter("@LanguageId", SqlDbType.Int, 4) { Value = langId });
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 250) { Value = title });
                    cmd.Parameters.Add(new SqlParameter("@ContentShort", SqlDbType.NVarChar, -1) { Value = contShort });
                    cmd.Parameters.Add(new SqlParameter("@ContentText", SqlDbType.NVarChar, -1) { Value = contText });
                    cmd.Parameters.Add(new SqlParameter("@ReservationText", SqlDbType.NVarChar, -1) { Value = resText });

                    SqlParameter parRetValue = new SqlParameter("@ReturnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (Convert.ToInt32(parRetValue.Value) == 0)
                    {
                        return new OfferTranslation(offerId, langId, title, contShort, contText, resText);
                    }

                    return null;
                }
            }
        }

        public static OfferTranslation UpdateOfferTranslaton(int offerId, int langId, string title, string contShort, string contText, string resText)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_UpdateOfferTranslation"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CollectiveOfferId", SqlDbType.Int, 4) { Value = offerId });
                    cmd.Parameters.Add(new SqlParameter("@LanguageId", SqlDbType.Int, 4) { Value = langId });
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 250) { Value = title });
                    cmd.Parameters.Add(new SqlParameter("@ContentShort", SqlDbType.NVarChar, -1) { Value = contShort });
                    cmd.Parameters.Add(new SqlParameter("@ContentText", SqlDbType.NVarChar, -1) { Value = contText });
                    cmd.Parameters.Add(new SqlParameter("@ReservationText", SqlDbType.NVarChar, -1) { Value = resText });

                    SqlParameter parRetValue = new SqlParameter("@RetValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (Convert.ToInt32(parRetValue.Value) == 0)
                    {
                        return new OfferTranslation(offerId, langId, title, contShort, contText, resText);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static void DeleteOfferTranslaton(int offerId, int langId)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_DeleteOfferTranslation"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CollectiveOfferId", SqlDbType.Int, 4) { Value = offerId });
                    cmd.Parameters.Add(new SqlParameter("@LanguageId", SqlDbType.Int, 4) { Value = langId });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static List<OfferTranslation> ListOfferTranslations(int offerId)
        {
            List<OfferTranslation> lst = new List<OfferTranslation>();

            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListOfferTranslations"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CollectiveOfferId", SqlDbType.Int, 4) { Value = offerId });

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new OfferTranslation(
                                Convert.ToInt32(rdr["CollectiveOfferId"]),
                                Convert.ToInt32(rdr["LanguageId"]),
                                rdr["Title"].ToString(),
                                rdr["ContentShort"].ToString(),
                                rdr["ContentText"].ToString(),
                                rdr["ReservationText"].ToString()
                                ));
                        }

                        rdr.Close();
                    }
                    con.Close();
                }
            }
            return lst;
        }


        public static OfferTranslation GetOfferTranslation(int offerId, int langId)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetOfferTranslation"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CollectiveOfferId", SqlDbType.Int, 4) { Value = offerId });
                    cmd.Parameters.Add(new SqlParameter("@LanguageId", SqlDbType.Int, 4) { Value = langId });

                    SqlParameter parTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parTitle);
                    SqlParameter parContentShort = new SqlParameter("@ContentShort", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parContentShort);
                    SqlParameter parContentText = new SqlParameter("@ContentText", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parContentText);
                    SqlParameter parReservationText = new SqlParameter("@ReservationText", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(parReservationText);
                    SqlParameter parRetValue = new SqlParameter("@RetValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if(Convert.ToInt32(parRetValue.Value) < 0)
                        return null;

                    return new OfferTranslation(offerId, langId, parTitle.Value.ToString(), parContentShort.Value.ToString(), parContentText.Value.ToString(), parReservationText.Value.ToString());
                }
            }

        }
    }
}