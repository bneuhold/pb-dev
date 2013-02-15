using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

public static class PutovalicaUtil
{
    private const string DEFAULT_CULTURE = "hr-HR";
    private const string DEFAULT_LANG = "hr";
    private const string LANG_PARAM = "lang";

    public static string GetConnectionString()
    {
        return ConfigurationManager.ConnectionStrings["Placeberry_CS"].ConnectionString;
    }

    public static string GetLanguage()
    {
        string lang = HttpContext.Current.Request.QueryString[LANG_PARAM];
        if (string.IsNullOrEmpty(lang))
        {
            var cookie = HttpContext.Current.Request.Cookies[LANG_PARAM];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                lang = cookie.Value;
            }
            else
            {
                string countryCode = string.Empty;
                IPAddress clientIP = GetClientIP();
                if (clientIP.AddressFamily == AddressFamily.InterNetwork) countryCode = GetCountryCode(clientIP.ToString());

                if (!string.IsNullOrEmpty(countryCode))
                {
                    lang = countryCode.ToLower();
                }
                else
                {
                    lang = DEFAULT_LANG;
                }

            }
        }
        return lang;
    }

    //TODO - vuci iz baze i cacheirati
    public static int GetLanguageId()
    {
        string lang = PutovalicaUtil.GetLanguage();
        switch (lang)
        {
            case "hr":
                return 1;
            case "en":
                return 2;
            case "de":
                return 3;
            case "it":
                return 4;
            case "cz":
                return 5;
            default:
                return 1;
        }
    }

    public static CultureInfo GetCurrentCulture()
    {
        string lang = GetLanguage();
        CultureInfo culture = null;
        switch (lang)
        {
            case "hr":
                culture = new CultureInfo("hr-HR");
                break;
            case "en":
                culture = new CultureInfo("en-US");
                break;
            case "de":
                culture = new CultureInfo("de-DE");
                break;
            case "it":
                culture = new CultureInfo("it-IT");
                break;
            case "cz":
                culture = new CultureInfo("cs-CZ");
                break;
            default:
                culture = new CultureInfo(DEFAULT_CULTURE);
                break;
        }
        return culture;
    }

    public static void SetLanguage(string lang)
    {
        var cookie = HttpContext.Current.Request.Cookies[LANG_PARAM];
        if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            cookie = new HttpCookie(LANG_PARAM, lang) { Expires = DateTime.Now.AddYears(1) };
        cookie.Value = lang;

        HttpContext.Current.Response.Cookies.Add(cookie);
    }

    private static IPAddress GetClientIP()
    {
        string userIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (!string.IsNullOrEmpty(userIP))
        {
            string[] userIPs = userIP.Split(',');
            userIP = userIPs[userIPs.Length - 1];
        }
        else
        {
            userIP = HttpContext.Current.Request.UserHostAddress;
        }

        return IPAddress.Parse(userIP);
    }

    private static string GetCountryCode(string IP)
    {
        using (SqlConnection con = new SqlConnection(GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetCountryFromIP"
            })
            {
                cmd.Parameters.Add(new SqlParameter("@IP", SqlDbType.NVarChar, 15) { Value = IP });
                SqlParameter parCountry = new SqlParameter("@country", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parCountry);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return parCountry.Value.ToString();
            }
        }
    }

    public static string QSAddArgWithValue(string url, string argName, string value)
    {
        url = url.ToLower();
        argName = argName.ToLower();

        string arg;

        if (url.Contains("?"))
        {
            arg = url.Contains("?" + argName + "=") ? ("?" + argName + "=") : ("&" + argName + "=");
        }
        else
        {
            arg = "?" + argName + "=";
        }

        if (url.Contains(arg))
        {
            string urlBefore = url.Substring(0, url.IndexOf(arg));
            string urlAfter = url.Substring(url.IndexOf(arg) + arg.Count());

            int nextArgIndex = urlAfter.IndexOf("&");
            urlAfter = nextArgIndex > -1 ? urlAfter.Substring(nextArgIndex) : string.Empty;

            return urlBefore + arg + value + urlAfter;
        }

        return url + arg + value;
    }

    public static string QSRemoveArg(string url, string argName)
    {
        url = url.ToLower();
        argName = argName.ToLower();

        string arg = argName + "=";

        if (!url.Contains(arg))
        {
            return url;
        }

        string urlBefore = url.Substring(0, url.IndexOf(arg));
        string urlAfter = url.Substring(url.IndexOf(arg) + arg.Count());
        int nextArgIndex = urlAfter.IndexOf("&");
        urlAfter = nextArgIndex > -1 ? urlAfter.Substring(nextArgIndex + 1) : string.Empty;

        return (urlBefore + urlAfter).TrimEnd('?').TrimEnd('&');
    }

    public static string MD5HashString(string str)
    {
        Byte[] originalBytes;
        Byte[] encodedBytes;
        System.Security.Cryptography.MD5CryptoServiceProvider md5;

        //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
        md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        originalBytes = ASCIIEncoding.Default.GetBytes(str);
        encodedBytes = md5.ComputeHash(originalBytes);

        //Convert encoded bytes back to a 'readable' string
        return BitConverter.ToString(encodedBytes).Replace("-", "").ToLower();

    }

    public static FacebookResponse GetFacebookResponse(string fbtoken)
    {
        WebRequest request = WebRequest.Create("https://graph.facebook.com/me?access_token=" + fbtoken);
        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream dataStream = response.GetResponseStream())
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(FacebookResponse));
                    FacebookResponse fbResp = (FacebookResponse)ser.ReadObject(dataStream);

                    return fbResp;
                }
            }
        }
        catch (System.Net.WebException)
        {
            return null;
        }
    }
}

[DataContract]
public class FacebookResponse
{
    [DataMember(Name = "id")]
    public string Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "first_name")]
    public string FirstName { get; set; }
    [DataMember(Name = "last_name")]
    public string LastName { get; set; }
    [DataMember(Name = "username")]
    public string Username { get; set; }
    [DataMember(Name = "email")]
    public string Email { get; set; }
    [DataMember(Name = "verified")]
    public bool Verified { get; set; }
}