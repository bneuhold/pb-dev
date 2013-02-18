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
        private Coupon()
        {

        }

        public static CreateResultType CreateCoupon(System.Web.Security.MembershipUser user, int offerId)
        {
            return CreateCoupon(user, null, offerId);
        }

        public static CreateResultType CreateCoupon(int userId, int offerId)
        {
            return CreateCoupon(null, (int?)userId, offerId);
        }

        private static CreateResultType CreateCoupon(System.Web.Security.MembershipUser user, int? userId, int offerId)
        {
            CreateResultType result = CreateResultType.UnknownError;

            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
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

                    if (userId.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@UserIdNum", SqlDbType.Int, 4) { Value = userId.Value });

                    if (user != null)
                        cmd.Parameters.Add(new SqlParameter("@UserIdUI", SqlDbType.UniqueIdentifier, 16) { Value = user.ProviderUserKey });

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

        private static string CreateCodeNumber(int couponId)
        {
            return couponId.ToString();
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