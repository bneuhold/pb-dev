using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;

public partial class Advert : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            int advertID = 0;

            if (!int.TryParse(Request.QueryString["id"], out advertID))
                Response.Redirect("/");

            GetAdvertResult advert = new UltimateDataContext().GetAdvert(advertID).Single<GetAdvertResult>();

            if (!advert.PlaceberryAdvert)
            {
                advertframe.Attributes.Add("src", advert.URLLink);
            }
            else if (advert.AccommodationId.HasValue)
            {
                Response.Redirect(String.Format("/AgencyListing.aspx?id={0}", advert.AccommodationId.Value), true);
            }
            else
            {
                Response.Redirect("/", true);
            }
        }
        else
            Response.Redirect("/", true);
    }
}