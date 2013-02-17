using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using UltimateDC;

public partial class RedirectAd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            int advertID = 0;

            if (!int.TryParse(Request.QueryString["id"], out advertID))
                Response.Redirect(Request.UrlReferrer.AbsoluteUri);

            GetAdvertResult advert = new UltimateDataContext().GetAdvert(advertID).Single<GetAdvertResult>();

            if (!advert.PlaceberryAdvert)
            {
                Response.Redirect(advert.URLLink);
            }
            else if (advert.AccommodationId.HasValue)
            {
                Response.Redirect(String.Format("/AgencyListing.aspx?id={0}", advert.AccommodationId.Value));
            }
            else
            {
                Response.Redirect(Request.UrlReferrer.AbsoluteUri);
            }
        }
        else
            Response.Redirect(Request.UrlReferrer.AbsoluteUri);
    }
}
