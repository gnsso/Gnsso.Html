using System;
using System.Collections.Generic;
using System.Text;

namespace Gnsso.Html
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class HtmlConverterAttribute : Attribute
    {
        public Type Type { get; set; }
        public object[] Args { get; set; }

        public HtmlConverterAttribute(Type type)
        {
            Type = type;
        }
    }
}
