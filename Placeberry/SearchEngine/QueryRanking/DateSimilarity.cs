using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QueryRanking
{
    class DateSimilarity
    {
        public DateSimilarity() { }
            
        public double CalculateScore(Advert ad, List<ParseToken> dateTokens)
        {
            if (dateTokens.Count == 0) return 1;
            if (ad.dateFrom.Equals(new DateTime(0)) || ad.dateTo.Equals(new DateTime(0))) return 0.8;

            ParseToken token = dateTokens[0];

            int mjesec = 0;
            string tag = token.word.parseTag;

            if ("DATE_MONTH".Equals(tag))
            {
                mjesec = Int32.Parse(Regex.Match(token.text, @"\d+").Value);
            } else {
                mjesec = Math.Min(token.termId - 1805, token.termId - 1817);
            }

            int m1 = ad.dateFrom.Month, m2 = ad.dateTo.Month;

            if (m1 <= m2) return (m1 <= mjesec && mjesec <= m2 ? 1 : 0);
            return (m1 <= mjesec || mjesec <= m2 ? 1 : 0);
        }
    }
}
