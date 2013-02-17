using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueryRanking;
using CroatianAnalyzer;

namespace QueryRankingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //CroatianStemmer stemmer = new CroatianStemmer();

            //while (true)
            //{
            //    String line = Console.ReadLine();
            //    Console.WriteLine(stemmer.Stem(line));
            //}
            //Console.Read();
            

            //LRUCache<String, int> cache = new LRUCache<string, int>(5);

            //cache.Add("jen", 1);
            //cache.Add("dva", 2);
            //cache.Add("tri", 3);
            //cache.Add("cetiri", 4);
            //Console.WriteLine(cache.Contains("jen"));
            //cache.Add("pet", 5);
            //Console.WriteLine(cache.Contains("jen"));
            //cache.Add("sest", 6);
            //Console.WriteLine(cache.Contains("jen"));
            //cache.Add("sedam", 7);
            //Console.WriteLine(cache.Contains("jen"));
            //Console.WriteLine(cache.Contains("dva"));
            //Console.WriteLine(cache.Contains("tri"));
            //Console.Read();

            TextSimilarity.indexPath = @"C:\Users\matija\Documents\Visual Studio 2012\Projects\Placeberry\PlaceSolution\Placeberry\LuceneIndex";
            CachedRanker ranker = CachedRanker.Instance;
            Dictionary<int, Advert> adverts = ranker.ranker.advertList.GetAllAdverts();

            Console.WriteLine("\nReady! Enter queries: ");

            while (true)
            {
                String query = Console.ReadLine();

                Console.WriteLine("Broj rezultata: " + ranker.SearchCount(query, 2));
                List<int> scores = ranker.Search(query, 1, 0, 10);

                int count = 0;
                foreach (int id in scores)
                {
                    count++;

                    if (count > 5) break;

                    Advert ad = adverts[id];
                    Console.WriteLine("ID: " + id);
                    Console.WriteLine("ID: " + id + " SCORE: " + ad.score.TotalScore());
                    Console.WriteLine(ad.score);

                    Console.WriteLine("Geo:");
                    Console.WriteLine(" " + ad.country + ", " + ad.region + ", " + ad.island + ", " + ad.city);

                    if (ad.priceFrom >= 0)
                    {
                        Console.WriteLine("Price:");
                        Console.WriteLine(" " + ad.priceFrom + " - " + ad.priceTo + " " + (ad.currencyId == 1 ? "HRK" : "EUR"));
                    }

                    if (ad.capacityMin != -1)
                    {
                        Console.WriteLine("Capacity:");
                        Console.WriteLine(" " + ad.capacityMin + " - " + ad.capacityMax);
                    }

                    if (!ad.dateFrom.Equals(new DateTime(0)))
                    {
                        Console.WriteLine("Date:");
                        Console.WriteLine(" " + ad.dateFrom + " - " + ad.dateTo);
                    }

                    Console.WriteLine(ad.accommodation);
                    Console.WriteLine();
                    Console.WriteLine(ad.title);
                    Console.WriteLine();
                    Console.WriteLine(ad.description);
                    Console.WriteLine("---------");
                    Console.WriteLine();
                }
            }

            Console.Read();
        }
    }
}
