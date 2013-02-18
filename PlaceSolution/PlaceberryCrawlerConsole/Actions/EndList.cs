using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    public class EndList : ActionBase
    {
        private int _startListIndex = -1;

        public string ListName { get; private set; }

        public EndList(Page page, XElement xel, int actionIndex)
            : base(page, xel, actionIndex)
        {
            if(xel.Element("ListName") == null)
            {
                throw new Exception("Error in GetElementValue xml Action element");
            }

            this.ListName = xel.Element("ListName").Value;


            // treba pronaci pripadajuci BeginAnchorList na ovoj stranici kako bi ExecuteAction poslao na njega.
            List<XElement> actions = (from els in _page.Xdoc.Descendants("Action")
                                      where Int32.Parse(els.Element("Page").Value) == _page.PageIndex
                                      select els).ToList();

            for (int i = 0; i < actions.Count; ++i)
            {
                if (actions[i].Element("ActionType").Value == ActionTypeKind.BeginAnchorList.ToString()
                    && actions[i].Element("ListName").Value == this.ListName)
                {
                    _startListIndex = i;
                    break;
                }
            }

            if (_startListIndex < 0)
            {
                throw new Exception("BeginAnchorList action not found for current EndList!");
            }
        }

        public override void ExecuteAction(out int nextActionIndex, out int nextPageIndex, out string nextPageUrl)
        {
            nextPageUrl = null;

            nextActionIndex = _startListIndex;
            nextPageIndex = _page.PageIndex;
        }
    }
}
