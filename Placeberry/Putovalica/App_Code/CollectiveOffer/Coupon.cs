using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;


namespace Collective
{
    public class Coupon
    {
        public int Id { get; private set; }
        public int CollectiveOfferId { get; private set; }
        public int UserId { get; private set; }
        public string CodeNumber { get; private set; }
        public DateTime DateBought { get; private set; }
        public DateTime DateStart { get; private set; }
        public DateTime DateEnd { get; private set; }
        public DateTime? DateUsed { get; private set; }
        public string ShopingCartID { get; private set; }
        public bool Active { get; private set; }

        // client data

        public bool IsClientDataSet { get; private set; }

        public double OfferPriceReal { get; private set; }
        public double OfferPrice { get; private set; }
        public string OfferTitle { get; private set; }
        public string FirstImgSrc { get; private set; }

        // admin data

        public bool IsAdminDataSet { get; private set; }

        public string OfferName { get; private set; }
        public string UserEmail { get; private set; }
        public string UserFirstName { get; private set; }
        public string UserLastName { get; private set; }

        private Coupon(int id, int collOffId, int userId, string codeNumber, DateTime dateBought, DateTime dateStart, DateTime dateEnd, DateTime? dateUsed, string shopingCartID, bool active)
        {
            this.Id = id;
            this.CollectiveOfferId = collOffId;
            this.UserId = userId;
            this.CodeNumber = codeNumber;
            this.DateBought = dateBought;
            this.DateStart = dateStart;
            this.DateEnd = dateEnd;
            this.DateUsed = dateUsed;
            this.ShopingCartID = shopingCartID;
            this.Active = active;
        }

        public static CreateResultType CreateCoupon(int userId, int offerId, string shopingCartID)
        {
            CreateResultType result = CreateResultType.UnknownError;

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_CreateCoupon"
                })
                {
                    SqlParameter parCouponId = new SqlParameter("@CouponId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCouponId);
                    
                    cmd.Parameters.Add(new SqlParameter("@OfferId", SqlDbType.Int, 4) { Value = offerId });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4) { Value = userId });
                    cmd.Parameters.Add(new SqlParameter("@ShopingCartID", SqlDbType.VarChar, 50) { Value = shopingCartID });

