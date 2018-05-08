using HtmlAgilityPack;
using System;
using System.Linq;

namespace Gnsso.Html
{
    public sealed class HtmlPropertySelector
    {
        private const string pattern = @"(?<prefix>.*{)?(?:(?<object>.+)::)?(?<value>[0-9a-zA-Z]+)(?:\.(?<valueProp>[0-9a-zA-Z-]+))?(?<suffix>}.*)?";

        private string selector;

        public HtmlPropertySelector(string selector)
        {
            this.selector = string.IsNullOrWhiteSpace(selector) ? "html.outer" : selector;
                this.selector = selector;
        }

        public string Execute(string html)
        {
            ValidationUtils.NotNull(html, nameof(html));

            return Execute(HtmlNode.CreateNode(html));
        }

        public string Execute(HtmlNode htmlNode)
        {
            if (!RegexUtils.TryMatch(selector, pattern, out var match))
                throw new ArgumentException("Property selector did not matched");

            var prefixGroup = match.Groups["prefix"];
            var suffixGroup = match.Groups["suffix"];

            var objectGroup = match.Groups["object"];
            var valueGroup = match.Groups["value"];
            if (!valueGroup.Success)
                throw new ArgumentException("Value group in property pattern did not matched");
            var valuePropGroup = match.Groups["valueProp"];
            
            if (objectGroup.Success)
                htmlNode = SelectorCache.GetOrAddObject(objectGroup.Value).Execute(htmlNode).FirstOrDefault();

            if (htmlNode == null) return null;

            string r()
            {
                switch (valueGroup.Value)
                {
                    case "text":
                        if (valuePropGroup.Success)
                            switch (valuePropGroup.Value)
                            {
                                case "decoded": return htmlNode.DecodedInnerText();
                                case "purified": return htmlNode.Purify().InnerText;
                                case "purified-decoded": return htmlNode.Purify().DecodedInnerText();
                            }
                        return htmlNode.DecodedInnerText();
                    case "html":
                        if (valuePropGroup.Success)
                            switch (valuePropGroup.Value)
                            {
                                case "inner": return htmlNode.InnerHtml;
                                case "outer": return htmlNode.OuterHtml;
                            }
                        return htmlNode.OuterHtml;
                    case "attr":
                        if (valuePropGroup.Success) return htmlNode.Attributes[valuePropGroup.Value]?.Value;
                        return null;
                    case "name":
                        return htmlNode.Name;
                    default:
                        throw new ArgumentException("Value selector must be one of 'text, html, attr, name'");
                }
            }
            if (prefixGroup.Success && suffixGroup.Success)
            {
                return prefixGroup.Value.TrimEnd('{') + r() + suffixGroup.Value.TrimStart('}');
            }
            else
            {
                return r();
            }
        }
    }
}
