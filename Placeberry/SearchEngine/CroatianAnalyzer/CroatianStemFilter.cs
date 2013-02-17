using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace CroatianAnalyzer
{
    class CroatianStemFilter : TokenFilter
    {
        private CroatianStemmer stemmer = null;

        private ITermAttribute termAtt;

        public CroatianStemFilter(TokenStream _in)
            : base(_in)
        {
            this.stemmer = new CroatianStemmer();
            termAtt = AddAttribute<ITermAttribute>();
        }

        public CroatianStemFilter(TokenStream _in, CroatianStemmer stemmer) 
            : base(_in)
        { 
            this.stemmer = stemmer;
            termAtt = AddAttribute<ITermAttribute>();
        }

        public override bool IncrementToken()
        {
            if (input.IncrementToken())
            {
                String term = termAtt.Term;
                String s = stemmer.Stem(term);
                if ((s != null) && !s.Equals(term))
                    termAtt.SetTermBuffer(s);

                //Console.WriteLine(term + " " + s);
                return true;
            }

            return false;
        }
    }
}
