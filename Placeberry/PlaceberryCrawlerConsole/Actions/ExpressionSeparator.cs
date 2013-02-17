using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaceberryCrawlerConsole
{
    public abstract class ExpressionSeparatorBase
    {
        protected string _expresion;
        protected bool _include;
        protected bool _mustExist;

        protected ExpressionSeparatorBase(string expression, bool include, bool mustExist)
        {
            _expresion = expression;
            _include = include;
            _mustExist = mustExist;
        }

        public bool ValidateSeparator(string value)
        {
            if (!String.IsNullOrEmpty(_expresion) && _mustExist && !value.Contains(_expresion))
            {
                return false;
            }

            return true;
        }

        public abstract string ProcessValue(string value);
    }


    public class StartExpressionSeparator : ExpressionSeparatorBase
    {
        public StartExpressionSeparator(string expression, bool include, bool mustExist)
            : base(expression, include, mustExist)
        { }

        public override string ProcessValue(string value)
        {
            string retVal = value;

            if (!String.IsNullOrEmpty(_expresion) && value.Contains(_expresion))
            {
                retVal = value.Substring(value.IndexOf(_expresion) + (_include ? 0 : _expresion.Length));
            }

            return retVal;
        }
    }


    public class EndExpressionSeparator : ExpressionSeparatorBase
    {
        public EndExpressionSeparator(string expression, bool include, bool mustExist)
            : base(expression, include, mustExist)
        { }

        public override string ProcessValue(string value)
        {
            string retVal = value;

            if (!String.IsNullOrEmpty(_expresion) && value.Contains(_expresion))
            {
                retVal = value.Remove(value.IndexOf(_expresion) + (_include ? _expresion.Length : 0));
            }

            return retVal;
        }
    }
}
