using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QueryRanking
{
    class CapacitySimilarity
    {
        public CapacitySimilarity() { }

        public Double CalculateScore(Advert ad, List<ParseToken> capacityTokens)
        {
            if (capacityTokens.Count == 0) return 1.0;
            if (ad.capacityMin == -1 && ad.capacityMax == -1) return 0.8;

            int noofpeeps = 1;

            string tag = capacityTokens[0].word.parseTag;

            for (int i = 2; i <= 10; ++i)
            {
                if (String.Format(@"CAPACITY_{0}_PEOPLE", i).Equals(tag))
                {
                    noofpeeps = i;
                }
            }

            if ("CAPACITY_1_PERSON".Equals(capacityTokens[0].word.parseTag))
            {
                noofpeeps = 1;
            }
            else if ("CAPACITY_X_PEOPLE".Equals(capacityTokens[0].word.parseTag))
            {
                noofpeeps = Int32.Parse(Regex.Match(capacityTokens[0].text, @"\d+").Value);

            }

            if (ad.capacityMax == -1) ad.capacityMax = 10000000;

            if (ad.capacityMin <= noofpeeps && noofpeeps <= ad.capacityMax) return 1.0;

            if (noofpeeps > 3 && noofpeeps == ad.capacityMin - 1) return 0.6;

            if (noofpeeps > 3 && noofpeeps == ad.capacityMax + 1) return 0.3;

            return 0.0;
        }
    }
}
