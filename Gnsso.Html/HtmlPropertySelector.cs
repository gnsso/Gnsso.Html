using HtmlAgilityPack;
using System;
using System.Linq;

namespace Gnsso.Html
{
    internal sealed class HtmlPropertySelector
    {
        private const string pattern = @"(?<f>.*{)?(?:(?<e>.+)::)?(?<k>[0-9a-zA-Z]+)(?:\.(?<a>[0-9a-zA-Z-]+))?(?<l>}.*)?";

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

        internal string Execute(HtmlNode htmlNode)
        {
            if (!RegexUtils.TryMatch(selector, pattern, out var match))
                throw new InvalidOperationException();

            var fg = match.Groups["f"];
            var lg = match.Groups["l"];

            var eg = match.Groups["e"];
            var kg = match.Groups["k"];
            if (!kg.Success) throw new InvalidOperationException();
            var ag = match.Groups["a"];
            
            if (eg.Success)
                htmlNode = SelectorCache.GetOrAddObject(eg.Value).Execute(htmlNode).FirstOrDefault();

            if (htmlNode == null) return null;

            string r()
            {
                switch (kg.Value)
                {
                    case "text":
                        if (ag.Success)
                            switch (ag.Value)
                            {
                                case "decoded": return htmlNode.DecodedInnerText();
                                case "purified": return htmlNode.Purify().InnerText;
                                case "purified-decoded": return htmlNode.Purify().DecodedInnerText();
                            }
                        return htmlNode.InnerText;
                    case "html":
                        if (ag.Success)
                            switch (ag.Value)
                            {
                                case "inner": return htmlNode.InnerHtml;
                                case "outer": return htmlNode.OuterHtml;
                            }
                        return htmlNode.OuterHtml;
                    case "attr":
                        if (ag.Success) return htmlNode.Attributes[ag.Value]?.Value;
                        return null;
                    case "name":
                        return htmlNode.Name;
                    default:
                        throw new ArgumentException();
                }
            }

            return (fg.Success ? fg.Value.TrimEnd('{') : "") + r() + (lg.Success ? lg.Value.TrimStart('}') : "");
        }
    }
}
