using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzyStrings;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace Geostuff
{
    class Matcher
    {
        public Matcher() { }

        
        public void Run()
        {
            List<Tuple<String, Double, Double>> cities = new List<Tuple<string, double, double>>();
            List<Int32> ultimateTableId = new List<Int32>();

            string filePath = @"C:\Users\matija\Documents\Visual Studio 2012\Projects\Placeberry\geo\cities.txt";
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        int p1 = line.IndexOf('\t');
                        int p2 = line.IndexOf('\t', p1 + 1);
                        Double x = Double.Parse(line.Substring(0, p1));
                        Double y = Double.Parse(line.Substring(p1 + 1, p2 - p1 - 1));
                        String name = line.Substring(p2 + 1).ToLower();
                        cities.Add(new Tuple<string, double, double>(name, x, y));
                        ultimateTableId.Add(-1);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                SqlConnection conn = new SqlConnection("user id=mosrecki;" +
                                            "password=pero99;" +
                                            "server=mriserver.dyndns.org;" +
                                            "database=PlaceberryAxilis;" +
                                            "connection timeout=2");
                conn.Open();

                SqlConnection conn2 = new SqlConnection("user id=mosrecki;" +
                                            "password=pero99;" +
                                            "server=mriserver.dyndns.org;" +
                                            "database=PlaceberryAxilis;" +
                                            "connection timeout=2");
                conn2.Open();


                SqlCommand command = new SqlCommand(
                    "SELECT gp.Id id, ut.RegexExpression regex, ut.Title title FROM GeoPlace gp LEFT JOIN UltimateTable ut ON gp.UltimateTableId=ut.Id WHERE gp.ObjectTypeId=2", conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    String id = reader["id"].ToString();
                    if ("".Equals(reader["regex"])) continue;
                    Regex regex = new Regex(reader["regex"].ToString());

                    List<int> candidates = new List<int>();
                    int i = 0;
                    foreach (Tuple<String, Double, Double> c in cities)
                    {
                        if (regex.IsMatch(c.Item1))
                            candidates.Add(i);
                        ++i;
                    }

                    if (candidates.Count == 1)
                    {
                        Update(cities[candidates[0]].Item2, cities[candidates[0]].Item2, id, conn2);
                    }
                    else if (candidates.Count > 1)
                    {
                        int bind = 0, bval = LevenshteinDistanceExtensions.LevenshteinDistance(reader["title"].ToString(), cities[candidates[0]].Item1);

                        for (i = 1; i < candidates.Count; ++i)
                        {
                            int tval = LevenshteinDistanceExtensions.LevenshteinDistance(reader["title"].ToString(), cities[candidates[i]].Item1);
                            if (tval < bval)
                            {
                                bind = i;
                            }
                            Console.WriteLine(cities[candidates[i]].Item1);
                        }

                        Console.WriteLine(reader["title"].ToString() + " " + cities[candidates[bind]].Item1);

                        Update(cities[candidates[bind]].Item2, cities[candidates[bind]].Item2, id, conn2);
                    }
                }

                conn.Close();
                conn2.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Update(double x, double y, String id, SqlConnection conn)
        {
            String query = String.Format("UPDATE GeoPlace SET Longitude={0}, Latitude={1} WHERE Id={2}", x, y, id);
            SqlCommand command2 = new SqlCommand(query, conn);
            command2.ExecuteNonQuery();
        }
    }
}
