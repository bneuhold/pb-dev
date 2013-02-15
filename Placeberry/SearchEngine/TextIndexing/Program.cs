using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;
using CroatianAnalyzer;

namespace TextIndexing
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // SQL.
                SqlConnection conn = new SqlConnection("user id=mrihr_User;" +
                                            "password=mriusr123;" +
                                            "server=mssql4.mojsite.com,1555;" +
                                            "database=mrihr_Placeberry_dev"); 
                conn.Open();
                SqlCommand command = new SqlCommand(
                    "SELECT Id, AccommodationType, Title, Description, Country, Region, Island, LanguageId, City FROM dbo.Advert", conn);
                SqlDataReader reader = command.ExecuteReader();

                // Lucene HR.
                Directory directory_hr = FSDirectory.Open(new DirectoryInfo(@"C:\Users\matija\Documents\Visual Studio 2012\Projects\Placeberry\PlaceSolution\Placeberry\LuceneIndex\hr"));
                Analyzer analyzer_hr = new CroAnalyzer(Version.LUCENE_30);
                IndexWriter writer_hr = new IndexWriter(directory_hr, analyzer_hr, true, IndexWriter.MaxFieldLength.LIMITED);
                
                // Lucene EN.
                Directory directory_en = FSDirectory.Open(new DirectoryInfo(@"C:\Users\matija\Documents\Visual Studio 2012\Projects\Placeberry\PlaceSolution\Placeberry\LuceneIndex\en"));
                Analyzer analyzer_en = new StandardAnalyzer(Version.LUCENE_30);
                IndexWriter writer_en = new IndexWriter(directory_en, analyzer_en, true, IndexWriter.MaxFieldLength.LIMITED);
                
                // Field info.
                int fieldCount = 8;
                String[] fieldName = {"Id", "AccommodationType", "Title", "Description", 
                                      "Country", "Region", "Island", "City"};
                Field.Store[] fieldStore = { Field.Store.YES, Field.Store.NO, Field.Store.NO, Field.Store.NO, 
                                             Field.Store.NO, Field.Store.NO, Field.Store.NO, Field.Store.NO };
                Field.Index[] fieldIndex = { Field.Index.NO, Field.Index.ANALYZED, Field.Index.ANALYZED, Field.Index.ANALYZED, 
                                             Field.Index.ANALYZED, Field.Index.ANALYZED, Field.Index.ANALYZED, Field.Index.ANALYZED };
                float[] fieldBoost = { 0, 2, 1, 1, 2.5F, 3, 3.5F, 4 };

                // Adding stuff to index..
                int count = 0;
                while (reader.Read())
                {
                    ++count;
                    if (count % 100 == 0) Console.WriteLine(count);

                    Document advertDoc = new Document();
                    
                    for (int i = 0; i < fieldCount; ++i)
                    {
                        Field field = new Field(fieldName[i], reader[fieldName[i]].ToString(), fieldStore[i], fieldIndex[i]);
                        field.Boost = fieldBoost[i];
                        advertDoc.Add(field);
                    }

                    int languageId = ("".Equals(reader["LanguageId"].ToString()) ? -1 : Int32.Parse(reader["LanguageId"].ToString()));

                    switch (languageId) {
                        case 1: writer_hr.AddDocument(advertDoc);
                            break;
                        case 2: writer_en.AddDocument(advertDoc);
                            break;
                    }
                }

                writer_hr.Optimize();
                writer_hr.Dispose();

                writer_en.Optimize();
                writer_en.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.Read();

        }
    }
}
