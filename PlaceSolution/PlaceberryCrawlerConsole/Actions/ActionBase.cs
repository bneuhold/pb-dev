using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PlaceberryCrawlerConsole
{
    public abstract class ActionBase
    {
        protected Page _page;
        protected XElement _xel;

        protected ActionTypeKind _actionType;
        protected int _actionIndex;


        protected ActionBase(Page page, XElement xel, int actionIndex)
        {
            if ((xel.Element("ActionType") == null || !Enum.TryParse(xel.Element("ActionType").Value, true, out _actionType)))
            {
                throw new Exception("Error in PagePlaceHolder xml Action element");
            }

            _page = page;
            _xel = xel;
            _actionIndex = actionIndex;
        }


        public abstract void ExecuteAction(out int nextActionIndex, out int nextPageIndex, out string nextPageUrl);


        // metode za dohvat vrijednosti xml elemenata

        protected List<string> GetXPaths()
        {
            List<string> lstXpaths = new List<string>();
            foreach (XElement e in _xel.Elements("ItemXPath"))
            {
                lstXpaths.Add(e.Value.Trim());
            }

            return lstXpaths;
        }

        protected string GetPredefinedValue()
        {
            string value = null;

            if (_xel.Element("PredefinedValue") != null)
            {
                value = _xel.Element("PredefinedValue").Value;
            }

            return value;
        }

        protected NodeValueType GetNodeValueType()
        {
            if (_xel.Element("NodeValueType") != null)
            {
                return (NodeValueType)Enum.Parse(typeof(NodeValueType), _xel.Element("NodeValueType").Value);
            }

            return NodeValueType.InnerText;
        }

        public StartExpressionSeparator GetStartExpSep()
        {
            XElement sepEl = _xel.Element("StartExpressionSeparator");

            if (sepEl == null)
                return null;

            return new StartExpressionSeparator(sepEl.Value,
                sepEl.Attribute("Include") == null ? false : Convert.ToBoolean(sepEl.Attribute("Include").Value),
                sepEl.Attribute("MustExist") == null ? true : Convert.ToBoolean(sepEl.Attribute("MustExist").Value));
        }

        public EndExpressionSeparator GetEndExpSep()
        {
            XElement sepEl = _xel.Element("EndExpressionSeparator");

            if (sepEl == null)
                return null;

            return new EndExpressionSeparator(sepEl.Value,
                sepEl.Attribute("Include") == null ? false : Convert.ToBoolean(sepEl.Attribute("Include").Value),
                sepEl.Attribute("MustExist") == null ? true : Convert.ToBoolean(sepEl.Attribute("MustExist").Value));
        }

        public List<Replacement> GetReplacements()
        {
            var repEls = _xel.Elements("Replacement");

            if (repEls == null)
                return null;

            List<Replacement> lstRep = new List<Replacement>();

            foreach (XElement repEl in repEls)
            {
                lstRep.Add(new Replacement(repEl.Value,
                    repEl.Attribute("To") == null ? String.Empty : repEl.Attribute("To").Value,
                    repEl.Attribute("Type") == null ? ReplacementType.Text : (ReplacementType)Enum.Parse(typeof(ReplacementType), repEl.Attribute("Type").Value)));
            }

            return lstRep;
        }


        public static ActionBase CreateAction(Page page, XElement xel, int actionIndex)
        {
            ActionTypeKind type;
            if (xel.Element("ActionType") == null || !Enum.TryParse(xel.Element("ActionType").Value, true, out type))
            {
                throw new Exception("ActionType element is missing or invalid!");
            }

            switch (type)
            {
                case ActionTypeKind.LoadPage:
                    return new LoadPage(page, xel, actionIndex);
                case ActionTypeKind.PagePlaceHolder:
                    return new PagePlaceHolder(page, xel, actionIndex);
                case ActionTypeKind.GetElementValue:
                    return new GetElementValue(page, xel, actionIndex);
                case ActionTypeKind.BeginAnchorList:
                    return new BeginAnchorList(page, xel, actionIndex);
                case ActionTypeKind.ClickElement:
                    return new ClickElement(page, xel, actionIndex);
                case ActionTypeKind.EndList:
                    return new EndList(page, xel, actionIndex);
                case ActionTypeKind.PageList:
                    return new PageList(page, xel, actionIndex);
                case ActionTypeKind.ActionCommand:
                    return new ActionCommand(page, xel, actionIndex);
                default:
                    throw new Exception("Unknown ActionType!");
            }
        }
    }

    public enum ActionTypeKind
    {
        LoadPage,
        PagePlaceHolder,
        GetElementValue,
        BeginAnchorList,
        ClickElement,
        EndList,
        PageList,
        ActionCommand
    }

    public enum NodeValueType
    {
        InnerText,
        OuterHtml,
        InnerHtml        
    }
}
