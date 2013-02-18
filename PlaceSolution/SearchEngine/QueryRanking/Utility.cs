using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace QueryRanking
{
    public class Utility
    {
        private static string connectionString = null;

        public Utility() { }

        public static void SetConnectionString(string newConnectionString)
        {
            connectionString = newConnectionString;
        }

        public static string GetConnectionString()
        {
            return connectionString;
        }

        public static SqlConnection GetDefaultConnection()
        {
            string connStr = (connectionString != null ? connectionString : "user id=mrihr_User; password=mriusr123; server=mssql4.mojsite.com,1555; database=mrihr_PlaceberryAxilis");
            return new SqlConnection(connStr);
        }
    }
}
