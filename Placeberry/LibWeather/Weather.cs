using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibWeather
{
    public class Weather
    {
        
        public Weather()
        {
            HourlyWeather = new List<HourlyWeather>();
        }

        public DateTime Date
        {
            get;
            set;
        }
        public string TemperatureDay
        {
            get;
            set;
        }
        public string TemperatureNight
        {
            get;
            set;
        }
        public string RealFeelDay
        {
            get;
            set;
        }
        public string RealFeelNight
        {
            get;
            set;
        }
        public string DescriptionDay
        {
            get;
            set;
        }
        public string DescriptionNight
        {
            get;
            set;
        }
        public string IconDay
        {
            get;
            set;
        }
        public string IconNight
        {
            get;
            set;
        }

        public List<HourlyWeather> HourlyWeather
        {
            get;
            set;
        }


    }
}
