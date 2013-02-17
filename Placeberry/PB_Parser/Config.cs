using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PB_Parser
{
    class Config
    {
        public string Url { get; set; }
        public string Lang { get; set; }

        public IList<Expression> Expressions { get; set; }
        public IList<Mapping> Mappings { get; set; }
        public IList<Duplication> Duplications { get; set; }

        public string LanguageColumn { get; set; }

        public class Duplication
        {
            public string From { get; set; }
            public string To { get; set; }
        }

        public class Mapping
        {
            public string From { get; set; }
            public string To { get; set; }
            public bool IsInHeader { get; set; }
        }

        public class Expression
        {
            public Regex Regex { get; set; }
            public string Item { get; set; }
        }
    }
}
