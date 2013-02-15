using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;

public partial class Controls_FavoriteCookieAds : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        var cookie = Request.Cookies.Get("savedAds");
        if (cookie != null)
        {
            string vals = cookie.Value;
            if (!string.IsNullOrEmpty(vals))
            {
                int ad = 0;
                var list = from p in HttpUtility.UrlDecode(vals).Split(',')
                           select int.TryParse(p, out ad) ? ad : -1;
                var listfiltered = (from p in list
                                    where p != -1
                                    select p).Distinct().ToList();

                if (listfiltered.Any())
                {
                    using (UltimateDataContext dc = new UltimateDataContext())
                    {
                        var favoriteAds = from p in dc.Adverts
                                          where listfiltered.Contains(p.Id)
                                          select new
                                          {
                                              Id = p.Id,
                                              Title = p.Title,
                                          };                        
                        repFavoriteAds.DataSource = favoriteAds;
                        repFavoriteAds.DataBind();
                    }
                }

            }
        }

    }
}