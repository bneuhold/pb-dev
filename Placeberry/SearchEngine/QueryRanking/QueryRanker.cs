using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QueryRanking
{
    public class QueryRanker
    {
        public AdvertList advertList;

        private Parser queryParser;
        private Dictionary<int, Word> words;

        private AccommodationSimilarity accSimilarity;
        private GeoSimilarity geoSimilarity;
        private TextSimilarity textSimilarity;
        private PriceSimilarity priceSimilarity;
        private DateSimilarity dateSimilarity;
        private CapacitySimilarity capacitySimilarity;

        public QueryRanker()
        {
            queryParser = new Parser();
            advertList = new AdvertList();

            accSimilarity = new AccommodationSimilarity();
            geoSimilarity = new GeoSimilarity();
            textSimilarity = new TextSimilarity();
            priceSimilarity = new PriceSimilarity();
            dateSimilarity = new DateSimilarity();
            capacitySimilarity = new CapacitySimilarity();

            Init();
        }

        private void Init()
        {
            Console.WriteLine("Initializing geographic search..");
            geoSimilarity.InitFromDB();

            Console.WriteLine("Initializing text search..");
            textSimilarity.Init();

            Console.WriteLine("Initializing query parser..");
            queryParser.Init();

            Console.WriteLine("Reading advert list..");
            advertList.InitFromDB();
            
            words = queryParser.GetAllWords();
        }

        public List<AdvertScore> Search(String query, Int32 languageId)
        {
            GroupedParseResult res = Parse(query, languageId);
            return SearchParsed(res, languageId);
        }

        public GroupedParseResult Parse(String query, Int32 languageId)
        {
            string rest;
            List<ParseToken> parseRes = queryParser.Parse(query, languageId, out rest);

            GroupedParseResult res = new GroupedParseResult();

            StringBuilder sb = new StringBuilder();
            sb.Append(rest).Append(" ");

            foreach (ParseToken token in parseRes)
            {
                Word word = words[token.termId];
                int otid = word.termTypeId;

                Console.WriteLine(token.text + " " + word.title + " " + otid);

                if (otid <= 7)
                {
                    if (geoSimilarity.ContainsUID(word.geoplaceId)) res.geoTokens.Add(token);
                    else sb.Append(token.word).Append(" ");
                }
                else if (otid == 12) res.accTokens.Add(token);
                else if (otid == 13) res.capacityTokens.Add(token);
                else if (otid == 14) res.priceTokens.Add(token);
                else if (otid == 15) res.dateTokens.Add(token);
                else sb.Append(token.word).Append(" ");
            }

            Console.WriteLine("\nAccommodation:");
            foreach (ParseToken t in res.accTokens) Console.WriteLine(" " + t.termId + " | " + t.text);

            Console.WriteLine("\nGeo:");
            foreach (ParseToken t in res.geoTokens) Console.WriteLine(" " + t.termId + " | " + t.text);

            Console.WriteLine("Price:");
            foreach (ParseToken t in res.priceTokens) Console.WriteLine(" " + t.termId + " | " + t.text);

            Console.WriteLine("Capacity:");
            foreach (ParseToken t in res.capacityTokens) Console.WriteLine(" " + t.termId + " | " + t.text);

            Console.WriteLine("Date:");
            foreach (ParseToken t in res.dateTokens) Console.WriteLine(" " + t.termId + " | " + t.text);

            Console.WriteLine("Rest: ");
            Console.WriteLine(" " + sb.ToString() + "\n");

            res.rest = sb.ToString().Trim();

            return res;
        }

        public List<AdvertScore> SearchParsed(GroupedParseResult parsedResults, int languageId)
        {
            List<AdvertScore> scores = new List<AdvertScore>();

            textSimilarity.ProcessQuery(parsedResults.rest, languageId);

            foreach (Advert ad in advertList.GetAllAdverts().Values)
            {
                if (ad.languageId != languageId) continue;

                AdvertScore score = new AdvertScore();
                score.advertId = ad.id;

                // 1. Text search.
                score.textScore = textSimilarity.GetScore(ad.id);

                // 2. Geo.
                score.geoScore = CalculateGeoScore(ad, parsedResults.geoTokens);
                if (parsedResults.geoTokens.Count == 0) score.geoScore = 1;

                // 3. Price.
                score.priceScore = priceSimilarity.CalculateScore(ad, parsedResults.priceTokens);

                // 4. Capacity.
                score.capacityScore = capacitySimilarity.CalculateScore(ad, parsedResults.capacityTokens);

                // 5. Date.
                score.dateScore = dateSimilarity.CalculateScore(ad, parsedResults.dateTokens);

                // 6. Accommodation.
                score.accScore = accSimilarity.CalculateScore(ad, parsedResults.accTokens);

                if (score.TotalScore() < 1) continue;
                ad.score = score;

                scores.Add(score);
            }

            scores.Sort();
            scores.Reverse();

            return scores;
        }

        private Double CalculateGeoScore(Advert ad, List<ParseToken> geoTokens)
        {
            if (ad.geoUID == -1) return 0.0;

            int count = 0;
            double sum = 0;
            foreach (ParseToken gt in geoTokens)
            {
                double ts = geoSimilarity.GetDistance(ad.geoUID, words[gt.termId].geoplaceId);
                if (ts > 1e-9)
                {
                    count++;
                    sum += ts;
                }
            }

            return (count == 0 ? 0.0 : sum / count);
        }
    }

    public class AdvertScore : IComparable
    {
        public int advertId;
        public double textScore;
        public double geoScore;
        public double priceScore = 1.0;
        public double capacityScore = 1.0;
        public double dateScore = 1.0;
        public double accScore = 1.0;

        public double TotalScore()
        {
            return (1 + textScore) * geoScore * priceScore * capacityScore * dateScore * accScore;
        }

        public int CompareTo(object obj)
        {
            AdvertScore rl = (AdvertScore)obj;
            return TotalScore().CompareTo(rl.TotalScore());
        }

        public override string ToString()
        {
            return String.Format("GS: {0} TS: {1} PC: {2} CS: {3} DS: {4}\n", geoScore, textScore, priceScore, capacityScore, dateScore);
        }
    };

    public class GroupedParseResult
    {
        public List<ParseToken> geoTokens = new List<ParseToken>();
        public List<ParseToken> accTokens = new List<ParseToken>();
        public List<ParseToken> priceTokens = new List<ParseToken>();
        public List<ParseToken> capacityTokens = new List<ParseToken>();
        public List<ParseToken> dateTokens = new List<ParseToken>();
        public string rest = "";
    }
}
