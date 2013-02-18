using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace PB_Parser
{
    class Init
    {
        public Init(string configPath)
        {
            var doc = XDocument.Load(configPath);

            var headers = (from xEl in doc.Descendants("column")
                           select new { xEl.Value }).ToList();

            var headerRow = new Table.TableRow(headers.Count);

            for (var i = 0; i < headers.Count; ++i)
            {
                headerRow[i] = headers[i].Value;
            }

            foreach (var config in from xEl in doc.Descendants("rssFeed")
                                   select new Config()
                                              {
                                                  Expressions = (from exp in xEl.Descendants("expression")
                                                                 select new Config.Expression()
                                                                            {
                                                                                Item = exp.Attribute("item").Value,
                                                                                Regex = new Regex(exp.Attribute("regex").Value)
                                                                            }).ToList(),
                                                  Duplications = (from dup in xEl.Descendants("duplicate")
                                                                  select new Config.Duplication()
                                                                             {
                                                                                 From = dup.Attribute("from").Value,
                                                                                 To = dup.Attribute("to").Value
                                                                             }).ToList(),
                                                  Mappings = (from map in xEl.Descendants("map")
                                                              select new Config.Mapping()
                                                                         {
                                                                             From = map.Attribute("from").Value,
                                                                             To = map.Attribute("to").Value,
                                                                             IsInHeader = (map.Attribute("header") == null || map.Attribute("header").Value=="false") ? false : true
                                                                         }).ToList(),
                                                LanguageColumn = xEl.Element("language").Attribute("column").Value,
                                                Url = xEl.Attribute("url").Value,
                                                Lang = xEl.Attribute("lang") != null ? xEl.Attribute("lang").Value : ""
            
                                              })
            {
                new CsvWriter("D:\\CSV_work\\CSV_create\\" + GetFileNameWithLang(config.Url, config.Lang)).Write(new Parser(config, headerRow).Parse());
            }

        }

        private static string GetFileName(string url)
        {
            if (!url.Contains("://"))
                url = "http://" + url;

            return new Uri(url).Host.Replace(".", "-") + "-" + DateTime.Today.ToString("yyy-MM-dd") + ".csv";
        }

        private static string GetFileNameWithLang(string url, string lang)
        {
            if (!url.Contains("://"))
                url = "http://" + url;

            return new Uri(url).Host.Replace(".", "-") + "_" + lang + "-" + DateTime.Today.ToString("yyy-MM-dd") + ".csv";
        }
    }
}
