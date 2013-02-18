using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    class PageComparer : IEqualityComparer<XElement>
    {
        public bool Equals(XElement x, XElement y)
        {
            return (int)x == (int)y;
        }

        public int GetHashCode(XElement obj)
        {
            return ((int)obj).GetHashCode();
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // hr, en, njema, tal
            string agentName = "apartmanihrvatska-hr";
            string advMngConfPath = @"D:\projekti-local\MRI\Placeberry\dev\PlaceberryCrawlerConsole\Adverts\AdvertFieldsConfig.xml";
            string agentPath = @"D:\projekti-local\MRI\Placeberry\dev\PlaceberryCrawlerConsole\Agents\" + agentName + ".xml";

            AdvertManager advMng = new AdvertManager(advMngConfPath);
            XDocument xdoc = XDocument.Load(agentPath);


            List<XElement> pages = xdoc.Descendants("Page").Distinct(new PageComparer()).ToList();

            List<Page> lstPages = new List<Page>();

            for (int i = 0; i < pages.Count; ++i)
            {
                lstPages.Add(new Page(xdoc, ref advMng, i));
            }

            int lastPageIndex = 0;
            int curPageIndex = 0;

            while (curPageIndex >= 0)
            {
                string nextPageUrl = null;
                lstPages[curPageIndex].ExecutePageActions(out curPageIndex, out nextPageUrl);

                if (nextPageUrl != null)
                {
                    if (!lstPages[curPageIndex].SetNextUri(nextPageUrl))
                    {
                        // ukoliko je obrada stranice vratila nesipravan url proces se vraca na backup indexa stranice (onog indexa koji je postavljen prije outa)
                        curPageIndex = lastPageIndex;
                        continue;
                    }
                }
                // ukoliko je sve proslo ok backupira se posljedni index
                lastPageIndex = curPageIndex;
            }

            new CsvWriter("D:\\projekti-local\\MRI\\Placeberry\\CSV_work\\C" + agentName + ".csv").Write(advMng.Adverts);
        }
    }
}
