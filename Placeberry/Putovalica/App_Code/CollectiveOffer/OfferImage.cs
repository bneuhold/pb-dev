using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;


namespace Collective
{
    public class OfferImage
    {
        public int ImageId { get; private set; }
        public string Src { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Alt { get; private set; }
        public int OfferId { get; private set; }
        public int Order { get; private set; }
        public bool Active { get; private set; }

        private OfferImage(int imgId, string src, string alt, string title, string desc, int offerId, int order, bool active)
        {
            this.ImageId = imgId;
            this.Src = src;
            this.Alt = alt;
            this.Title = title;
            this.Description = desc;
            this.OfferId = offerId;
            this.Order = order;
            this.Active = active;
        }

        public static OfferImage CreateImage(string title, string desc, string alt, int offerId, int order, bool active, string folderPath, string ext)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_CreateOfferImage"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 500) { Value = title });
                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 2500) { Value = desc });
                    cmd.Parameters.Add(new SqlParameter("@Alt", SqlDbType.NVarChar, 500) { Value = alt });
                    cmd.Parameters.Add(new SqlParameter("@OfferId", SqlDbType.Int, 4) { Value = offerId});
                    cmd.Parameters.Add(new SqlParameter("@Order", SqlDbType.Int, 4) { Value = order });
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active });
                    cmd.Parameters.Add(new SqlParameter("@Ext", SqlDbType.VarChar, 10) { Value = ext });

                    SqlParameter parImgId = new SqlParameter("@ImageId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parImgId);
                    SqlParameter parSrc = new SqlParameter("@Src", SqlDbType.NVarChar, 1000) { Value = folderPath, Direction = ParameterDirection.InputOutput };
                    cmd.Parameters.Add(parSrc);


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (parImgId.Value == DBNull.Value)
                        return null;

                    return new OfferImage(Convert.ToInt32(parImgId.Value), parSrc.Value.ToString(), alt, title, desc, offerId, order, active);
                }
            }
        }

        public static List<OfferImage> ListOfferImages(int offerId)
        {
            List<OfferImage> lst = new List<OfferImage>();

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListOfferImages"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@OfferId", SqlDbType.Int, 4) { Value = offerId });

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new OfferImage(
                                Convert.ToInt32(rdr["Id"]),
                                rdr["Src"].ToString(),
                                rdr["Alt"].ToString(),
                                rdr["Title"].ToString(),
                                rdr["Description"].ToString(),
                                Convert.ToInt32(rdr["CollectiveOfferId"]),
                                Convert.ToInt32(rdr["Ord"]),
                                Convert.ToBoolean(rdr["Active"]))
                                );
                        }
                        rdr.Close();
                    }
                    con.Close();
                }
            }

            return lst;
        }

        public static void DeleteImage(int imageId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_DeleteOfferImage"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@ImageId", SqlDbType.Int, 4) { Value = imageId });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static void UpdateImage(int imageId, string title, string desc, int order, bool active)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_UpdateOfferImage"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@ImageId", SqlDbType.Int, 4) { Value = imageId });
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 500) { Value = title });
                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 2500) { Value = desc });
                    cmd.Parameters.Add(new SqlParameter("@Order", SqlDbType.Int, 4) { Value = order });
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}