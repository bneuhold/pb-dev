using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGoogleWeather
{
    public class Place
    {
        public int Id
        { get; set; }
        public string Title
        { get; set; }
        public string Param
        { get; set; }
        public Forecast Forecast
        { get; set; }

        public Place(int id, string title, string param)
        {
            this.Id = id;
            this.Title = title;
            this.Param = param;
        }
    }
}
