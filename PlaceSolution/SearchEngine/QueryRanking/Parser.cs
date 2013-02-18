using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using FuzzyStrings;

namespace QueryRanking
{
    public class Word
    {
        public int termId;
        public string title;
        public Regex regex;
        public int languageId;
        public int termTypeId;
        public string parseTag;
        public int geoplaceId;

        public Word(int id, string t, Regex r, int lid, int tid, string tag, int gid)
        {
            termId = id;
            title = t;
            regex = r;
            languageId = lid;
            termTypeId = tid;
            parseTag = tag;
            geoplaceId = gid;
        }
    }

    public class ParseToken
    {
        public int termId;
        public String text;
        public Word word;

        public ParseToken(int id, string t, Word w)
        {
            termId = id;
            text = t;
            word = w;
        }
    }

    class Parser
    {
        // Map from (languageId -> (termId -> Word)).
        private Dictionary<int, Dictionary<int, Word>> wordsForLang;

        // Map from (termId -> Word).
        private Dictionary<int, Word> allWords;

        // Set of ids of supported languages.
        private HashSet<int> langIds;

        public Parser()
        {
            allWords = new Dictionary<int, Word>();
            wordsForLang = new Dictionary<int, Dictionary<int, Word>>();

            int[] sli = { 1, 2 };
            langIds = new HashSet<int>(sli);
            
            foreach (int l in sli) {
                wordsForLang.Add(l, new Dictionary<int,Word>());
            }
        }

        public Dictionary<int, Word> GetAllWords() { return allWords; }

        public void Init()
        {
            try
            {
                SqlConnection conn = Utility.GetDefaultConnection();
                conn.Open();

                string strcmd = @"SELECT * From SearchTerm WHERE LanguageId IN (1, 2) AND SearchTermTypeId IN (1, 2, 3, 4, 5, 6, 7, 12, 13, 14, 15)";
                SqlCommand sqlcmd = new SqlCommand(strcmd, conn);
                SqlDataReader reader = sqlcmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Int32.Parse(reader["Id"].ToString());
                    string title = reader["Title"].ToString();
                    Regex regex = new Regex(reader["Regex"].ToString());
                    int lid = Int32.Parse(reader["LanguageId"].ToString());
                    int gid = reader["SearchGeoplaceId"] == DBNull.Value ? -1 : Int32.Parse(reader["SearchGeoplaceId"].ToString());
                    int tid = Int32.Parse(reader["SearchTermTypeId"].ToString());
                    string tag = reader["ParseTag"].ToString();

                    Word word = new Word(id, title, regex, lid, tid, tag, gid);
                    wordsForLang[lid].Add(id, word);
                    allWords.Add(id, word);
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<ParseToken> Parse(String query, int languageId, out string rest)
        {
            if (!langIds.Contains(languageId)) languageId = 1;

            List<ParseToken> uids = new List<ParseToken>();
            StringBuilder restSB = new StringBuilder();

            foreach (char c in "\'„“()") query.Replace(c, '\"');
            query.Replace("''", "\"");
            query.Replace("\"\"", "\"");

            //string[] phraseWords = query.Split('\"');

            query = query.ToLower().Trim();

            foreach (char c in "!@#%^&*_~\"\t") query.Replace(c, ' ');

            char[] kita = {' '};
            string[] words = query.Split(kita, StringSplitOptions.RemoveEmptyEntries);

            int n = words.Length;

            for (int i = 0; i < n; ++i)
            {
                StringBuilder sb = new StringBuilder();
                int res_id = -1, res_len = -1;
                string res_str = "";

                for (int j = 0; i + j < n && j < 6; ++j)
                {
                    sb.Append(words[i+j]).Append(" ");
                    int tmp_res = TryToMatch(sb.ToString().Trim(), languageId);

                    if (tmp_res != -1)
                    {
                        res_len = j;
                        res_id = tmp_res;
                        res_str = sb.ToString().Trim();
                    }
                }

                if (res_id != -1)
                {
                    uids.Add(new ParseToken(res_id, res_str, allWords[res_id]));
                    i += res_len;
                }
                else
                {
                    restSB.Append(words[i]).Append(" ");
                }
            }

            rest = restSB.ToString().Trim();
            return uids;
        }

        public int TryToMatch(string subquery, int languageId)
        {
            Console.WriteLine("+" + subquery + "+");
            int regex_val = -1, regex_ind = -1;

            Dictionary<int, Word> words = wordsForLang[languageId];

            foreach (Word w in words.Values)
            {
                if (w.regex != null && w.regex.IsMatch(subquery))
                {
                    int val = FuzzyStrings.LevenshteinDistanceExtensions.LevenshteinDistance(subquery, w.title, false);

                    if (regex_ind == -1 || val < regex_val)
                    {
                        regex_val = val;
                        regex_ind = w.termId;
                    }
                }
            }

            return regex_ind;
        }
    }
}
