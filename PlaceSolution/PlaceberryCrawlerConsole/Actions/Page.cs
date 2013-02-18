using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using HtmlAgilityPack;
using WatiN.Core;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Web;

namespace PlaceberryCrawlerConsole
{
    public class Page
    {
        // Agent mechanics
        private int _currActionIndex;
        private List<ActionBase> _lstActions;
        public int PageIndex { get; private set; }
        public XDocument Xdoc { get; private set; }

        // HTML parsig
        public Uri LoadedUri { get; private set; }      // uri stranice koja je trenutno ucitana
        public Uri NexUriToLoad { get; private set; }   // uri stranice koje se treba ucitati iducim pozivom LoadPage metode
        private Uri _baseUri = null;                    // bazni uri stranice. uglavnom sluzi za prepravljanje relativnih urlova unutar html-a u apsolutne
        private HtmlDocument _htmlDoc;

        private IE _browser;
        private HtmlNodeCollection _anchorLstNodeColl;
        private int _anchorLstCurrIndex;

        public AdvertManager AdvMng { get; private set; }        


        public Page(XDocument xdoc, ref AdvertManager advertManager, int pageIndex)
        {
            _htmlDoc = new HtmlDocument();

            //Agent mechanics
            this.PageIndex = pageIndex;
            _currActionIndex = 0;
            this.AdvMng = advertManager;
            this.Xdoc = xdoc;

            List<XElement> actions = (from els in xdoc.Descendants("Action")
                                      where Int32.Parse(els.Element("Page").Value) == pageIndex
                                      select els).ToList();

            _lstActions = new List<ActionBase>();

            for (int i = 0; i < actions.Count; ++i)
            {
                _lstActions.Add(ActionBase.CreateAction(this, actions[i], i));
            }
        }

        /// <summary>
        /// Postavlja iduci url koji ce bit ucitan. Ovu je metodu obavezno pozvat prije prvog poziva LoadPage metode.
        /// </summary>
        /// <param name="nextUrl"></param>
        public bool SetNextUri(string nextUrl)
        {
            nextUrl = nextUrl.Trim();
            Uri tempUri;

            if (Uri.TryCreate(nextUrl, UriKind.RelativeOrAbsolute, out tempUri))
            {
                Uri baseUri = _baseUri == null ? this.LoadedUri : _baseUri;
                if (!tempUri.IsAbsoluteUri && !Uri.TryCreate(baseUri, tempUri, out tempUri))
                {
                    return false;
                }

                this.NexUriToLoad = tempUri;
                return true;
            }

            return false;
        }

        public void LoadPage(Encoding encoding, string baseUrl)
        {
            LoadPage(false, encoding, baseUrl);
        }

        public void LoadPage(bool useBrowser, Encoding encoding, string baseUrl)
        {
            if (NexUriToLoad == null)
            {
                throw new Exception("Page url must be set in SetNextUri method before calling LoadPage method!");
            }

            // bazni uri stranice. uglavnom sluzi za prepravljanje relativnih urlova unutar html-a u apsolutne.
            if (baseUrl != null && !Uri.TryCreate(baseUrl.Trim(), UriKind.Absolute, out _baseUri))
            {
                throw new Exception("Error while creating BaseUri for page: " + this.PageIndex);
            }

            if (this.LoadedUri != null && this.LoadedUri == NexUriToLoad)
            {
                // da se izbjegne nepotrebo ucitavanje stranice ako je ista vec ucitana
                return;
            }

            HtmlNode.ElementsFlags.Remove("form");

            this.LoadedUri = NexUriToLoad;
            _anchorLstNodeColl = null;
            _anchorLstCurrIndex = 0;

            if (useBrowser)
            {
                // ovako privremeno nesto
                if (_browser == null)
                {
                    _browser = new IE();
                }

                _browser.GoTo(this.LoadedUri.AbsoluteUri);
                Thread.Sleep(10000);
            }
            else
            {
                using (WebClient webClient = new WebClient())
                {
                    using (Stream stream = webClient.OpenRead(this.LoadedUri.AbsoluteUri))
                    {
                        _htmlDoc.Load(stream, encoding);
                    }
                }

                //_htmlDoc = new HtmlWeb().Load(this.LoadedUri.AbsoluteUri);
            }

            // ocistiti script nodove jer ionako nicem ne sluze
            //List<HtmlNode> scriptNodes = _htmlDoc.DocumentNode.Descendants("script").ToList();
            //foreach (HtmlNode sn in scriptNodes)
            //{
            //    sn.ParentNode.RemoveChild(sn);
            //}
            // sluzit ce za dohvat google maps koordinata

            RepairAncorHref();
            RepairImageSrc();
        }

