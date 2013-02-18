using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Collective
{
    public class CategoryTranslation
    {
        public int CategoryId { get; private set; }
        public int LanguageId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public CategoryTranslation()   // dummy konstruktor za grid
        {
            this.CategoryId = 0;
            this.LanguageId = 0;
            this.Title = string.Empty;
            this.Description = string.Empty;
        }


        private CategoryTranslation(int catId, int langId, string title, string desc)
        {
            this.CategoryId = catId;
            this.LanguageId = langId;
            this.Title = title;
            this.Description = desc;
        }

        public static CategoryTranslation CreateCategoryTranslaton(int catId, int langId, string title, string desc)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_CreateCategoryTranslation"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CatId", SqlDbType.Int, 4) { Value = catId });
                    cmd.Parameters.Add(new SqlParameter("@LangId", SqlDbType.Int, 4) { Value = langId });
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 250) { Value = title });
                    cmd.Parameters.Add(new SqlParameter("@Desc", SqlDbType.NVarChar, 1000) { Value = desc });

                    SqlParameter parRetValue = new SqlParameter("@ReturnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (Convert.ToInt32(parRetValue.Value) == 0)
                    {
                        return new CategoryTranslation(catId, langId, title, desc);
                    }

                    return null;
                }
            }
        }

        public static void UpdateCategoryTranslaton(int catId, int langId, string title, string desc)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_UpdateCategoryTranslation"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CatId", SqlDbType.Int, 4) { Value = catId });
                    cmd.Parameters.Add(new SqlParameter("@LangId", SqlDbType.Int, 4) { Value = langId });
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 250) { Value = title });
                    cmd.Parameters.Add(new SqlParameter("@Desc", SqlDbType.NVarChar, 1000) { Value = desc });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static void DeleteCategoryTranslaton(int catId, int langId)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_DeleteCategoryTranslation"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CatId", SqlDbType.Int, 4) { Value = catId });
                    cmd.Parameters.Add(new SqlParameter("@LangId", SqlDbType.Int, 4) { Value = langId });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static List<CategoryTranslation> ListCategoryTranslations(int catId)
        {
            List<CategoryTranslation> lst = new List<CategoryTranslation>();

            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListCategoryTranslations"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CatId", SqlDbType.Int, 4) { Value = catId });

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new CategoryTranslation(
                                Convert.ToInt32(rdr["CollectiveCategoryId"]),
                                Convert.ToInt32(rdr["LanguageId"]),
                                rdr["Title"].ToString(),
                                rdr["Description"].ToString()
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