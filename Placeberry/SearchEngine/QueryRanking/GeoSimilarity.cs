using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace QueryRanking
{
    class GeoPoint {
        public Int32 id;
        public Double longitude;
        public Double latitude;
        public Int32 parentId;
    }

    class GeoSimilarity
    {
        private Double scale = 1.0;

        private Dictionary<Int32, GeoPoint> points = new Dictionary<int, GeoPoint>();

        public GeoSimilarity() { }

        public void InitFromDB()
        {
            try
            {
                SqlConnection conn = new SqlConnection("user id=mrihr_User;" +
                                            "password=mriusr123;" +
                                            "server=mssql4.mojsite.com,1555;" +
                                            "database=mrihr_PlaceberryAxilis");
                conn.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT Id, Latitude, Longitude, ParentId FROM SearchGeoplace", conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    GeoPoint point = new GeoPoint();
                    point.id = Int32.Parse(reader["Id"].ToString());
                    point.latitude = Double.Parse(reader["Latitude"].ToString());
                    point.longitude = Double.Parse(reader["Longitude"].ToString());
                    point.parentId = ("".Equals(reader["ParentId"].ToString()) ? -1 : Int32.Parse(reader["ParentId"].ToString()));
                    points.Add(point.id, point);
                }

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Double GetDistance(int adUID, int queryUID, bool scale=true)
        {
            if (!points.ContainsKey(adUID) || !points.ContainsKey(queryUID)) return 0;

            if (IsParent(adUID, queryUID)) return 2;

            if (scale) return ScaleDistance(Dist(adUID, queryUID));
            return Dist(adUID, queryUID);
        }

        private bool IsParent(int uidPoint1, int uidPoint2)
        {
            if (uidPoint1 == uidPoint2) return true;

            int curr = uidPoint1;
            
            while (points[curr].parentId != -1)
            {
                curr = points[curr].parentId;
                if (curr == uidPoint2) return true;
            }

            return false;
        }

        public Boolean ContainsUID(int uid)
        {
            return points.ContainsKey(uid);
        }

        private Double ScaleDistance(Double distance)
        {
            return Math.Exp(-scale * distance);
        }

        private Double Dist(int id1, int id2)
        {
            double x1 = points[id1].longitude, x2 = points[id2].longitude;
            double y1 = points[id1].latitude, y2 = points[id2].latitude;
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}
