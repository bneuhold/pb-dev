using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    public class AdvertManager
    {
        private int _maxFieldLen;

        public Table.TableRow CurrentAdvert { get; private set; }
        private Dictionary<string, int> _dicColNamesMap;   // za mapiranje imena kolone na broj kolone

        public int MaxFieldLen { get { return _maxFieldLen; } }

        public Table Adverts { get; private set; }


        public AdvertManager(string configPath)
        {
            XDocument xdoc = XDocument.Load(configPath);

            var headers = (from xEl in xdoc.Descendants("column")
                           select new { xEl.Value }).ToList();

            Table.TableRow headerRow = new Table.TableRow(headers.Count);
            _dicColNamesMap = new Dictionary<string, int>(headers.Count);

            for (var i = 0; i < headers.Count; ++i)
            {
                headerRow[i] = headers[i].Value;
                _dicColNamesMap.Add(headers[i].Value, i);
            }

            Adverts = new Table(headerRow.columns.Count) { Header = headerRow };
            CurrentAdvert = new Table.TableRow(headerRow.columns.Count);

            if (xdoc.Element("MaxFieldLen") == null || !Int32.TryParse(xdoc.Element("MaxFieldLen").Value, out _maxFieldLen))
            {
                _maxFieldLen = 500;
            }
        }

        public void InsertCurrentAdvert()
        {
            Table.TableRow row = Adverts.AddRow();

            for (int i = 0; i < row.columns.Count; ++i)
            {
                row.columns[i] = CurrentAdvert.columns[i];
            }
        }

        public void AddFieldValue(string fieldName, string value)
        {
            if (String.IsNullOrEmpty(fieldName) || !_dicColNamesMap.ContainsKey(fieldName))
            {
                throw new Exception("Field name wrong formatted!");
            }

            if(value == null)
            {
                throw new Exception("Vale can't be null!");
            }

            value = value.Trim();

            CurrentAdvert[_dicColNamesMap[fieldName]] = value;
        }
    }
}
