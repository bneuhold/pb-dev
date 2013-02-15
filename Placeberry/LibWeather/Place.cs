using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibWeather
{
    public class Place : IEqualityComparer<Place>
    {
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Parameters
        {
            get;
            set;
        }
        public LocalWeather LocalWeather
        {
            get;
            set;
        }

        public bool Equals(Place left, Place right)
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

        public int GetHashCode(Place place)
        {
            return (place.Id).GetHashCode();
        }
    }
}
