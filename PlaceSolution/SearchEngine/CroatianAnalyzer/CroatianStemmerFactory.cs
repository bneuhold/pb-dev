using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CroatianAnalyzer
{
    public class CroatianStemmerFactory
    {

        static public CroatianStemmer CreateStemmer() {
            List<Regex> rules = ReadRules(@"C:\Users\matija\Documents\Visual Studio 2012\Projects\pb2\PlaceSolution\SearchEngine\CroatianAnalyzer\rules.txt");
            List<String> transFrom = new List<string>();
            List<String> transTo = new List<String>();
            ReadTransformations(@"C:\Users\matija\Documents\Visual Studio 2012\Projects\pb2\PlaceSolution\SearchEngine\CroatianAnalyzer\transformations.txt", transFrom, transTo);
            return new CroatianStemmer(rules, transFrom, transTo);
        }

        static private List<Regex> ReadRules(string inputFile)
        {
            string[] rawRules = System.IO.File.ReadAllLines(inputFile, System.Text.Encoding.UTF8);
            List<Regex> rules = new List<Regex>();

            foreach (string r in rawRules)
            {
                if (r[0] != '#')
                {
                    string[] splitRule = r.Split(' ');
                    rules.Add(new Regex(@"^(" + splitRule[0] + @")(" + splitRule[1] + @")$"));
                }
            }

            return rules;
        }

        static private void ReadTransformations(string inputFile, List<String> transFrom, List<String> transTo)
        {
            string[] rawTransformations = System.IO.File.ReadAllLines(inputFile, System.Text.Encoding.UTF8);
            
            foreach (string t in rawTransformations)
            {
                string[] splitTrans = t.Split('\t');
                transFrom.Add(splitTrans[0]);
                transTo.Add(splitTrans[1]);
            }

        }

    }
}
