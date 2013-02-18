using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Geostuff
{
    class GeoDistCalculator
    {
        public const Double scale = 1.0;

        private Dictionary<Int32, Tuple<Double, Double, Int32>> graph = new Dictionary<int,Tuple<double,double,int>>();


        public void GeoDistCalculat() { }

        public void Init() { 
            try
            {
                SqlConnection conn = new SqlConnection("user id=mosrecki;" +
                                            "password=pero99;" +
                                            "server=mriserver.dyndns.org;" +
                                            "database=PlaceberryAxilis;" +
                                            "connection timeout=2");
                conn.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT UltimateTableId, ParentId, Latitude, Longitude FROM GeoPlace WHERE Latitude IS NOT NULL AND Longitude IS NOT NULL", conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) 
                {
                    Int32 ultimateTableId = Int32.Parse(reader["UltimateTableId"].ToString());
                    Int32 parentId = "".Equals(reader["ParentId"].ToString()) ? -1 : Int32.Parse(reader["ParentId"].ToString());
                    Double lat = Double.Parse(reader["Latitude"].ToString());
                    Double lon = Double.Parse(reader["Longitude"].ToString());

                    graph.Add(ultimateTableId, new Tuple<Double, Double, Int32>(lat, lon, parentId));
                }

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public double GetDistance(int utableId1, int utableId2)
        {
            return Dist(utableId1, utableId2);

            Dictionary<int, int> path = new Dictionary<int, int>();

            // Penjemo se gore od prvog cvora.
            int curr1 = utableId1;
            path[curr1] = curr1;
            while (graph[curr1].Item3 != -1) 
            {
                int next = graph[curr1].Item3;
                path[next] = curr1;
                curr1 = next;
            }

            // Penjemo se gore od drugog cvora dok ne naletimo na prosli path.
            int curr2 = utableId2;
            int past = curr2;
            while (graph[curr2].Item3 != -1 && !path.ContainsKey(curr2)) 
            {
                past = curr2;
                curr2 = graph[curr2].Item3;
            }

            int prvi, drugi;

            if (!path.ContainsKey(curr2)) 
            {
                // Ako se nikad ne spoje usporedjujemo dva najvisa cvora do kojih smo dosli.
                prvi = curr1;
                drugi = curr2;
            } else {
                // Inace su se nasli, znaci usporedit cemo prosla dva cvora (dva childa).
                prvi = path[curr1];
                drugi = past;
            }

            // Udaljenost dva odgovarajuca deteta onog cvora u kojem su se nasli.
            double dist = Dist(prvi, drugi);

            Console.WriteLine(dist);

            // Udaljenost od prvog cvora do gore.
            curr1 = utableId1;
            while (curr1 != prvi)
            {
                int next = graph[curr1].Item3;
                dist += Dist(curr1, next);
                Console.WriteLine(Dist(curr1, next));
                curr1 = next;
            }
            
            // Udaljenost drugog cvora do gore.
            curr2 = utableId2;
            while (curr2 != drugi)
            {
                int next = graph[curr2].Item3;
                dist += Dist(curr2, next);
                Console.WriteLine(Dist(curr2, next));

                curr2 = next;
            }

            return dist;
        }

        
        public static double ScaleDistance(double distance)
        {
            return Math.Exp(-scale * distance);
        }

        private double Dist(int id1, int id2)
        {
            double x1 = graph[id1].Item1, x2 = graph[id2].Item1;
            double y1 = graph[id1].Item2, y2 = graph[id2].Item2;
            return Math.Sqrt((x1-x2)*(x1-x2) + (y1-y2)*(y1-y2));
        }
    }
}
