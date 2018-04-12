using System;
using System.Collections.Generic;
using System.Text;

namespace Gnsso.Html
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HtmlRegexAttribute : Attribute
    {
        public string Pattern { get; set; }
        public string Replacement { get; set; }

        public HtmlRegexAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
