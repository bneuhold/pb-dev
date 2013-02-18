using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace QueryRanking
{
    class Logger
    {
        private SqlConnection conn;
        private Object sync;

        public Logger()
        {
            conn = Utility.GetDefaultConnection();
            conn.Open();
            sync = new Object();
        }

        public void LogQuery(string query, int languageId)
        {
            lock (sync)
            {
                string strcmd = String.Format(@"INSERT INTO SearchQueryLog (Query, LanguageId) VALUES ('{0}', {1})", query, languageId);
                SqlCommand sqlcmd = new SqlCommand(strcmd, conn);
                sqlcmd.ExecuteNonQuery();
            }
        }
    }
}
