using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using QueryRanking;

/// <summary>
/// Summary description for GetResults
/// </summary>

namespace Placeberry.DAL
{
    public static partial class GetResults
    {
        /// <summary>
        /// Ista storica vraća count kao return value. Template ne podržava čitanje return valuea pa se moralo eksplicitno radit...
        /// </summary>
        /// <param name="query"></param>
        /// <param name="languageId"></param>
        /// <param name="top"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static int GetDataCount( string query, int languageId, int? top, int orderBy, out string queryMessage)
        {
            CachedRanker ranker = CachedRanker.Instance;
            queryMessage = "";
            return ranker.SearchCount(query, languageId);
        }
    }

}