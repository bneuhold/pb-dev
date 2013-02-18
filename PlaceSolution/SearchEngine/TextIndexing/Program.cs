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
            string connString = @"user id=mrihr_User; password=mriusr123; server=mssql4.mojsite.com,1555; database=mrihr_Placeberry_dev";
            string indexPath = @"C:\Users\matija\Documents\Visual Studio 2012\Projects\Placeberry\PlaceSolution\Placeberry\LuceneIndex";

            AdvertIndexer.IndexAllAdverts(indexPath, connString, Version.LUCENE_30);
        }
    }
}
