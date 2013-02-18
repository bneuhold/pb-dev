using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    public class ClickElement : ActionBase
    {
        private int _destinationPage;
        private List<string> _xpaths;
        private StartExpressionSeparator _startExprSep;
        private EndExpressionSeparator _endExprSep;
        private NodeValueType _nodeValueType;


        public ClickElement(Page page, XElement xel, int actionIndex)
            : base(page, xel, actionIndex)
        {
            _xpaths = GetXPaths();

            if (_xpaths.Count == 0
                || xel.Element("DestinationPage") == null || !Int32.TryParse(xel.Element("DestinationPage").Value, out _destinationPage))
            {
                throw new Exception("Error in ClickElement xml Action element.");
            }

            _startExprSep = GetStartExpSep();
            _endExprSep = GetEndExpSep();
            _nodeValueType = GetNodeValueType();

            // ClickElement treba izvuci href iz targetiranog noda koji vodi na kliknuti link
            // po defaultu bi trebao pocinjati sa hreaf=", i zavrsiti sa " ukoliko Start i End Expression separatori nisu navedeni
            if (_startExprSep == null)
            {
                _startExprSep = new StartExpressionSeparator("href=\"", false, true);
            }

            if (_endExprSep == null)
            {
                _endExprSep = new EndExpressionSeparator("\"", false, true);
            }
            // isto tako po defaultu uzima NodevalueType OuterHtml ukoliko nije naveden
            if (_nodeValueType == null)
                _nodeValueType = NodeValueType.OuterHtml;
        }

        public override void ExecuteAction(out int nextActionIndex, out int nextPageIndex, out string nextPageUrl)
        {
            nextActionIndex = _actionIndex + 1; // ovo ostaje nextAction Index ove stranice. Page na kojoj ce se nastaviti ima oznaceni svoj index actiona na kojem nastavlja

            nextPageUrl = _page.GetNodeValue(_xpaths, _nodeValueType, _startExprSep, _endExprSep, null);

            // ako je uspio naci link na destination page otici ce na destination page i poslati novi link, u suprotnome samo ce nastaviti dalje po akcijama trenutne stranice
            nextPageIndex = nextPageUrl != null ? this._destinationPage : this._page.PageIndex;
        }
    }
}
