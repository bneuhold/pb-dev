using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibWeather
{
    public class HourlyWeather
    {
        public DateTime Hour
        {
            get;
            set;
        }        
        public string Description
        {
            get;
            set;
        }
        public string Temperature
        {
            get;
            set;
        }
        public string RealFeel
        {
            get;
            set;
        }
        public string Humidity
        {
            get;
            set;
        }
        public string WindDirection
        {
            get;
            set;
        }
        public string WindSpeed
        {
            get;
            set;
        }
    }
}
