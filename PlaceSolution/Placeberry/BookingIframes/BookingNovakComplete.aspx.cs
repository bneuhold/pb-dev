using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;

public partial class BookingIframes_BookingNovakComplete : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        string lang = "hr";

        if (Request.QueryString["lang"] != null)
            lang = Request.QueryString["lang"].ToString();

        try
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
        }
        catch (Exception)
        {
            lang = "hr";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BookingSessionManager booking = HttpContext.Current.Session[BookingSessionManager.BOOKING_SESSION_NAME] as BookingSessionManager;

            if (booking != null)
            {
                string errMsg;
                if (!booking.SendMail(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, out errMsg) && !String.IsNullOrEmpty(errMsg))
                {
                    this.lblErrorMsg.Text = errMsg;
                }
            }
        }
    }
}