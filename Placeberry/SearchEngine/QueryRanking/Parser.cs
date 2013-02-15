﻿using System;
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
                SqlConnection conn = new SqlConnection("user id=mrihr_User;" +
                                            "password=mriusr123;" +
                                            "server=mssql4.mojsite.com,1555;" +
                                            "database=mrihr_Placeberry_dev");
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
            if (!langIds.Contains(languageId))
            {
                languageId = 1;
                //rest = "";
                //return null;
            }

            List<ParseToken> uids = new List<ParseToken>();
            StringBuilder restSB = new StringBuilder();

            query.Replace('\'', '\"');
            query.Replace("''", "\"");
            query.Replace("„", "\"");
            query.Replace("“", "\"");
            query.Replace("(", "\"");
            query.Replace(")", "\"");

            query = query.ToLower().Trim();

            while (query.IndexOf('\"') != -1)
            {
                int p1 = query.IndexOf('\"');
                int p2 = query.IndexOf('\"', p1 + 1);
                if (p2 == -1)
                {
                    query.Replace("\"", "");
                }

                String parara = query.Substring(p1 + 1, p2 - p1 - 1);
                int found = TryToMatch(parara, languageId);
                if (found != -1)
                {
                    uids.Add(new ParseToken(found, parara, allWords[found]));
                    query = query.Substring(0, p1) + query.Substring(p2 + 1);
                }
                else
                {
                    query.Replace("\"", "");
                }
            }

            query.Replace("!", "");
            query.Replace("&", "");
            query.Replace("*", "");
            query.Replace("/", "");
            query.Replace("\\", "");
            query.Replace("+", "");
            query.Replace("#", "");
            query.Replace("\t", " ");

            int pos1 = 0;
            while (pos1 < query.Length)
            {
                int pos2 = query.Length;
                int found = -1;
                while (true)
                {
                    String pods = query.Substring(pos1, pos2 - pos1);
                    //Console.WriteLine(pods);
                    found = TryToMatch(pods, languageId);
                    if (found != -1)
                    {
                        uids.Add(new ParseToken(found, pods, allWords[found]));
                        pos1 = pos2 + 1;
                        break;
                    }

                    int index = pods.LastIndexOf(' ');
                    if (index == -1) break;
                    pos2 = pos1 + index;
                }

                if (found == -1)
                {
                    restSB.Append(query.Substring(pos1, pos2 - pos1)).Append(" ");
                    int index2 = query.IndexOf(' ', pos1);
                    if (index2 == -1) break;
                    pos1 = index2 + 1;
                }
            }

            //string[] sq = query.Split();

            //for (int i = 0; i < sq.Length; ++i)
            //{
            //    StringBuilder sb = new StringBuilder();

            //    int found = -1;
            //    for (int j = i; j < sq.Length; ++j)
            //    {
            //        sb.Append(sq[j]);

            //        found = TryToMatch(sb.ToString());
            //        Console.WriteLine(sb.ToString() + " " + found);
            //        if (found != -1)
            //        {
            //            i = j;
            //            uids.Add(found);
            //            break;
            //        }
            //        sb.Append(" ");
            //    }

            //    if (found == -1)
            //    {
            //        rest.Append(sq[i]).Append(" ");
            //    }
            //}

            rest = restSB.ToString().Trim();
            return uids;
        }

        public int TryToMatch(string subquery, int languageId)
        {
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
