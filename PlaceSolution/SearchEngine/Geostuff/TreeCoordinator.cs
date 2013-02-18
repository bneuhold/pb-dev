using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Geostuff
{
    class TreeCoordinator
    {
        Dictionary<int, List<int>> forest = new Dictionary<int, List<int>>();
        Dictionary<int, Tuple<String, Double, Double, int>> data = new Dictionary<int, Tuple<string, double, double, int>>();
        List<int> roots = new List<int>();
        List<int> updated = new List<int>();

        public TreeCoordinator() { }

        public void Init()
        {
            
            try
            {
                SqlConnection conn = new SqlConnection("user id=mosrecki;" +
                                            "password=pero99;" +
                                            "server=mriserver.dyndns.org;" +
                                            "database=PlaceberryAxilis;" +
                                            "connection timeout=2");
                conn.Open();


                SqlCommand command = new SqlCommand(
                    "SELECT Id, UltimateTableId, ParentId, Latitude, Longitude, SubtreeCount FROM GeoPlace", conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    String id = reader["Id"].ToString();
                    Int32 ultimateTableId = Int32.Parse(reader["UltimateTableId"].ToString());

                    Int32 parentId = "".Equals(reader["ParentId"].ToString()) ? -1 : Int32.Parse(reader["ParentId"].ToString());
                    Double lat = "".Equals(reader["Latitude"].ToString()) ? -5000 : Double.Parse(reader["Latitude"].ToString());
                    Double lon = "".Equals(reader["Longitude"].ToString()) ? -5000 : Double.Parse(reader["Longitude"].ToString());
                    Int32 count = Int32.Parse(reader["SubtreeCount"].ToString());

                    data.Add(ultimateTableId, new Tuple<String, Double, Double, int> (id, lat, lon, count));

                    if (parentId == -1)
                    {
                        roots.Add(ultimateTableId);
                    }
                    else
                    {
                        if (!forest.ContainsKey(parentId))
                            forest.Add(parentId, new List<int>());

                        forest[parentId].Add(ultimateTableId);
                    }
                }

                Console.WriteLine(roots.Count);
                Console.WriteLine(data.Count);

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Run()
        {
            foreach (Int32 root in roots)
            {
                DFS(root);
            }

            try
            {
                SqlConnection conn = new SqlConnection("user id=mosrecki;" +
                                            "password=pero99;" +
                                            "server=mriserver.dyndns.org;" +
                                            "database=PlaceberryAxilis;" +
                                            "connection timeout=2");
                conn.Open();

                foreach (Int32 node in updated)
                {
                    Tuple<String, Double, Double, int> podaci = data[node];
                    String query = String.Format("UPDATE GeoPlace SET latitude={0}, longitude={1}, subtreecount={2} WHERE Id = {3}",
                            podaci.Item2, podaci.Item3, podaci.Item4, podaci.Item1);
                    SqlCommand command = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();
                }

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void DFS(Int32 node)
        {
            if (!forest.ContainsKey(node)) return;

            int count = 0;
            double lat = 0;
            double lon = 0;

            foreach (Int32 child in forest[node])
            {
                DFS(child);
                Tuple<String, Double, Double, int> dete = data[child];
                if (dete.Item2 > -1000)
                {
                    lat += dete.Item2 * dete.Item4;
                    lon += dete.Item3 * dete.Item4;
                    count += dete.Item4;
                }
            }

            if (count != 0)
            {
                lat /= count;
                lon /= count;

                Tuple<String, Double, Double, int> podaci = data[node];
                data[node] = new Tuple<string, double, double, int>(podaci.Item1, lat, lon, count);
                
                updated.Add(node);
            }
        }
    }
}
