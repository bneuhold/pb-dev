using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    public class PageList : ActionBase
    {
        private int _pagePlaceHolderIndex = -1;
        private List<string> _xpaths;  // jel treba vise od jednog xpatha?! (se moze link na iducu straicu na nekoj od stranica pojavi u drugom xpathu?!)
        private NodeValueType _nodeValueType;
        private StartExpressionSeparator _startExprSep;
        private EndExpressionSeparator _endExprSep;


        public PageList(Page page, XElement xel, int actionIndex)
            : base(page, xel, actionIndex)
        {
            _xpaths = GetXPaths();

            if (_xpaths.Count == 0)
            {
                throw new Exception("Error in PageList xml Action element. Missing XPath!");
            }

            // pronaci LoadPage element na ovoj stranici
            List<XElement> actions = (from els in _page.Xdoc.Descendants("Action")
                                      where Int32.Parse(els.Element("Page").Value) == _page.PageIndex
                                      select els).ToList();

            for (int i = 0; i < actions.Count; ++i)
            {
                if (actions[i].Element("ActionType").Value == ActionTypeKind.PagePlaceHolder.ToString())
                {
                    _pagePlaceHolderIndex = i;
                    break;
                }
            }

            if (_pagePlaceHolderIndex < 0)
            {
                throw new Exception("LoadPage action not found for current PageList action!");
            }
            
            _nodeValueType = GetNodeValueType();

            _startExprSep = GetStartExpSep();
            _endExprSep = GetEndExpSep();


            // PageList treba izvuci href iz targetiranog noda koji vodi na iducu stranicu
            // po defaultu bi trebao pocinjati sa hreaf=", i zavrsiti sa " ukoliko Start i End Expression separatori nisu navedeni

            if (_startExprSep == null)
            {
                _startExprSep = new StartExpressionSeparator("href=\"", false, true);
            }

            if(_endExprSep == null)
            {
                _endExprSep = new EndExpressionSeparator("\"", false, true);
            }
        }

        public override void ExecuteAction(out int nextActionIndex, out int nextPageIndex, out string nextPageUrl)
        {
            nextPageUrl = null;

            // izvuci i postaviti iduci url na koji ce pripadajuca stranica ici
            string nextUrl = _page.GetNodeValue(_xpaths, _nodeValueType, _startExprSep, _endExprSep, null);


            if (nextUrl != null && _page.SetNextUri(nextUrl))
            {
                // ukoliko sve prode ok:

                nextActionIndex = _pagePlaceHolderIndex;    // actionIndex ove stranice postavlja na PagePlaceHolder action ove stranice (koji je uvijek prvi)
                nextPageIndex = _page.PageIndex;            // pageIndex ostaje ovaj
            }
            else
            {
                // ako ne uspije nac link na iducu stranicu ili ako link nije uspjesno postavljen:

                nextPageIndex = _page.PageIndex - 1;    // vraca se na prethodnu stranicu
                nextActionIndex = 0;                    // kako je ovo uvijek posljednji action na stranici postavja nextactionindex svoje stranice na 0
            }
        }
    }
}
