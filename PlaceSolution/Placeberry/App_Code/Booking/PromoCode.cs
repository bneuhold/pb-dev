using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for PromoCode
/// </summary>
public class PromoCode
{
    public int Id { get; private set; }
    public int AgencyId { get; private set; }
    public string Code { get; private set; }
    public DateTime? ValidFrom { get; private set; }
    public DateTime? ValidTo { get; private set; }
    public DateTime? DateUsed { get; private set; }
    public double DiscountValue { get; private set; }
    public char DiscountType { get; private set; }

    public bool IsUsed { get { return this.DateUsed != null; } }


	private PromoCode(int id, int agencyId, string code, DateTime? validFrom, DateTime? validTo, DateTime? dateUsed, double discountValue, char discountType)
	{
        this.Id = id;
        this.AgencyId = agencyId;
        this.Code = code;
        this.ValidFrom = validFrom;
        this.ValidTo = validTo;
        this.DateUsed = dateUsed;
        this.DiscountValue = discountValue;
        this.DiscountType = discountType;
	}

    public static PromoCode GetPromoCode(int agencyId, string code)
    {
        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetPromoCodeIdForBooking"
            })
            {
                SqlParameter parPromoCodeId = new SqlParameter("@PromoCodeId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parPromoCodeId);
                SqlParameter parValidFrom = new SqlParameter("@ValidFrom", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parValidFrom);
                SqlParameter parValidTo = new SqlParameter("@ValidTo", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parValidTo);
                SqlParameter parDateUsed = new SqlParameter("@DateUsed", SqlDbType.DateTime, 8) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parDateUsed);
                SqlParameter parDiscountValue = new SqlParameter("@DiscountValue", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 15, Scale = 2 };
                cmd.Parameters.Add(parDiscountValue);
                SqlParameter parDiscountType= new SqlParameter("@DiscountType", SqlDbType.Char, 1) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parDiscountType);

                cmd.Parameters.Add(new SqlParameter("@AgencyId", SqlDbType.Int, 4) { Value = agencyId });
                cmd.Parameters.Add(new SqlParameter("@Code", SqlDbType.VarChar, 20) { Value = code });

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                if (parPromoCodeId.Value == DBNull.Value)
                {
                    return null;
                }

                return new PromoCode(
                    Convert.ToInt32(parPromoCodeId.Value),
                    agencyId,
                    code,
                    parValidFrom.Value != DBNull.Value ? (DateTime?)Convert.ToDateTime(parValidFrom.Value) : null,
                    parValidTo.Value != DBNull.Value ? (DateTime?)Convert.ToDateTime(parValidTo.Value) : null,
                    parDateUsed.Value != DBNull.Value ? (DateTime?)Convert.ToDateTime(parDateUsed.Value) : null,
                    Convert.ToDouble(parDiscountValue.Value),
                    Convert.ToChar(parDiscountType.Value));
            }
        }
    }

    public double CalculatePrice(double price)
    {
        switch (this.DiscountType)
        {
            case 'P':
                return price - (price / 100) * this.DiscountValue;
            case 'F':
                return price - this.DiscountValue;
            default:
                return price;
        }
    }
}