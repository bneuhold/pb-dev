using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UltimateDC;
using System.Text.RegularExpressions;
using System.Globalization;

public partial class InfoBox : System.Web.UI.Page
{
    protected string Latitude = string.Empty;
    protected string Longitude = string.Empty;
    protected string Address = string.Empty;
    protected string City = string.Empty;
    protected string Country = string.Empty;
    protected string AdvertName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        int advertId = 0;
        if (int.TryParse(Request.QueryString["infoid"], out advertId))
        {
            using (UltimateDataContext dc = new UltimateDataContext())
            {
                var advert = (from p in dc.Adverts
                              where p.Id == advertId
                              select p).SingleOrDefault();

                if (advert != null)
                {
                    Latitude = advert.Latitude.HasValue ? advert.Latitude.Value.ToString("###.######", CultureInfo.InvariantCulture) : string.Empty;
                    Longitude = advert.Longitude.HasValue ? advert.Longitude.Value.ToString("###.######", CultureInfo.InvariantCulture) : string.Empty;
                    Address = advert.Address ?? string.Empty;
                    Country = advert.Country ?? string.Empty;
                    City = advert.City ?? string.Empty;
                    AdvertName = advert.Title ?? string.Empty;

                    

                    var ultimateTable = advert.AdvertInfo;

                    if (ultimateTable != null)
                    {
                        GoogleForecast1.ForecastId = ultimateTable.Id;

                        if (ultimateTable.UltimateTableInfos.Any())
                        {
                            var query = ultimateTable.UltimateTableInfos.Where(i => i.LanguageId == Common.GetLanguageId());
                            query = query.Any() ? query : ultimateTable.UltimateTableInfos;

                            var wiki = query.Take(1).SingleOrDefault();

                            if (wiki != null)
                            {
                                if (!string.IsNullOrEmpty(wiki.WikiLink))
                                {
                                    aWikiLink.HRef = wiki.WikiLink;
                                    aWikiLink.InnerText = (String.IsNullOrEmpty(City) ? "» View more" : "» View more about " + City);
                                }
                                if (!string.IsNullOrEmpty(wiki.WikiDescription))
                                {
                                    ltlWikiDesc.Text = Common.shortContentNoTags(wiki.WikiDescription, 404);
                                }

                                if (string.IsNullOrEmpty(wiki.WikiLink) && string.IsNullOrEmpty(wiki.WikiDescription))
                                {
                                    //liWiki.Visible = false;
                                }


                                if (!string.IsNullOrEmpty(wiki.Info))
                                {
                                    ltlInfo.Text = wiki.Info;
                                }
                                else
                                {
                                    //liInfo.Visible = false;
                                }
                            }
                            else
                            {
                                //liInfo.Visible = false;
                                //liWiki.Visible = false;
                            }
                        }

                        var carrierLines = (from p in dc.CarrierLineTimeTables
                                            where p.PlaceToId == ultimateTable.Id
                                            select p.CarrierLineSeason.CarrierLine).Distinct();
                        if (carrierLines.Any())
                        {
                            ltlCarriers.Text = string.Join("<br />", carrierLines.Select(i => i.Title).ToArray());
                        }
                        
                    }
                    else
                    {
                        //prazno
                    }
                }

            }
        }
    }
}