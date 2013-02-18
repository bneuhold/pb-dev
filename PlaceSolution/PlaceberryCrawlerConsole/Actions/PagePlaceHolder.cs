using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    public class PagePlaceHolder : ActionBase
    {
        private Encoding _encoding;
        private string _baseUrl = null;


        public PagePlaceHolder(Page page, XElement xel, int actionIndex)
            : base(page, xel, actionIndex)
        {
            if (xel.Element("BaseUrl") != null)
            {
                _baseUrl = xel.Element("BaseUrl").Value;
            }
            
            if (xel.Element("Encoding") == null)
            {
                throw new Exception("PagePlaceHolder is missing encoding element!");
            }

            switch (xel.Element("Encoding").Value.ToLower())
            {
                case "utf8":
                    _encoding = Encoding.UTF8;
                    break;
                case "default":
                    _encoding = Encoding.Default;
                    break;
                default:
                    throw new Exception("Wrong Encoding type in PagePlaceHolder!");
            }
        }

        public override void ExecuteAction(out int nextActionIndex, out int nextPageIndex, out string nextPageUrl)
        {
            nextPageUrl = null;

            try
            {
                _page.LoadPage(_encoding, _baseUrl);
            }
            catch (Exception)
            {
                // ako ne uspije ucitati stranicu:
                // ak je stigo iz ClickElementa treba vratiti na prethodnu stranicu, nastavit ce dalje po listi agenata u kojoj je bilo ClickElement, iza ClickElementa
                // ak je stigo iz PageLista (to je next, next...) opet vratiti na  prethodnu stranicu.
                nextPageIndex = _page.PageIndex - 1;
                // _sctionIndex ne treba mjenjati
            }

            nextActionIndex = _actionIndex + 1;
            nextPageIndex = _page.PageIndex;
        }
    }
}
