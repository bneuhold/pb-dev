using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    public class LoadPage : ActionBase
    {
        private int _destinationPage;
        private bool _agentStart = false;
        private string _pageUrl;

        public LoadPage(Page page, XElement xel, int actionIndex)
            : base(page, xel, actionIndex)
        {
            if (xel.Element("PageUrl") == null)
            {
                throw new Exception("LoadPage is missing PageUrl element!");
            }

            int destPag;
            if (xel.Element("DestinationPage") == null || !Int32.TryParse(xel.Element("DestinationPage").Value, out destPag))
            {
                throw new Exception("Error in PagePlaceHolder xml Action element");
            }

            _destinationPage = destPag;
            _pageUrl = xel.Element("PageUrl").Value;
        }

        public override void ExecuteAction(out int nextActionIndex, out int nextPageIndex, out string nextPageUrl)
        {
            nextPageUrl = null;
            nextActionIndex = _actionIndex;

            // LoadPage Action je uvjek samo jedan i nalazi se na nultoj stranici.
            // Ukoliko se pozove po drugi put znaci da je agent gotov!
            if (!_agentStart)
            {
                nextPageIndex = _destinationPage;
                nextPageUrl = _pageUrl; // postavlja url prvom page-u 
                nextActionIndex = _actionIndex;
                
                _agentStart = true;
            }
            else 
            {
                nextPageIndex = this._page.PageIndex - 1;
            }
        }
    }
}
