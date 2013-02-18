using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGoogleWeather
{
    public class ForecastInformation
    {
        public string City
        { get; set; }
        public string PostalCode
        { get; set; }
        public DateTime Date
        { get; set; }
        public CurrentCondition CurrentCondition
        { get; set; }
        public List<ForecastCondition> ForecastCondition
        { get; set; }

        public ForecastInformation()
        {
            this.City = string.Empty;
            this.PostalCode = string.Empty;
            this.Date = new DateTime();
            this.CurrentCondition = new CurrentCondition();
            this.ForecastCondition = new List<ForecastCondition>();
        }
    }
}
