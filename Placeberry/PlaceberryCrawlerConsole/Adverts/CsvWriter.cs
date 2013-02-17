using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PlaceberryCrawlerConsole
{
    class CsvWriter
    {
        private string _filename;
        public CsvWriter(string filename)
        {
            _filename = filename;
        }

        public void Write(Table table)
        {
            using (var sw = new StreamWriter(_filename))
            {
                var rows = new List<Table.TableRow> {table.Header};
                rows.AddRange(table.Rows);

                foreach (var row in rows)
                {
                    var first = true;

                    foreach (var column in row.columns)
                    {
                        if (!first)
                            sw.Write(",");
                        else first = false;

                        sw.Write(PrepareCsvCol(column));
                    }

                    sw.Write(Environment.NewLine);
                }
            }
        }

        private string PrepareCsvCol(string col)
        {
            if (col.Contains(",") || col.Contains("\n"))
                return "\"" + col.Replace("\"", "'") + "\"";

            return col;
        }
    }
}
