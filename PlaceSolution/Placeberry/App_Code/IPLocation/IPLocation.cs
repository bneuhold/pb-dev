using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;


public static class IPLocation
{
    public static string GetCountryCode(string IP)
    {
        SqlCommand sqlCommand = new SqlCommand("GetCountryFromIP");
        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

        SqlParameter country = new SqlParameter("@country", SqlDbType.NVarChar, 2);
        country.Direction = System.Data.ParameterDirection.Output;

        sqlCommand.Parameters.AddWithValue("@IP", IP);
        sqlCommand.Parameters.Add(country);

        using (sqlCommand.Connection = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            sqlCommand.Connection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Connection.Close();
        }

        return country.Value as string;
    }


}

