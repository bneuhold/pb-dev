using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    public class BeginAnchorList : ActionBase
    {
        private int _endListIndex = -1;         // predstavlja index EndList action za ovaj BeginAnchorList action
        private int _currentNodeIndex = 0;      // Predsavlja index sa kojeg ce se dohvatiti nod iz AnchorListNodeCollection-a u pripadajuce Page klasi. Ovdje je zapravo brojac poziva ExecuteAction metode.
        private int _startNodeIndex = 0;        // Predstavlja index prvog noda koji ce se dohvacati. Npr. ako ima prvo neke info nodove pa tek onda privatan smjestaj, hoteli... da preskoci te prve nodove.
        private int _lastNodeIndex = 0;         // Predstavlja index posljednjeg noda koji ce se dohvacati. Npr. ako ima privatan smjestaj, hoteli... pa kasnije neki info tagovi, da se izbjegnu ti posljednji nepotrebni tagovi

        // malo nespretno iz ovog actiona pratiti indexe u kolekciji nodova iz Page klase, al da se ne zelim mjesati rad sa HtmlAgilitypackom u Action klasama

        private List<string> _xpaths;
        private string _listName;


        public BeginAnchorList(Page page, XElement xel, int actionIndex)
            : base(page, xel, actionIndex)
        {
            _xpaths = GetXPaths();

            if (_xpaths.Count == 0 || xel.Element("ListName") == null)
            {
                throw new Exception("Error in GetElementValue xml Action element");
            }

            this._listName = xel.Element("ListName").Value;

            // pronaci odgovarajuci endlist element u xdocu ovog page-a
            List<XElement> actions = (from els in _page.Xdoc.Descendants("Action")
                                      where Int32.Parse(els.Element("Page").Value) == _page.PageIndex
                                      select els).ToList();

            for (int i = 0; i < actions.Count; ++i)
            {
                if (actions[i].Element("ActionType").Value == ActionTypeKind.EndList.ToString()
                    && actions[i].Element("ListName").Value == this._listName)
                {
                    _endListIndex = i;
                    break;
                }
            }

            if (_endListIndex == -1)
            {
                throw new Exception("EndList action not found for current BeginAnchorList!");
            }

            // ukoliko je postavljeno postaviti zadnji index node-a koji ce se dohvacati
            // npr. ako idu jahte, jedrili, katamarane pa kasnije neki info tagovi indeksirati do tih info tagova
            if (xel.Element("ItemCounter") != null)
            {
                Int32.TryParse(xel.Element("ItemCounter").Value, out _lastNodeIndex);
            }
            // ukoliko je postavljeno pronaci postaviti prvi index node-a koji ce se dohvacati
            // npr. ako idu neki info tagovi pa onda jahte, jedrilice, katamarani... da se preskoce info nodovi
            if (xel.Element("ItemSkip") != null)
            {
                Int32.TryParse(xel.Element("ItemSkip").Value, out _startNodeIndex);
            }

            // postaviti konacne node indexe
            _lastNodeIndex += _startNodeIndex;
            _currentNodeIndex = _startNodeIndex;

        }

        public override void ExecuteAction(out int nextActionIndex, out int nextPageIndex, out string nextPageUrl)
        {
            nextPageUrl = null;
            nextPageIndex = _page.PageIndex;

            // Page klasa dohvati sve AnchorList nodove za odgovarajuci xpath i _currentNodeIndex se postavlja na pocetni
            if (!_page.SetAnchorListNodeCollection(_xpaths))
            {
                Console.WriteLine("Nema noda za listu: " + _listName + ", na stranici: " + _page.PageIndex + ". grad: " + _page.AdvMng.CurrentAdvert.columns[18]);
                // ukoliko kolekcija nodova ne postoji postaviti nextActionIndex na prvi iza pripadajuceg EndList elementa
                nextActionIndex = _endListIndex + 1;
                _currentNodeIndex = _startNodeIndex;
                return;
            }

            if ((_lastNodeIndex != 0 && _currentNodeIndex == _lastNodeIndex) || !_page.SetAnchorListIndex(_currentNodeIndex))
            {
                // ukoliko sljedeci node vise ne postoji postaviti index na prvi iza pripadajuceg EndList elementa
                // ali i resetirati counter u slucaju da se ponovo sa iduce stranice na ovu listu
                nextActionIndex = _endListIndex + 1;
                _currentNodeIndex = _startNodeIndex;
                return;
            }

            ++_currentNodeIndex;
            nextActionIndex = _actionIndex + 1;
        }
    }
}
