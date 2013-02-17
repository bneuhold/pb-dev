using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibWeather;

public partial class Controls_WheaterForecast : System.Web.UI.UserControl
{
    protected Place place;

    private int placeId;

    public int PlaceId
    {
        get { return this.placeId; }
        set { this.placeId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        var place = (from p in WeatherMaster.Places
                     where p.Id == placeId
                     select p).SingleOrDefault();

        LocalWeather lw = null;
        Weather today = null;
        List<HourlyWeather> hourly = null;
        List<Weather> weekly = null;

        if (place != null)
        {
            lw = place.LocalWeather;
            today = lw == null ? null : lw.TodaysWeather;
            weekly = lw == null ? null : !lw.AllWeather.Any() ? null : lw.AllWeather;
            hourly = today == null ? null : !today.HourlyWeather.Any() ? null : today.HourlyWeather;
        }

        if (lw != null && today != null && hourly != null)
        {
            fvwPlace.DataSource = new List<Place> { place };
            fvwPlace.DataBind();

            if (today != null)
            {
                fvwToday.DataSource = new List<object> { today };
                fvwToday.DataBind();
            }
            else
            {
                fvwToday.Visible = false;
            }

            if (hourly != null)
            {
                repHourly.DataSource = today.HourlyWeather;
                repHourly.DataBind();
            }
            else
            {
                repHourly.Visible = false;
            }

            if (weekly != null)
            {
                repWeeks.DataSource = weekly;
                repWeeks.DataBind();
            }
            else
            {
                repWeeks.Visible = false;
            }
        }
        else
        {
            this.Visible = false;
        }

    }
}