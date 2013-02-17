using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGoogleWeather
{
    public class ForecastCondition
    {
        public string DayOfWeek
        { get; set; }
        public string LowC
        { get; set; }
        public string HighC
        { get; set; }
        public string Condition
        { get; set; }
        public string Icon
        { get; set; }

        public ForecastCondition()
        {
            this.DayOfWeek = string.Empty;
            this.LowC = string.Empty;
            this.HighC = string.Empty;
            this.Condition = string.Empty;
            this.Icon = string.Empty;
        }
    }
}
