using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Version = Lucene.Net.Util.Version;

namespace CroatianAnalyzer
{
    public class CroAnalyzer : Analyzer
    {
        private static readonly String[] CROATIAN_STOP_WORDS = 
        {
            "biti", "jesam", "budem", "sam", "jesi", "budeš", "si", "jesmo", "budemo", "smo", "jeste", "budete", "ste", "jesu", 
            "budu", "su", "bih", "bijah", "bjeh", "bijaše", "bi", "bje", "bješe", "bijasmo", "bismo", "bjesmo", "bijaste", "biste", 
            "bjeste", "bijahu", "biste", "bjeste", "bijahu", "bi", "biše", "bjehu", "bješe", "bio", "bili", "budimo", "budite", "bila", 
            "bilo", "bile", "ću", "ćeš", "će", "ćemo", "ćete", "želim", "želiš", "želi", "želimo", "želite", "žele", "moram", "moraš", 
            "mora", "moramo", "morate", "moraju", "trebam", "trebaš", "treba", "trebamo", "trebate", "trebaju", "mogu", "možeš", "može", "možemo", "možete"
        };

        private Version matchVersion;
        private CroatianStemmer stemmer = null;
        //private ISet<String> stopSet = null;

        public CroAnalyzer(Version version) {
            this.matchVersion = version;
            this.stemmer = new CroatianStemmer();
            //StopFilter.MakeStopSet(CroAnalyzer.CROATIAN_STOP_WORDS);
        }

        public CroAnalyzer(Version version, CroatianStemmer stemmer) {
            this.matchVersion = version;
            this.stemmer = stemmer;
            //StopFilter.MakeStopSet(CroAnalyzer.CROATIAN_STOP_WORDS);
        }

        public override TokenStream TokenStream(String fieldName, TextReader reader) {
            Tokenizer source = new StandardTokenizer(matchVersion, reader);
            TokenStream sink = new StandardFilter(source);
            sink = new LowerCaseFilter(sink);
            //sink = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(matchVersion), sink, stopSet);
            sink = new CroatianStemFilter(sink, stemmer);
            return sink;
        }
    }
}