                    SqlParameter parRetValue = new SqlParameter("@RetValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetValue);

                    con.Open();
                    cmd.ExecuteNonQuery();                    

                    result = (CreateResultType)Convert.ToInt32(parRetValue.Value);

                    if (result == CreateResultType.Success)
                    {
                        int couponId = Convert.ToInt32(parCouponId.Value);
                        string codeNum = CreateCodeNumber(couponId);

                        using (SqlCommand cmd2 = new SqlCommand()
                        {
                            Connection = con,
                            CommandType = CommandType.StoredProcedure,
                            CommandText = "Collective_AddCouponCodeNumber"
                        })
                        {
                            cmd2.Parameters.Add(new SqlParameter("@CouponId", SqlDbType.Int, 4) { Value = couponId });
                            cmd2.Parameters.Add(new SqlParameter("@CodeNumber", SqlDbType.VarChar, 12) { Value = codeNum });

                            cmd2.ExecuteNonQuery();
                        }
                    }

                    con.Close();
                }
            }

            return result;
        }

        public static List<Coupon> ListCouponsForClient(int userId, int langId)
        {
            List<Coupon> lst = new List<Coupon>();


            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListCouponsForClient"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4) { Value = userId });
                    cmd.Parameters.Add(new SqlParameter("@LangId", SqlDbType.Int, 4) { Value = langId });

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Coupon(
                                Convert.ToInt32(rdr["Id"]),
                                Convert.ToInt32(rdr["CollectiveOfferId"]),
                                Convert.ToInt32(rdr["UserId"]),
                                rdr["CodeNumber"].ToString(),
                                Convert.ToDateTime(rdr["DateBought"]),
                                Convert.ToDateTime(rdr["DateStart"]),
                                Convert.ToDateTime(rdr["DateEnd"]),
                                rdr["DateUsed"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["DateUsed"]) : null,
                                rdr["ShopingCartID"].ToString(),
                                Convert.ToBoolean(rdr["Active"]))
                                {
                                    OfferPriceReal = Convert.ToDouble(rdr["OfferPriceReal"]),
                                    OfferPrice = Convert.ToDouble(rdr["OfferPrice"]),
                                    OfferTitle = rdr["OfferTitle"].ToString(),
                                    FirstImgSrc = rdr["FirstImgSrc"] != DBNull.Value ? rdr["FirstImgSrc"].ToString() : null,
                                    IsClientDataSet = true,
                                    IsAdminDataSet = false
                                });
                        }

                        rdr.Close();
                    }
                    con.Close();
                }
            }

            return lst;
        }

        public static Coupon GetCouponForClient(int couponid, int langid)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetCouponForClient"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CouponId", SqlDbType.Int, 4) { Value = couponid });
                    cmd.Parameters.Add(new SqlParameter("@LangId", SqlDbType.Int, 4) { Value = langid });

                    SqlParameter parCollectiveOfferId = new SqlParameter("@CollectiveOfferId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCollectiveOfferId);
                    SqlParameter parUserId = new SqlParameter("@UserId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parUserId);
                    SqlParameter parCodeNumber = new SqlParameter("@CodeNumber", SqlDbType.VarChar, 12) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCodeNumber);
                    SqlParameter parDateBought = new SqlParameter("@DateBought", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parDateBought);
                    SqlParameter parDateStart = new SqlParameter("@DateStart", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parDateStart);
                    SqlParameter parDateEnd = new SqlParameter("@DateEnd", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parDateEnd);
                    SqlParameter parDateUsed = new SqlParameter("@DateUsed", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parDateUsed);
                    SqlParameter parShopingCartID = new SqlParameter("@ShopingCartID", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parShopingCartID);
                    SqlParameter parActive = new SqlParameter("@Active", SqlDbType.Bit, 1) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parActive);
                    SqlParameter parOfferPriceReal = new SqlParameter("@OfferPriceReal", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parOfferPriceReal);
                    SqlParameter parOfferPrice = new SqlParameter("@OfferPrice", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parOfferPrice);
                    SqlParameter parOfferTitle = new SqlParameter("@OfferTitle", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parOfferTitle);
                    SqlParameter parFirstImageSrc = new SqlParameter("@FirstImageSrc", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parFirstImageSrc);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (parCollectiveOfferId.Value == DBNull.Value)
                        return null;

                    return new Coupon(
                        couponid,
                        Convert.ToInt32(parCollectiveOfferId.Value),
                        Convert.ToInt32(parUserId.Value),
                        parCodeNumber.Value.ToString(),
                        Convert.ToDateTime(parDateBought.Value),
                        Convert.ToDateTime(parDateStart.Value),
                        Convert.ToDateTime(parDateEnd.Value),
                        parDateUsed.Value != DBNull.Value ? (DateTime?)Convert.ToDateTime(parDateUsed.Value) : null,
                        parShopingCartID.Value.ToString(),
                        Convert.ToBoolean(parActive.Value))
                        {
                            OfferPriceReal = Convert.ToDouble(parOfferPriceReal.Value),
                            OfferPrice = Convert.ToDouble(parOfferPrice.Value),
                            OfferTitle = parOfferTitle.Value.ToString(),
                            FirstImgSrc = parFirstImageSrc.Value != DBNull.Value ? parFirstImageSrc.Value.ToString() : null,
                            IsClientDataSet = true,
                            IsAdminDataSet = false
                        };
                }
            }
        }


        public static bool CheckShopingCartID(string shopingCartID)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_CheckShopingCartID"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@ShopingCartID", SqlDbType.VarChar, 50) { Value = shopingCartID });

                    SqlParameter parRetVal = new SqlParameter("@ReturnValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetVal);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return Convert.ToBoolean(parRetVal.Value);
                }
            }
        }

        private static string CreateCodeNumber(int couponId)
        {
            return couponId.ToString();
        }

        public static List<Coupon> PagCouponsForAdmin(string offerName, string searchUserEmail, string searchUserFirstName, string searchUserLastName, int pageNum, int numOfRows, out int totalPageCount, out int validPagNum)
        {
            List<Coupon> lst = new List<Coupon>();

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_PagCouponsForAdmin"
                })
                {
                    if (!String.IsNullOrEmpty(offerName))
                    {
                        cmd.Parameters.Add(new SqlParameter("@OfferName", SqlDbType.NVarChar, 250) { Value = offerName });
                    }

                    if (!String.IsNullOrEmpty(searchUserEmail))
                    {
                        cmd.Parameters.Add(new SqlParameter("@SearchUserEmail", SqlDbType.NVarChar, 50) { Value = searchUserEmail });
                    }

                    if (!String.IsNullOrEmpty(searchUserFirstName))
                    {
                        cmd.Parameters.Add(new SqlParameter("@SearchUserFirstName", SqlDbType.NVarChar, 50) { Value = searchUserFirstName });
                    }

                    if (!String.IsNullOrEmpty(searchUserLastName))
                    {
                        cmd.Parameters.Add(new SqlParameter("@SearchUserLastName", SqlDbType.NVarChar, 50) { Value = searchUserLastName });
                    }

                    cmd.Parameters.Add(new SqlParameter("@RowCount", SqlDbType.Int, 4) { Value = numOfRows });

                    SqlParameter parCurrPage = new SqlParameter("@PageNum", SqlDbType.Int, 4) { Value = pageNum, Direction = ParameterDirection.InputOutput };
                    cmd.Parameters.Add(parCurrPage);

                    SqlParameter parTotalPageCount = new SqlParameter("@TotalPageCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parTotalPageCount);

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Coupon(
                                Convert.ToInt32(rdr["Id"]),
                                Convert.ToInt32(rdr["CollectiveOfferId"]),
                                Convert.ToInt32(rdr["UserId"]),
                                rdr["CodeNumber"].ToString(),
                                Convert.ToDateTime(rdr["DateBought"]),
                                Convert.ToDateTime(rdr["DateStart"]),
                                Convert.ToDateTime(rdr["DateEnd"]),
                                rdr["DateUsed"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["DateUsed"]) : null,
                                rdr["ShopingCartID"].ToString(),
                                Convert.ToBoolean(rdr["Active"])
                                )
                            {
                                OfferName = rdr["OfferName"].ToString(),
                                UserEmail = rdr["UserEmail"].ToString(),
                                UserFirstName = rdr["UserFirstName"].ToString(),
                                UserLastName = rdr["UserLastName"].ToString(),

                                IsAdminDataSet = true,
                                IsClientDataSet = false
                            });
                        }

                        rdr.Close();

                        totalPageCount = Convert.ToInt32(parTotalPageCount.Value);
                        validPagNum = Convert.ToInt32(parCurrPage.Value);
                    }
                    con.Close();
                }
            }

            return lst;
        }


        public static void ToggleActive(int couponId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ToggleCouponActive"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = couponId });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static void DeleteCoupon(int couponId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_DeleteCoupon"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = couponId });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }


        public enum CreateResultType
        {
            Success = 0,
            SoldOut = 1,
            OutOfDate = 2,
            MaxUserBoughtCountReached = 3,
            UnknownError = 4
        }
    }    
}