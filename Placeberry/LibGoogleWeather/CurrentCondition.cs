using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGoogleWeather
{
    public class CurrentCondition
    {
        public string Condition
        { get; set; }
        public string TempF
        { get; set; }
        public string TempC
        { get; set; }
        public string Humidity
        { get; set; }
        public string Wind
        { get; set; }
        public string Icon
        { get; set; }

        public CurrentCondition()
        {
            this.Condition = string.Empty;
            this.TempF = string.Empty;
            this.TempC = string.Empty;
            this.Humidity = string.Empty;
            this.Wind = string.Empty;
            this.Icon = string.Empty;
        }
    }
}
