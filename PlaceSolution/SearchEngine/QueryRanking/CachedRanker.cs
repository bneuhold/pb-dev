using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryRanking
{
    public class CachedRanker
    {
        public QueryRanker ranker;
        private LRUCache<String, SearchResult> cache;

        private const int cacheCapacity = 1000;
        private const int cacheNumAdverts = 100;
        private const int cacheMaxAdverts = 1600;

        private static volatile CachedRanker instance = new CachedRanker();
        private static object sync = new Object();

        public static CachedRanker Instance
        {
            get
            {
                return instance;
            }
        }

        private CachedRanker()
        {
            ranker = new QueryRanker();
            cache = new LRUCache<string, SearchResult>(cacheCapacity);
        }

        public List<Int32> Search(String query, int languageId, int start, int count)
        {
            lock (sync)
            {
                var key = Key(query, languageId);

                GroupedParseResult parseRes;
                List<AdvertScore> searchRes;
                List<Int32> advertIds;
                int size, maxsize;

                SearchResult res;

                if (!cache.Get(key, out res))
                {
                    parseRes = ranker.Parse(query, languageId);
                    searchRes = ranker.SearchParsed(parseRes, languageId);
                    advertIds = new List<int>();

                    maxsize = Math.Min(searchRes.Count, cacheMaxAdverts);
                    size = Math.Min(maxsize, cacheNumAdverts);

                    while (start + count > size && size < maxsize)
                    {
                        size = Math.Min(2 * size, maxsize);
                    }

                    for (int i = 0; i < size; ++i)
                    {
                        advertIds.Add(searchRes[i].advertId);
                    }

                    cache.Add(key, new SearchResult(query, languageId, advertIds, parseRes, searchRes.Count));
                }
                else
                {
                    parseRes = res.parseRes;
                    advertIds = res.advertIds;
                    size = advertIds.Count;
                    maxsize = Math.Min(cacheMaxAdverts, res.numRes);

                    if (size != maxsize && start + count > size)
                    {
                        searchRes = ranker.SearchParsed(parseRes, languageId);
                        maxsize = Math.Min(searchRes.Count, cacheMaxAdverts);

                        while (start + count > size && size < maxsize)
                        {
                            size = Math.Min(2 * size, maxsize);
                        }

                        for (int i = advertIds.Count; i < size; ++i)
                        {
                            advertIds.Add(searchRes[i].advertId);
                        }
                    }
                }

                if (start > size) return new List<int>();
                return advertIds.GetRange(start, Math.Min(size - start, count));
            }
        }

        public int SearchCount(String query, int languageId)
        {
            lock (sync)
            {
                var key = Key(query, languageId);
                SearchResult res;

                if (!cache.Get(key, out res))
                {
                    GroupedParseResult parseRes = ranker.Parse(query, languageId); ;
                    List<AdvertScore> searchRes = ranker.SearchParsed(parseRes, languageId); ;
                    List<Int32> advertIds = new List<int>();

                    for (int i = 0; i < Math.Min(searchRes.Count, cacheNumAdverts); ++i)
                    {
                        advertIds.Add(searchRes[i].advertId);
                    }

                    cache.Add(key, new SearchResult(query, languageId, advertIds, parseRes, searchRes.Count));
                    return searchRes.Count;
                }

                return res.numRes;
            }
        }

        private String Key(String query, int languageId)
        {
            return query + languageId.ToString();
        }
    }

    class SearchResult
    {
        public String query;
        public Int32 languageId;
        public List<Int32> advertIds;
        public GroupedParseResult parseRes;
        public int numRes;

        public SearchResult(String q, Int32 l, List<Int32> i, GroupedParseResult p, int n)
        {
            query = q;
            languageId = l;
            advertIds = i;
            parseRes = p;
            numRes = n;
        }
    }
}

