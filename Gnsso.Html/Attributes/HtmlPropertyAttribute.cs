using System;

namespace Gnsso.Html
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class HtmlPropertyAttribute : Attribute
    {
        private string selector;
        public string Selector => selector;

        public HtmlPropertyAttribute() : this(null)
        {

        }

        public HtmlPropertyAttribute(string selector)
        {
            this.selector = selector;
        }
    }
}
