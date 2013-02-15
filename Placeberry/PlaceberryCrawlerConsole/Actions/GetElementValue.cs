using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace PlaceberryCrawlerConsole
{
    public class GetElementValue : ActionBase
    {
        private string _predefinedValue = null; // za jezik i source se lako upisu vrijednosti u PredefinedValue tag jer su stalno iste pa da se ne cupa HtmlAgilityPackom bespotrebno
        private List<string> _xpaths;
        private NodeValueType _nodeValueType;
        private StartExpressionSeparator _startExprSep;
        private EndExpressionSeparator _endExprSep;
        private List<Replacement> _lstReplacements;
        private string _fieldName = null;


        public GetElementValue(Page page, XElement xel, int actionIndex)
            : base(page, xel, actionIndex)
        {

            if (xel.Element("FieldName") == null)
            {
                throw new Exception("Error in GetElementValue xml Action element, missing FieldName!");
            }

            _fieldName = xel.Element("FieldName").Value;
            _predefinedValue = GetPredefinedValue();

            if (_predefinedValue == null)
            {
                _xpaths = GetXPaths();

                if (_xpaths.Count == 0)
                {
                    throw new Exception("Error in GetElementValue xml Action element, missing XPaths!");
                }

                _nodeValueType = GetNodeValueType();
                _startExprSep = GetStartExpSep();
                _endExprSep = GetEndExpSep();
                _lstReplacements = GetReplacements();
            }
        }

        public override void ExecuteAction(out int nextActionIndex, out int nextPageIndex, out string nextPageUrl)
        {
            nextPageUrl = null;
            nextActionIndex = _actionIndex + 1;
            nextPageIndex = _page.PageIndex;

            if (_predefinedValue != null)
            {
                _page.AdvMng.AddFieldValue(_fieldName, _predefinedValue);
            }
            else
            {
                string value = _page.GetNodeValue(_xpaths, _nodeValueType, _startExprSep, _endExprSep, _lstReplacements);

                if (value == null)
                {
                    value = string.Empty;
                }

                // WORKAROUND za description
                if (_fieldName != "33Description")
                {
                    value = value.Replace("\n", " ");
                }

                value = value.Trim();

                if (value.Length > _page.AdvMng.MaxFieldLen)
                {
                    value = value.Substring(0, _page.AdvMng.MaxFieldLen - 3);
                    value += "...";
                }

                _page.AdvMng.AddFieldValue(_fieldName, value);
            }
        }
    }
}
