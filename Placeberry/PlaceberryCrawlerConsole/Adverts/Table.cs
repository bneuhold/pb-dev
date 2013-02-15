using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaceberryCrawlerConsole
{
    public class Table
    {
        public TableRow Header { get; set; }

        public List<TableRow> Rows { get; private set; }

        private int _nColumns;

        public Table(int nColumns)
        {
            Header = new TableRow(nColumns);

            Rows= new List<TableRow>();

            _nColumns = nColumns;
        }

        public TableRow AddRow()
        {
            var tableRow = new TableRow(_nColumns);
            Rows.Add(tableRow);
            return tableRow;
        }

        public int GetColumnId(string header)
        {
            for ( int i = 0 ; i <_nColumns ; ++i)
            {
                if (Header[i] == header || Header[i].Substring(2) == header)
                    return i;
            }
            return -1;
        }

        public class TableRow
        {
            public List<string> columns;

            public TableRow(int n)
            {
                columns = new List<string>(n);
                for (int i = 0 ; i < n ; ++i)
                    columns.Add("");
            }

            public string this [int key]
            {
                get { return columns[key]; }
                set { columns[key] = value; }
            }
        }
    }
}
