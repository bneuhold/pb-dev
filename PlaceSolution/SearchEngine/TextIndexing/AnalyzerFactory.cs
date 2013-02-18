using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;/*
using Lucene.Net.Analysis.De;
using Lucene.Net.Analysis.Cz;*/
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;
using CroatianAnalyzer;

namespace TextIndexing
{
    public class AnalyzerFactory
    {
        private HashSet<int> langs;
        private Dictionary<int, string> langSuffix;
        private Dictionary<int, Analyzer> langAnalyzer;
        private Version version;

        public AnalyzerFactory(Version version = Version.LUCENE_30)
        {
            this.version = version;
            InitLanguages(version);
        }

        private void InitLanguages(Version version)
        {
            langs = new HashSet<int>(new int[] { 1, 2 });
            langSuffix = new Dictionary<int, string>();
            langAnalyzer = new Dictionary<int, Analyzer>();

            // Croatian
            langSuffix[1] = @"hr";
            langAnalyzer[1] = new CroAnalyzer(version);

            // English
            langSuffix[2] = @"en";
            langAnalyzer[2] = new StandardAnalyzer(version);
            /*
            // German
            langSuffix[3] = @"de";
            langAnalyzer[3] = new GermanAnalyzer(version);

            // Italian
            //langSuffix[4] = @"it";
            //langAnalyzer[4] = ???;

            // Czech
            langSuffix[5] = @"cz";
            langAnalyzer[5] = new CzechAnalyzer(version);
             * */
        }

        public Dictionary<int, IndexWriter> CreateWriters(string indexPath)
        {
            Dictionary<int, IndexWriter> writers = new Dictionary<int, IndexWriter>();

            indexPath.TrimEnd(new char[] {'\\'});

            foreach (var l in langs)
            {
                Directory directory = FSDirectory.Open(new DirectoryInfo(indexPath + @"\" + langSuffix[l]));
                Analyzer analyzer = langAnalyzer[l];
                writers[l] = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);
            }

            return writers;
        }

        public Dictionary<int, SearchPack> CreateSearchPacks(string indexPath)
        {
            Dictionary<int, SearchPack> searchers = new Dictionary<int, SearchPack>();

            indexPath.TrimEnd(new char[] { '\\' });

            foreach (var l in langs)
            {
                Directory directory = FSDirectory.Open(new DirectoryInfo(indexPath + @"\" + langSuffix[l]));
                IndexReader reader = IndexReader.Open(directory, true);
                Analyzer analyzer = langAnalyzer[l];
                Searcher searcher = new IndexSearcher(reader);
                QueryParser queryParser = new QueryParser(version, "Description", analyzer);

                searchers[l] = new SearchPack(queryParser, searcher);
            }

            return searchers;
        }
    }

    public class SearchPack {
        public QueryParser queryParser;
        public Searcher searcher;

        public SearchPack(QueryParser queryParser, Searcher searcher)
        {
            this.queryParser = queryParser;
            this.searcher = searcher;
        }
    }
}
