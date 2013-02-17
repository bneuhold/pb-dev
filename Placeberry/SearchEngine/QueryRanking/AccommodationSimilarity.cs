using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryRanking
{
    class AccommodationSimilarity
    {

        public AccommodationSimilarity() { }

        public Double CalculateScore(Advert ad, List<ParseToken> tokens)
        {
            if (tokens.Count == 0) return 1.0;

            foreach (var t in tokens)
            {
                if (t.word.termId == ad.accUID) return 1.0;
            }

            return 0.45;
        }
    }
}
