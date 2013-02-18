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
        public string MetaDesc { get; private set; }
        public string MetaKeywords { get; private set; }
        public string UrlTag { get; private set; }

        public CategoryTranslation() { }   // dummy konstruktor za grid

        public CategoryTranslation(int catId, int langId, string title, string desc, string metaDesc, string metaKW, string urlTag)
        {
            this.CategoryId = catId;
            this.LanguageId = langId;
            this.Title = title;
            this.Description = desc;
            this.MetaDesc = metaDesc;
            this.MetaKeywords = metaKW;
            this.UrlTag = urlTag;
        }

        public static CategoryTranslation.CreateTranslatonResult CreateCategoryTranslaton(int catId, int langId, string title, string desc, string metaDesc, string metaKW, string urlTag)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
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
                    cmd.Parameters.Add(new SqlParameter("@MetaDesc", SqlDbType.NVarChar, 1000) { Value = metaDesc });
                    cmd.Parameters.Add(new SqlParameter("@MetaKeywords", SqlDbType.NVarChar, 1000) { Value = metaKW });
                    cmd.Parameters.Add(new SqlParameter("@UrlTag", SqlDbType.NVarChar, 250) { Value = urlTag });

                    SqlParameter parRetValue = new SqlParameter("@ReturnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return (CategoryTranslation.CreateTranslatonResult)Convert.ToInt32(parRetValue.Value);
                }
            }
        }

        public static CategoryTranslation.UpdateTranslatonResult UpdateCategoryTranslaton(int catId, int langId, string title, string desc, string metaDesc, string metaKW, string urlTag)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
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
                    cmd.Parameters.Add(new SqlParameter("@MetaDesc", SqlDbType.NVarChar, 1000) { Value = metaDesc });
                    cmd.Parameters.Add(new SqlParameter("@MetaKeywords", SqlDbType.NVarChar, 1000) { Value = metaKW });
                    cmd.Parameters.Add(new SqlParameter("@UrlTag", SqlDbType.NVarChar, 250) { Value = urlTag });

                    SqlParameter parRetValue = new SqlParameter("@ReturnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return (CategoryTranslation.UpdateTranslatonResult)Convert.ToInt32(parRetValue.Value);
                }
            }
        }

        public static void DeleteCategoryTranslaton(int catId, int langId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
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

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
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
                                rdr["Description"].ToString(),
                                rdr["MetaDesc"].ToString(),
                                rdr["MetaKeywords"].ToString(),
                                rdr["UrlTag"].ToString()
                                ));
                        }

                        rdr.Close();
                    }
                    con.Close();
                }
            }
            return lst;
        }

        public static CategoryTranslation GetCategoryTranslation(int catId, int langId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetCategoryTranslation"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = catId });
                    cmd.Parameters.Add(new SqlParameter("@LangId", SqlDbType.Int, 4) { Value = langId });

                    SqlParameter parTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parTitle);
                    SqlParameter parDesc = new SqlParameter("@Description", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parDesc);
                    SqlParameter parMetaDesc = new SqlParameter("@MetaDesc", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parMetaDesc);
                    SqlParameter parMetaKW = new SqlParameter("@MetaKeywords", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parMetaKW);
                    SqlParameter parUrlTag = new SqlParameter("@UrlTag", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parUrlTag);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return new CategoryTranslation(catId, langId, parTitle.Value.ToString(), parDesc.Value.ToString(),
                        parMetaDesc.Value != DBNull.Value ? parMetaDesc.Value.ToString() : null, parMetaKW.Value != DBNull.Value ? parMetaKW.Value.ToString() : null, parUrlTag.Value.ToString());
                }
            }

        }


        public enum CreateTranslatonResult
        {
            Success = 1,
            TagExistsForOtherCategory = 2,
            TranslationForLangExists = 3
        }

        public enum UpdateTranslatonResult
        {
            Success = 1,
            TagExistsForOtherCategory = 2
        }
    }
}