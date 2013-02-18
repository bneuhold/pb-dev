using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibGoogleWeather;

public partial class Controls_GoogleForecast : System.Web.UI.UserControl
{
    private int forecastId = 0;
    public int ForecastId
    {
        get { return this.forecastId; }
        set { this.forecastId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Place place = null;
        Forecast forecast = null;
        ForecastInformation info = null;

        place = WeatherMaster.Places.Where(i => i.Id == this.forecastId).SingleOrDefault();

        forecast = place != null ? place.Forecast : null;
        info = forecast != null ? forecast.ForecastInformation : null;

        this.Visible = false;

        if (info != null)
        {
            ltToday.Text = info.Date.DayOfWeek + " " + info.Date.ToShortDateString();
            ltlCondition.Text = info.CurrentCondition.Condition;
            ltlHumidity.Text = info.CurrentCondition.Humidity;
            ltlTempC.Text = info.CurrentCondition.TempC;
            ltlWind.Text = info.CurrentCondition.Wind;
            imgIcon.Src = info.CurrentCondition.Icon.Replace("ig", "resources");
            imgIcon.Alt = info.CurrentCondition.Condition;

            repForecastDays.DataSource = info.ForecastCondition;
            repForecastDays.DataBind();

            this.Visible = true;
        }

    }
}