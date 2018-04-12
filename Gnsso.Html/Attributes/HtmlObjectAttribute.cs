using System;

namespace Gnsso.Html
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public sealed class HtmlObjectAttribute : Attribute
    {
        private string selector;
        public string Selector => selector;

        public HtmlObjectAttribute() : this(null)
        {
            
        }

        public HtmlObjectAttribute(string selector)
        {
            this.selector = selector;
        }
    }
}
