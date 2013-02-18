using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    public class ActionCommand : ActionBase
    {
        private string _commandLine;

        public ActionCommand(Page page, XElement xel, int actionIndex)
            : base(page, xel, actionIndex)
        {
            if (xel.Element("CommandLine") == null)
            {
                throw new Exception("ActionCommand is missing CommandLine element!");
            }

            _commandLine = xel.Element("CommandLine").Value;
        }

        public override void ExecuteAction(out int nextActionIndex, out int nextPageIndex, out string nextPageUrl)
        {
            nextPageUrl = null;
            nextActionIndex = _actionIndex + 1;
            nextPageIndex = _page.PageIndex;


            if (_commandLine.ToLower().Equals("insert advert"))
            {
                _page.AdvMng.InsertCurrentAdvert(); 
            }
        }
    }

}
