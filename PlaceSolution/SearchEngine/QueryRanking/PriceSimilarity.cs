using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QueryRanking
{
    class PriceSimilarity
    {
        private static double MAX = 10000000;
        private static double EPS = 1e-9;

        public PriceSimilarity() { }

        public Double CalculateScore(Advert ad, List<ParseToken> priceTokens)
        {

            double l = ad.priceFrom, h = ad.priceTo;

            if (priceTokens.Count == 0) return 1.0;
            if (l == -1 && h == -1) return 0.8;

            if (l == -1) l = 0;
            if (h == -1) h = MAX;

            double lq = 0.0, hq = MAX;
            
            foreach (ParseToken token in priceTokens)
            {
                string tag = token.word.parseTag;

                if ("PRICE_MIN_X".Equals(tag))
                {
                    lq = ParseAndAdjustCurrency(token.text, ad.currencyId);
                }
                else if ("PRICE_MAX_X".Equals(tag))
                {
                    hq = ParseAndAdjustCurrency(token.text, ad.currencyId);
                }
                else if ("PRICE_MIN_X_MAX_Y".Equals(tag))
                {
                    string levi = "", desni = "";

                    int bio = 0;
                    for (int i = 0; i < token.text.Length; ++i)
                    {
                        if (Char.IsDigit(token.text[i])) bio = 1;
                        if (bio == 1 && !char.IsDigit(token.text[i])) {
                            levi = token.text.Substring(0, i);
                            desni = token.text.Substring(i);
                            break;
                        }
                    }

                    if ("".Equals(levi) || "".Equals(desni)) continue;

                    String zz = Regex.Match(levi, @"[1-9][0-9]*([.,][0-9][0-9])?").Value;
                    zz.Replace(',', '.');
                    lq = Double.Parse(zz);

                    zz = Regex.Match(desni, @"[1-9][0-9]*([.,][0-9][0-9])?").Value;
                    zz.Replace(',', '.');
                    hq = Double.Parse(zz);

                    int cid = ad.currencyId;
                    int cid2 = 2;

                    if (Regex.IsMatch(desni, @"kun|hr?\skn|kn")) cid2 = 1;

                    if (cid == 1 && cid2 == 2)
                    {
                        lq *= 7.5;
                        hq *= 7.5;
                    }
                    else if (cid == 2 && cid2 == 1)
                    {
                        lq /= 7.5;
                        hq /= 7.5;
                    }
                }
            }

            return HardBoundScore(l, h, lq, hq);
        }

        private double HardBoundScore(double ad_low, double ad_high, double q_low, double q_high)
        {
            double ll = Math.Max(ad_low, q_low);
            double hh = Math.Min(ad_high, q_high);

            return (ll - EPS <= hh ? 1 : 0);
        }

        private double LinearSoftBoundScore(double ad_low, double ad_high, double q_low, double q_high)
        {
            if (Math.Abs(ad_low - ad_high) < EPS)
            {
                // Advert price is fixed.
                return (q_low - EPS <= ad_low && ad_low <= q_high + EPS ? 1 : 0);
            }

            // Calculating intersection interval.
            double int_low = Math.Max(ad_low, q_low);
            double int_high = Math.Min(ad_high, q_high);

            if (q_high != MAX && int_low - 5 <= int_high && Math.Abs(q_high - q_low) > EPS)
            {
                // Query price has an upper bound.
                return Math.Max(0, 0.9 + 0.1 * ((int_high - int_low) / (q_high - q_low)));
            }

            if (Math.Abs(q_low - q_high) < EPS)
            {
                // Query price is fixed.
                return (ad_low - EPS <= q_low && q_low <=  + EPS ? 1 : 0);
            }

            // Otherwise just test if they intersect.
            return (int_high > int_low - EPS ? 1 : 0);
        }

        //private double SoftScore(double ad_low, double ad_high, double q_low, double q_high)
        //{
        //    // NEDOVRSENO.

        //    if (Math.Abs(ad_low - ad_high) < EPS)
        //    {
        //        // Advert price is fixed.
        //        return (q_low - EPS <= ad_low && ad_low <= q_high + EPS ? 1 : 0);
        //    }

        //    // Calculating intersection interval.
        //    double ll = Math.Max(ad_low, q_low);
        //    double hh = Math.Min(ad_high, q_high);

        //    return 0;
        //}
        
        private double ParseAndAdjustCurrency(string word, int cid)
        {
            String zz = Regex.Match(word, @"[1-9][0-9]*([.,][0-9][0-9])?").Value;
            zz.Replace(',', '.');
            Double tmp = Double.Parse(zz);

            int cid2 = 2;

            if (Regex.IsMatch(word, @"kun|hr?\skn|kn")) cid2 = 1;

            if (cid == 1 && cid2 == 2)
            {
                tmp *= 7.5;
            }
            else if (cid == 2 && cid2 == 1)
            {
                tmp /= 7.5;
            }

            return tmp;
        }
    }
}
