using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Threading;
using System.Globalization;
using UltimateDC;

public partial class BookingIframes_NovakSearch : System.Web.UI.Page
{
    private const int NOVAK_AGENCY_ID = 11;

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
        DateTime arrivDate;
        int numOfDays;
        int numOfPers;

        if (DateTime.TryParse(Request.QueryString["arrivDate"], out arrivDate)
            && Int32.TryParse(Request.QueryString["numOfDays"], out numOfDays)
            && Int32.TryParse(Request.QueryString["numOfPers"], out numOfPers))
        {
            XDocument xdoc = new XDocument(new XElement("root"));

            if (arrivDate > DateTime.Now)
            {
                List<Booking> lstBook = Booking.GetAllBookingsForAgency(NOVAK_AGENCY_ID, true, null);

                UltimateDataContext dc = new UltimateDataContext();

                var accommodations = from a in dc.Accommodations
                                        where a.AgencyId == NOVAK_AGENCY_ID
                                        select a;

                foreach (var accomm in accommodations)
                {
                    bool isAvail = numOfPers <= accomm.CapacityMax;

                    if (isAvail)
                    {
                        List<Booking> thisAccommBookings = (from b in lstBook
                                                            where b.AccommodationId == accomm.Id
                                                            select b).ToList<Booking>();

                        isAvail = CheckDateAvail(thisAccommBookings, numOfDays, arrivDate);
                    }

                    if (isAvail)
                    {
                        xdoc.Root.Add(new XElement("itemId", accomm.Id.ToString()));
                    }
                }

                //XDocument doc = new XDocument(
                //new XElement("root",
                //         new XAttribute("name", "value"),
                //         new XElement("child", "text node")));
            }

            Response.Clear();
            Response.ContentType = "text/xml; encoding='utf-8'";
            Response.Write(xdoc.ToString());
            Response.End();
        }
    }

    private bool CheckDateAvail(List<Booking> lstBook, int numOfDays, DateTime arrivDate)
    {
        for (int i = 0; i < numOfDays; ++i)
        {
            DateTime dt = arrivDate.AddDays(i);

            foreach (var b in lstBook)
            {
                if (b.ContainsDay(dt))
                {
                    return false;
                }
            }
        }

        return true;

    }
}