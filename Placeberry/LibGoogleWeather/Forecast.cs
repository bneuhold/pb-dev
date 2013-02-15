using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace LibGoogleWeather
{
    public class Forecast:IEqualityComparer<Forecast>
    {
        //Primjer linka
        //google.com/ig/api?weather=zagreb
        private const int WAIT_TIME = 20 * 1000;

        private string urlWeekThis = "http://www.google.com/ig/api?weather={0}&hl=hr&ie=utf-8&oe=utf-8";
        private DateTime timeFetched;

        public int Id
        { get; set; }
        public string Title
        { get; set; }
        public string Param
        { get; set; }
        public ForecastInformation ForecastInformation
        { get; set; }
        public DateTime TimeFetched
        {
            get { return this.timeFetched; }
        }

        public Forecast(int id, string title, string param)
        {
            this.Id = id;
            this.Title = title;
            this.Param = param;

            this.urlWeekThis = string.Format(urlWeekThis, param);
        }

        public void FetchForecast()
        {
            GetForecast();
        }

        private void GetForecast()
        {
            Page pageWeekThis = new Page { Url = urlWeekThis };

            ManualResetEvent doneWeekThis = new ManualResetEvent(false);

            GetHtml(doneWeekThis, pageWeekThis);

            doneWeekThis.WaitOne();

            this.ForecastInformation = DoParseForecast(pageWeekThis.Html);

            timeFetched = DateTime.Now;
        }
        private void GetHtml(ManualResetEvent done, Page page)
        {
            HttpWebRequest request = HttpWebRequest.Create(page.Url) as HttpWebRequest;

            IAsyncResult requestAR = request.BeginGetResponse(i =>
            {
                Page p = i.AsyncState as Page;
                var response = request.EndGetResponse(i);

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                p.Html = reader.ReadToEnd();

                reader.Close();
                response.Close();

                done.Set();

            }, page);

            ThreadPool.RegisterWaitForSingleObject(requestAR.AsyncWaitHandle, (state, timedOut) =>
            {
                if (timedOut)
                {
                    HttpWebRequest r = state as HttpWebRequest;
                    if (r != null)
                    {
                        r.Abort();
                    }
                }

            }, request, WAIT_TIME, true);
        }
        private ForecastInformation DoParseForecast(string text)
        {
            ForecastInformation forecast = new ForecastInformation();

            try
            {
                XDocument xml = XDocument.Parse(text);
             
                var weather = xml.Element("xml_api_reply").Element("weather");
                var forecastInfo = weather.Element("forecast_information");

                forecast.City = forecastInfo.Element("city").Attribute("data").Value;
                forecast.PostalCode = forecastInfo.Element("postal_code").Attribute("data").Value;
                forecast.Date = DateTime.Parse(forecastInfo.Element("forecast_date").Attribute("data").Value).Date;

                CurrentCondition current = new CurrentCondition();
                forecast.CurrentCondition = current;

                var currentCond = weather.Element("current_conditions");

                current.Condition = currentCond.Element("condition").Attribute("data").Value;
                current.TempC = currentCond.Element("temp_c").Attribute("data").Value;
                current.TempF = currentCond.Element("temp_f").Attribute("data").Value;
                current.Humidity = currentCond.Element("humidity").Attribute("data").Value;
                current.Wind = currentCond.Element("wind_condition").Attribute("data").Value;
                current.Icon = currentCond.Element("icon").Attribute("data").Value;

                forecast.ForecastCondition = (from p in weather.Elements("forecast_conditions")
                                              select new ForecastCondition()
                                              {
                                                  DayOfWeek = p.Element("day_of_week").Attribute("data").Value,
                                                  Condition = p.Element("condition").Attribute("data").Value,
                                                  HighC = p.Element("high").Attribute("data").Value,
                                                  LowC = p.Element("low").Attribute("data").Value,
                                                  Icon = p.Element("icon").Attribute("data").Value,
                                              }).ToList();
            }
            catch (Exception)
            {
            }


            return forecast;
        }


        public bool Equals(Forecast left, Forecast right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            return left.Id == right.Id;
        }
        public int GetHashCode(Forecast forecast)
        {
            return (forecast.Id).GetHashCode();
        }
    }
}
