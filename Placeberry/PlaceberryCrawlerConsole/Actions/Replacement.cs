using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PlaceberryCrawlerConsole
{
    public class Replacement
    {
        private string _from;
        private string _to;
        private ReplacementType _type;


        public Replacement(string from, string to, ReplacementType type)
        {
            _from = from;
            _to = to;
            _type = type;
        }

        public string Replace(string value)
        {
            string retVal = value;

            switch (_type)
            {
                case ReplacementType.Text:
                    retVal = retVal.Replace(_from, _to);
                    break;

                case ReplacementType.Regex:
                    retVal = Regex.Replace(retVal, _from, _to, RegexOptions.IgnoreCase);
                    break;
            }

            return retVal;
        }
    }

    public enum ReplacementType
    {
        Text,
        Regex
    }
}
