using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;

namespace PB_Parser
{
    class Parser
    {
        private Config _config;
        private Table _table;

        public Parser(Config config, Table.TableRow headerRow)
        {
            _table = new Table(headerRow.columns.Count)
                         {Header = headerRow};
            _config = config;
        }

        public Table Parse()
        {
            //var rss = XDocument.Load(_config.Url);

            XDocument rss;
            using (WebClient webClient = new WebClient())
            {
                using (Stream stream = webClient.OpenRead(_config.Url))
                {
                    rss = XDocument.Load(stream);
                }
            }

            if (rss == null)
            {
                throw new Exception("Faild to read rss from url");
            }

            if (string.IsNullOrEmpty(_config.Lang))
            {
                var langElem = rss.Descendants("channel").FirstOrDefault().Element("language");

                if (langElem != null)
                    _config.Lang = langElem.Value;
            }

            var items = rss.Descendants("item");

            foreach (var item in items)
            {
                var row = _table.AddRow();

                foreach (var mapping in _config.Mappings)
                {
                    if (_table.GetColumnId(mapping.To) == -1)
                        throw new Exception("Mapping " + mapping.To + " ne postoji medju kolonama.");

                    row[_table.GetColumnId(mapping.To)] = mapping.IsInHeader
                                                              ? rss.Descendants("channel").FirstOrDefault().Element(mapping.From).Value
                                                              : item.Element(mapping.From).Value;
                }

                foreach (var expression in _config.Expressions)
                {
                    var groups = expression.Regex.Match(item.Element(expression.Item).Value).Groups;

                    for ( int i = 1 ; i <groups.Count ; ++i)
                    {
                        int columnId = _table.GetColumnId(expression.Regex.GroupNameFromNumber(i));
                        if (columnId == -1)
                        {
                            throw new Exception("Expression " + expression.Regex.GroupNameFromNumber(i) +
                                                " ne postoji medju kolonama.");
                        }
                        row[columnId] = groups[i].Value;
                    }
                }

                foreach (var duplication in _config.Duplications)
                {
                    var fromColId = _table.GetColumnId(duplication.From);
                    var toColId = _table.GetColumnId(duplication.To);

                    if (fromColId == -1)
                        throw new Exception("Duplication " + duplication.From + " ne postoji medju kolonama.");
                    if (toColId == -1)
                        throw new Exception("Duplication " + duplication.To + " ne postoji medju kolonama.");

                    row[toColId] = row[fromColId];
                }

                row[_table.GetColumnId(_config.LanguageColumn)] = _config.Lang;
            }

            return _table;
        }
    }
}
