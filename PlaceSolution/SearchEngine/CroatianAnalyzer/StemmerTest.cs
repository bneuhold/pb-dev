using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;
using System.IO;
using Lucene.Net.Analysis.Tokenattributes;

namespace CroatianAnalyzer
{
    class StemmerTest
    {
        static void Main(string[] args)
        {
            CroatianStemmer stemmer = CroatianStemmerFactory.CreateStemmer();

            Analyzer a = new CroAnalyzer(Version.LUCENE_29, stemmer);

            TextReader reader = new StringReader("Ideja koja je navodno proizašla iz svlačionice Dinama, kako su igrači spremni s milijun kuna nagraditi jednog sretnog navijača u zamjenu za pun stadion protiv PSG-a, zgrozila je hrvatski nogometni puk. Zgrozio se i Mario Stanić koji je jasno rekao što misli o novom potezu Modre uprave.");

            TokenStream ts = a.TokenStream("text", reader);
            TermAttribute ta = (TermAttribute)ts.GetAttribute(typeof(TermAttribute));

            while (ts.IncrementToken())
            {
                Console.WriteLine(ta.Term());
            }
            
            while (true)
            {
                String line = Console.ReadLine().Trim();
                Console.WriteLine(stemmer.Stem(line));
            }

            Console.Read();
        }
    }
}
