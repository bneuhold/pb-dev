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


/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    public const string DEFAULT_CULTURE = "hr-HR";
    public const string DEFAULT_LANG = "hr";
    public const string LANG_PARAM = "lang";

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
                IPAddress clientIP = Common.GetClientIP();
                if (clientIP.AddressFamily == AddressFamily.InterNetwork) countryCode = IPLocation.GetCountryCode(clientIP.ToString());

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
        string lang = Common.GetLanguage();
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
        string lang = Common.GetLanguage();
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
        var cookie = HttpContext.Current.Request.Cookies[Common.LANG_PARAM];
        if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            cookie = new HttpCookie(Common.LANG_PARAM, lang) { Expires = DateTime.Now.AddYears(1) };
        cookie.Value = lang;

        HttpContext.Current.Response.Cookies.Add(cookie);
    }
    public static IPAddress GetClientIP()
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

    public static bool IsImage(string fileType)
    {
        if (string.IsNullOrEmpty(fileType)) return false;

        bool isImage = false;

        switch (fileType)
        {
            case "image/gif":
            case "image/jpeg":
            case "image/png":
                isImage = true;
                break;
            default:
                isImage = false;
                break;
        }
        return isImage;
    }


    public static void JavascripAlert(string poruka, Page page)
    {
        ScriptManager.RegisterStartupScript(page, page.GetType(), "message", String.Format("alert('{0}');", poruka), true);
    }


    public static string shortContent(string text, int cntChar, int cntRows, int cntCharPerRow)
    {
        int realCntChar = cntChar;

        text = (text.Length > realCntChar ? text.Substring(0, realCntChar) : text.Substring(0, text.Length));

        int index = text.IndexOf("<br", 0);
        int lastindex = 0;
        while ((index > 0) && (cntRows > 0))
        {
            lastindex = index;
            //realCntChar -= cntCharPerRow;
            index = text.IndexOf("<br", index + 3);
            cntRows--;
        }
        /*if (text.Contains("Dear Ultra"))
            throw new Exception(cntRows.ToString() + "    " + index.ToString());*/
        //treba jos nes s redovima napravit / to fali
        //throw new Exception(cntRows.ToString() + "    " + index.ToString());
        //ako ima index i cntRows = 0
        if (lastindex > 0 && cntRows == 0)
            return text.Substring(0, (index > lastindex ? index : lastindex));
        else if ((text.Length >= realCntChar) && (realCntChar > 0))
            text = text.Substring(0, realCntChar).Replace("<br>", "").Replace("</br>", "").Replace("<br />", "");
        else
            return text;

        index = text.LastIndexOfAny("., ".ToCharArray());

        //throw new Exception("text:" + text + "\r\nindex:" + index.ToString());

        realCntChar = (index > 0 ? index + 1 : realCntChar);

        if (realCntChar < 0)
            realCntChar = 0;

        return ((text.Length + 3) > realCntChar ? text.Substring(0, realCntChar) + "..." : text.Substring(0, text.Length));
    }

    public static string shortContentNoTags(string text, int cntChar)
    {
        text = text.Replace("<p>", "").Replace("</p>", "").Replace("<br>", "").Replace("</br>", "").Replace("<br />", "");
        return shortContent(text, cntChar, 1, cntChar);
    }

    public static string GenericDescription(object advert, int languageId)
    {
        return string.Empty;
    }

    public static string FormatCapacity(object capacityMin, object capacityMax)
    {       
        string cmin = capacityMin != null ? capacityMin.ToString() : null;
        string cmax = capacityMax != null ? capacityMax.ToString() : null;

        bool emptymin = string.IsNullOrEmpty(cmin);
        bool emptymax = string.IsNullOrEmpty(cmax);
        
        if (emptymin && emptymax)
            return string.Empty;

        if (emptymin)
            return cmax;

        if (emptymax)
            return cmin;

        if (cmin == cmax)
            return cmax;

        //if (cmin != cmax)        
        return String.Format("{0}-{1}", cmin, cmax);
    }

    public static string FormatLocationDescription(object country, object region, object island, object city)
    {
        string description = string.Empty;

        string str = country as string;
        if (!string.IsNullOrEmpty(str)) description += str.Trim();

        str = region as string;
        if (!string.IsNullOrEmpty(str)) description += description == string.Empty ? str : ", " + str;

        str = island as string;
        if (!string.IsNullOrEmpty(str)) description += description == string.Empty ? str : ", " + str;

        str = city as string;
        if (!string.IsNullOrEmpty(str)) description += description == string.Empty ? str : ", " + str;


        return description;
    }

    public static string FormatAdvertUrl(object advertId, object placeberryAdvert, object agencyUrlTag, object accommUrlTag)
    {
        bool placeadvert = (bool)placeberryAdvert;
        if (placeadvert)
        {
            string agencyurltag = agencyUrlTag as string;
            string accommurltag = accommUrlTag as string;

            if (!string.IsNullOrEmpty(agencyurltag) && !string.IsNullOrEmpty(accommurltag))
                return GenerateAccommodationUrl(agencyurltag, accommurltag);
        }

        return String.Format("/RedirectAd.aspx?id={0}", advertId);
    }

    public static string FormatSourceUrl(object placeberryAdvert, object agencyUrlTag, object sourceUrl)
    {
        bool placeadvert = (bool)placeberryAdvert;

        if (placeadvert)
        {
            string agencyurltag = agencyUrlTag as string;

            if (!string.IsNullOrEmpty(agencyurltag))
                return GenerateAgencyUrl(agencyurltag);
        }

        return String.Format("http://{0}", sourceUrl as string ?? string.Empty);
    }

    public static string GenerateAgencyUrl(string agencyUrlTag){
        return String.Format("/{0}/{1}", Resources.placeberry.Agency_UrlBase, agencyUrlTag);
    }

    public static string GenerateAccommodationUrl(string agencyUrlTag, string accommodationUrlTag){
        return String.Format("/{0}/{1}/{2}", Resources.placeberry.Agency_UrlBase, agencyUrlTag, accommodationUrlTag);
    }

    public static string FixQueryStringUrlTag(string urlTag)
    {
        urlTag = urlTag.Trim();
        urlTag = urlTag.ToLower();
        urlTag = urlTag.Replace(' ', '+');

        return urlTag;
    }

}