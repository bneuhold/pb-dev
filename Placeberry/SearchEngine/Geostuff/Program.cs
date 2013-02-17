using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using FuzzyStrings;

namespace Geostuff
{
    class Program
    {
        static void Main(string[] args)
        {
            GeoDistCalculator dister = new GeoDistCalculator();

            dister.Init();

            while (true)
            {
                string[] line = Console.ReadLine().Split();
                int id1 = Int32.Parse(line[0]);
                int id2 = Int32.Parse(line[1]);
                double dist = dister.GetDistance(id1, id2);
                Console.WriteLine(dist + " " + GeoDistCalculator.ScaleDistance(dist));
            }

            Console.Read();
        }
    }
}
