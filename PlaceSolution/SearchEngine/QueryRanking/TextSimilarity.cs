using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Index;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;
using CroatianAnalyzer;
using TextIndexing;

namespace QueryRanking
{
    public class TextSimilarity
    {
        public static String indexPath;
        
        // Map (languageId -> stuff to index that language).
        private Dictionary<int, SearchPack> langSearch;

        private Dictionary<Int32, Double> searchResults;

        public TextSimilarity()
        {
            //indexPath = @"C:\Users\matija\Documents\Visual Studio 2012\Projects\Placeberry\PlaceSolution\SearchEngine\LuceneIndex";
        }

        public void Init()
        {
            Version version = Version.LUCENE_30;
            AnalyzerFactory afactory = new AnalyzerFactory(version);
            langSearch = afactory.CreateSearchPacks(indexPath);
        }

        public void ProcessQuery(String rawQuery, int languageId)
        {
            if (langSearch == null || !langSearch.ContainsKey(languageId)) {
                throw new Exception("Language not supported.");
            }

            QueryParser queryParser = langSearch[languageId].queryParser;
            Searcher searcher = langSearch[languageId].searcher;

            searchResults = new Dictionary<int, double>();

            if("".Equals(rawQuery.Trim())) return;

            Query query = queryParser.Parse(rawQuery);
            TopDocs top = searcher.Search(query, 1000);
            
            foreach (var hit in top.ScoreDocs)
            {
                var doc = searcher.Doc(hit.Doc);
                Int32 id = Int32.Parse(doc.Get("Id"));
                searchResults[id] = hit.Score;
            }
        }

        public Double GetScore(int uid)
        {
            if (searchResults == null) throw new Exception("You must first process a query in order to query the results.");

            if (searchResults.ContainsKey(uid))
            {
                return Math.Max(0, searchResults[uid]);
            }

            return 0;
        }
    }
}