        // Postavlja svim linkovima koji imaju Relative URL na Apsolute URL.
        private void RepairAncorHref()
        {
            HtmlNodeCollection links = _htmlDoc.DocumentNode.SelectNodes(@"//a[@href]");
            if (links != null)
            {
                foreach (HtmlNode link in links)
                {
                    // preskociti linkove koji nemaju href, href je prazan ili poziva javascript
                    HtmlAttribute att = link.Attributes["href"];
                    if (att == null)
                        continue;

                    string href = att.Value;
                    if (String.IsNullOrEmpty(href) || href.StartsWith("javascript", StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    // ocistiti web posebne znakove
                    href = HttpUtility.HtmlDecode(href);

                    // ukoliko su hrefovi postavljeni na relative uri postaviti ih na absolute uri
                    Uri uriLink = new Uri(href, UriKind.RelativeOrAbsolute);

                    if (!uriLink.IsAbsoluteUri)
                    {
                        Uri baseUri = _baseUri == null ? this.LoadedUri : _baseUri;
                        uriLink = new Uri(baseUri, uriLink);
                        // izmjeniti href trenutno obradenog linka u ucitanom HtmlDocument-u
                        link.Attributes["href"].Value = uriLink.AbsoluteUri;
                    }
                }
            }
        }

        // Postavlja src svim slikama koji imaju Relative URL na Apsolute URL.
        private void RepairImageSrc()
        {
            HtmlNodeCollection imgs = _htmlDoc.DocumentNode.SelectNodes(@"//img[@src]");
            if (imgs != null)
            {
                foreach (HtmlNode img in imgs)
                {
                    // preskociti slike koje nemaju src, src je prazan ili poziva javascript
                    HtmlAttribute att = img.Attributes["src"];
                    if (att == null)
                        continue;

                    string src = att.Value;
                    if (String.IsNullOrEmpty(src))
                        continue;

                    src = HttpUtility.HtmlDecode(src);

                    Uri uriSrc = new Uri(src, UriKind.RelativeOrAbsolute);

                    if (!uriSrc.IsAbsoluteUri)
                    {
                        Uri baseUri = _baseUri == null ? this.LoadedUri : _baseUri;
                        uriSrc = new Uri(baseUri, uriSrc);

                        // izmjeniti href trenutno obradenog linka u ucitanom HtmlDocument-u
                        img.Attributes["src"].Value = uriSrc.AbsoluteUri;
                    }
                }
            }
        }


        // AnchorList manipulation methods

        /// <summary>
        /// Postavlja CurrentNode prema prvom pronadenom xpathu u zadanoj listi xpath-ova i vraca true. Ukoliko niti jedan xpath nije pronaden vraca false;
        /// </summary>
        /// <param name="xpaths"></param>
        /// <returns></returns>
        public bool SetAnchorListNodeCollection(List<string> xpaths)
        {
            foreach (string xp in xpaths)
            {
                if (SetAnchorListNodeCollection(xp))
                    break;
            }

            return _anchorLstNodeColl != null;
        }

        public bool SetAnchorListNodeCollection(string xpath)
        {
            xpath = xpath.Trim();

            _anchorLstNodeColl = _htmlDoc.DocumentNode.SelectNodes(xpath);
            _anchorLstCurrIndex = 0;

            // WORKAROUND za slcaj da uleti tbody kojeg obicno nema
            if (_anchorLstNodeColl == null && (xpath.Contains("/tbody[1]") || xpath.Contains("/tbody")))
            {
                xpath = xpath.Replace("/tbody[1]", string.Empty);
                xpath = xpath.Replace("/tbody", string.Empty);
                _anchorLstNodeColl = _htmlDoc.DocumentNode.SelectNodes(xpath);
            }

            return _anchorLstNodeColl != null;
        }

        public bool SetAnchorListIndex(int newIndex)
        {
            if (_anchorLstNodeColl == null)
            {
                _anchorLstCurrIndex = 0;
                return false;
            }

            _anchorLstCurrIndex = newIndex;

            if (_anchorLstCurrIndex >= _anchorLstNodeColl.Count)
            {
                _anchorLstNodeColl = null;
                _anchorLstCurrIndex = 0;

                return false;
            }

            return true;
        }

        public string GetNodeValue(List<string> xpaths, NodeValueType nodeValueType,
            StartExpressionSeparator startExprSep, EndExpressionSeparator endExprSep, List<Replacement> lstReplacements)
        {
            string retVal = null;

            foreach (string xpath in xpaths)
            {
                retVal = GetNodeValue(xpath, nodeValueType, startExprSep, endExprSep, lstReplacements);
                if (retVal != null)
                    break;
            }

            return retVal;
        }

        public string GetNodeValue(string xpath, NodeValueType nodeValueType,
            StartExpressionSeparator startExprSep, EndExpressionSeparator endExprSep, List<Replacement> lstReplacements)
        {
            string retVal = null;
            HtmlNode node = null;

            xpath = xpath.Trim();

            if (xpath == ".")
            {
                node = _anchorLstNodeColl[_anchorLstCurrIndex];
            }
            else
            {
                if (xpath.StartsWith("..")) // znaci da ide prema parentu po neki xpath axis (npr ../following-sibling ili ../preceding-sibling)
                {
                    if (_anchorLstNodeColl == null)
                    {
                        return null;
                    }

                    xpath = _anchorLstNodeColl[_anchorLstCurrIndex].XPath + "/" + xpath;    // u tom slucaju treba nadograditi postojeci
                }

                node = _htmlDoc.DocumentNode.SelectSingleNode(xpath);

                // WORKAROUND za slcaj da uleti tbody kojeg obicno nema, ali ga recimo firefox voli dodavati
                if (node == null && (xpath.Contains("/tbody[1]") || xpath.Contains("/tbody")))
                {
                    xpath = xpath.Replace("/tbody[1]", string.Empty);
                    xpath = xpath.Replace("/tbody", string.Empty);
                    node = _htmlDoc.DocumentNode.SelectSingleNode(xpath);
                }
            }

            if (node != null)
            {
                switch (nodeValueType)
                {
                    case NodeValueType.InnerText:
                        retVal = node.InnerText;
                        break;

                    case NodeValueType.InnerHtml:
                        retVal = node.InnerHtml;
                        break;

                    case NodeValueType.OuterHtml:
                        retVal = node.OuterHtml;
                        break;
                }


                // 1. PROCESUIRANJE StartExpressionSeparatora i EndExpressionSeparatora
                if (startExprSep != null)
                {
                    if (!startExprSep.ValidateSeparator(retVal))
                        return null;

                    retVal = startExprSep.ProcessValue(retVal);
                }

                if (endExprSep != null)
                {
                    if (!endExprSep.ValidateSeparator(retVal))
                        return null;

                    retVal = endExprSep.ProcessValue(retVal);
                }


                // 2. CISCENJE POVRATNE VRIJEDNOSTI
                // dekodira web znakove kao sto su '&lt;' i '&gt;', brise visestruko pojavljivanje novih redaka, pretvara tabove u space i brise spaceove ili navodnike sa pocetka i kraja
                retVal = HttpUtility.HtmlDecode(retVal);

                retVal = retVal.Replace("\r\n", "\n");
                retVal = retVal.Replace("\\r\\n", "\n");
                retVal = retVal.Replace("\\n", "\n");

                while (retVal.Contains("\n\n"))
                {
                    retVal = retVal.Replace("\n\n", "\n");
                }

                retVal = retVal.Replace("\t", " ");
                retVal = retVal.Replace("\\t", " ");

                retVal = retVal.Trim();
                retVal = retVal.Trim('"');
                retVal = retVal.Trim('\'');
                retVal = retVal.Trim();


                // 3. PROVODENJE REPLACEMENTA
                if(lstReplacements != null)
                {
                    foreach (Replacement rep in lstReplacements)
                    {
                        retVal = rep.Replace(retVal);
                    }
                }
            }

            return retVal;
        }


        /// <summary>
        /// Returns index of next page where actions should be executed.
        /// </summary>
        /// <returns></returns>
        public void ExecutePageActions(out int nextPageIndex, out string nextPageUrl)
        {
            nextPageIndex = this.PageIndex;
            nextPageUrl = null;

            while (nextPageIndex == this.PageIndex)
            {
                if (_currActionIndex < _lstActions.Count)
                {
                    _lstActions[_currActionIndex].ExecuteAction(out _currActionIndex, out nextPageIndex, out nextPageUrl);
                }
                else
                {
                    // ako je index za akcije presao iza zadnjeg polja u akcijama vratiti se na prethodnu stranicu i postaviti index za akcije na pocetak
                    _currActionIndex = 0;
                    nextPageIndex = this.PageIndex - 1;
                }
            }
        }
    }
}
