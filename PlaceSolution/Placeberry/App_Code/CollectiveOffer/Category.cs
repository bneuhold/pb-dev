using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Collective
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Priority { get; private set; }
        public bool Active { get; private set; }

        public Category()   // dummy za prazan grid
        {
            this.Id = 0;
            this.Name = String.Empty;
            this.Active = false;
            this.Priority = 0;
        }

        private Category(int id, string name, int priority, bool active)
        {
            this.Id = id;
            this.Name = name;
            this.Priority = priority;
            this.Active = active;
        }

        public static Category CreateCategory(string name, int priority, bool active)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_CreateCategory"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 250) { Value = name });
                    cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.Int, 4) { Value = priority });
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active });

                    SqlParameter parRetValId = new SqlParameter("@Id", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parRetValId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return new Category(Convert.ToInt32(parRetValId.Value), name, priority, active);
                }
            }
        }

        public static Category GetCategory(int id)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetCategory"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = id });
                    SqlParameter parName = new SqlParameter("@Name", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parName);
                    SqlParameter parPriority = new SqlParameter("@Priority", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parPriority);
                    SqlParameter parActive = new SqlParameter("@Active", SqlDbType.Bit, 1) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parActive);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (parName.Value == DBNull.Value)
                        return null;

                    return new Category(id, parName.Value.ToString(), Convert.ToInt32(parPriority.Value), Convert.ToBoolean(parActive.Value));
                }
            }
        }


        public static Category UpdateCategory(int id, string name, int priority, bool active)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_UpdateCategory"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = id });
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 250) { Value = name });
                    cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.Int, 4) { Value = priority });
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return new Category(id, name, priority, active);
                }
            }
        }


        public static void DeleteCategory(int id)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_DeleteCategory"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = id });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }


        public static List<Category> ListCategories(bool? active)
        {
            List<Category> lst = new List<Category>();

            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListCategoriesForAdmin"
                })
                {
                    if (active.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active.Value });
                    }

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Category(
                                Convert.ToInt32(rdr["Id"]),
                                rdr["Name"].ToString(),
                                Convert.ToInt32(rdr["Priority"]),
                                Convert.ToBoolean(rdr["Active"])
                                ));
                        }

                        rdr.Close();
                    }
                    con.Close();
                }
            }
            return lst;
        }
    }
}