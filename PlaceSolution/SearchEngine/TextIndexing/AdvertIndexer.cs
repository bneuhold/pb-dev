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
    class AdvertIndexer
    {
        public static void IndexAllAdverts(string indexPath, string connectionString, Version version = Version.LUCENE_30)
        {
            try
            {
                // Initialize SQL connection and fetch all adverts.
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT Id, AccommodationType, Title, Description, Country, Region, Island, LanguageId, City FROM dbo.Advert", conn);
                SqlDataReader reader = command.ExecuteReader();

                // Initialize Lucene.Net index writers.
                AnalyzerFactory afactory = new AnalyzerFactory(Version.LUCENE_30);
                var writers = afactory.CreateWriters(indexPath);

                // Initiate indexing options for individual fields.
                int fieldCount = 8;
                String[] fieldName = {"Id", "AccommodationType", "Title", "Description", 
                                      "Country", "Region", "Island", "City"};

                Field.Store[] fieldStore = { Field.Store.YES, Field.Store.NO, Field.Store.NO, Field.Store.NO, 
                                             Field.Store.NO, Field.Store.NO, Field.Store.NO, Field.Store.NO };

                Field.Index[] fieldIndex = { Field.Index.NO, Field.Index.ANALYZED, Field.Index.ANALYZED, Field.Index.ANALYZED, 
                                             Field.Index.ANALYZED, Field.Index.ANALYZED, Field.Index.ANALYZED, Field.Index.ANALYZED };

                float[] fieldBoost = { 0, 2, 1, 1, 2.5F, 3, 3.5F, 4 };

                // Index adverts one by one..
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

                    if (writers.ContainsKey(languageId))
                    {
                        writers[languageId].AddDocument(advertDoc);
                    }
                }

                // Cleanup.
                foreach (var w in writers.Values)
                {
                    w.Optimize();
                    w.Dispose();
                }

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Done!");
            Console.Read();
        }
    }
}
