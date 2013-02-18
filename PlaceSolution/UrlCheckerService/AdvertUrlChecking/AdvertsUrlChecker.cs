using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Xml.Linq;

namespace AdvertUrlChecking
{
    public class AdvertsUrlChecker
    {
        //"Data Source=192.168.5.89;Initial Catalog=Placeberry_dev;Persist Security Info=True;User ID=sa;Password=BabaCvita1"

        private int _webReqTimeout;
        private StringBuilder _sbEventLogEntry;

        public List<AdvertUrl> LstAdvertUrl { get; private set; }

        public AdvertsUrlChecker(List<AdvertUrl> lstAdUrl, int webReqTimeout, StringBuilder sbEventLogEntry)
        {
            LstAdvertUrl = lstAdUrl;
            _webReqTimeout = webReqTimeout;
            _sbEventLogEntry = sbEventLogEntry;
        }

        public void CheckUrls()
        {
            foreach(AdvertUrl adUrl in LstAdvertUrl)
            {
                adUrl.IsValid = CheckUrl(adUrl, _webReqTimeout, "HEAD", _sbEventLogEntry);
            }
        }


        private bool CheckUrl(AdvertUrl adUrl, int timeout, string method, StringBuilder sbLog)
        {
            bool retValue = false;

            sbLog.Append(Environment.NewLine + "AdvertId: " + adUrl.AdvertId + Environment.NewLine /*+ "Url: " + adUrl.Url + Environment.NewLine*/);    // moze bit prevelik zapis u log radi url-a kojji se lako vidi iz baze

            try
            {
                HttpWebRequest wc = WebRequest.Create(adUrl.Url) as HttpWebRequest;
                //wc.Proxy = null;
                //wc.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/0.2.153.1 Safari/525.19";
                wc.Timeout = timeout;
                wc.Method = method;

                using (HttpWebResponse wr = wc.GetResponse() as HttpWebResponse)
                {
                    sbLog.Append("StatusCode: " + wr.StatusCode + Environment.NewLine);
                    if (wr.StatusCode == HttpStatusCode.OK)
                    {
                        retValue = true;
                    }
                    wr.Close();
                }
            }
            catch (WebException wex)
            {
                if (method == "HEAD" && wex.Message.ToLower().Contains("method not allowed"))
                {
                    sbLog.Append("HEAD method not allowed, checking with GET method.");
                    retValue = CheckUrl(adUrl, timeout, "GET", sbLog);
                }
            }
            catch (Exception ex)
            {
                sbLog.Append(ex.Message + Environment.NewLine);
            }

            return retValue;
        }

        // dohvaca stranice adverata poredanih od najstarijeg
        public static List<AdvertUrl> GetAdvertsForUrlChecking(string connString, int startRow, int numOfRows)
        {
            List<AdvertUrl> lst = new List<AdvertUrl>();

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetAdvertsForUrlChecking"
                })
                {

                    cmd.Parameters.Add(new SqlParameter("@StartRow", SqlDbType.Int, 4) { Value = startRow });
                    cmd.Parameters.Add(new SqlParameter("@NumOfRows", SqlDbType.Int, 4) { Value = numOfRows });

                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new AdvertUrl(Convert.ToInt32(rdr["Id"]), rdr["Url"].ToString()));
                        }

                        rdr.Close();
                    }

                    con.Close();
                }
            }

            return lst;
        }

        public static void UpdateAdvertsUrlChecked(string connString, XDocument xdoc)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "UpdateAdvertsUrlChecked"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@AdvertsUrlChecked", SqlDbType.Xml) { Value = xdoc.ToString() });

                    con.Open();
                    cmd.ExecuteNonQuery();                        
                    con.Close();
                }
            }
        }
    }
}
