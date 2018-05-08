using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gnsso.Html
{
    public class HtmlObjectSelector
    {
        private const string nodePattern = @"(?<descendant><*>{1,2})[\s]*?(?<tag>[a-zA-Z][a-zA-Z0-9-]*|\*)(?<attributes>" + attributesPattern + @"*)?(?<indexer>\[(?:(?:reverse)|(?:reverse,)?\d{1,3}(?:\.\.\d{1,3})?)\])?";
        private const string attributesPattern = @"(?:#[a-zA-Z0-9-_]+|\.[a-zA-Z0-9-_]+|@[a-zA-Z][a-zA-Z0-9-]*(?:[|^!~]?=[a-zA-Z0-9-_]+)?)";
        private const string attributePattern = @"@(?<name>[a-zA-Z][a-zA-Z0-9-]*)(?:(?<operator>[|^!~])?=(?<value>[a-zA-Z0-9-_]+)?)?";
        private const string indexerPattern = @"\[((?<reverseonly>r)|(?<reverse>r,)?(?<start>\d{1,3})(?<count>\.\.\d{1,3})?)\]";
        private const string valuePattern = @"(?:(?<element>.*)::)?(?<key>[a-zA-Z0-9]+)(?:\.(?<argument>[a-zA-Z0-9]+))?";

        private string selector;
        private bool noSelector;

        public HtmlObjectSelector(string selector)
        {
            if (!(noSelector = string.IsNullOrWhiteSpace(selector)))
                this.selector = (selector.StartsWith(">") ? selector : ">>" + selector).TrimStart('<');
        }

        public IEnumerable<HtmlNode> Execute(string html)
        {
            ValidationUtils.NotNull(html, nameof(html));

            return Execute(HtmlNode.CreateNode(html));
        }

        public IEnumerable<HtmlNode> Execute(HtmlNode node)
        {
            return Execute(new[] { node }.AsEnumerable());
        }

        public IEnumerable<HtmlNode> Execute(IEnumerable<HtmlNode> elements)
        {
            if (noSelector) return elements;

            var matches = Regex.Matches(selector, nodePattern);
            foreach (Match match in matches)
            {
                var descendantGroup = match.Groups["descendant"];
                if (!descendantGroup.Success) throw new ArgumentException("Descendant group in node pattern did not matched");
                var tagGroup = match.Groups["tag"];
                if (!tagGroup.Success) throw new ArgumentException("Tag group in node pattern did not matched");
                ExecuteDescendant(descendantGroup.Value, ref elements);
                ExecuteTag(tagGroup.Value, ref elements);
                var attributesGroup = match.Groups["attributes"];
                if (attributesGroup.Success) ExecuteAttributes(attributesGroup.Value, ref elements);
                var indexerGroup = match.Groups["indexer"];
                if (indexerGroup.Success) ExecuteIndexer(indexerGroup.Value, ref elements);
            }
            return elements;
        }

        private void ExecuteDescendant(string descendantSelector, ref IEnumerable<HtmlNode> elements)
        {
            while (descendantSelector.StartsWith("<"))
            {
                elements = elements.Select(s => s.ParentNode ?? s).Distinct();
                descendantSelector = descendantSelector.Substring(1);
            }
            if (descendantSelector == ">>") elements = elements.SelectMany(s => s.Descendants());
            else if (descendantSelector == ">") elements = elements.SelectMany(s => s.ChildNodes);
            else throw new ArgumentException($"Descendant selector '{descendantSelector}' is not valid");
        }

        private void ExecuteTag(string tagSelector, ref IEnumerable<HtmlNode> elements)
        {
            if (tagSelector != "*") elements = elements.Where(w => w.Name == tagSelector);
        }

        private void ExecuteAttributes(string attributesSelector, ref IEnumerable<HtmlNode> elements)
        {
            var matches = Regex.Matches(attributesSelector, attributesPattern);
            var fixedAttributeSelectors = new List<string>();
            foreach (Match match in matches)
            {
                var value = match.Value;
                if (value[0] == '.') fixedAttributeSelectors.Add($"@class~={value.Substring(1)}");
                else if (value[0] == '#') fixedAttributeSelectors.Add($"@id={value.Substring(1)}");
                else fixedAttributeSelectors.Add(value);
            }
            foreach (var attributeSelector in fixedAttributeSelectors)
            {
                var match = Regex.Match(attributeSelector, attributePattern);
                var nameGroup = match.Groups["name"];
                if (!nameGroup.Success) throw new ArgumentException("Name group in attribute pattern did not matched");
                var valueGroup = match.Groups["value"];
                if (valueGroup.Success)
                {
                    var operatorGroup = match.Groups["operator"];
                    if (operatorGroup.Success)
                    {
                        elements = elements.Where(w => w.HasAttribute(nameGroup.Value, valueGroup.Value, operatorGroup.Value));
                    }
                    else
                    {
                        elements = elements.Where(w => w.HasAttribute(nameGroup.Value, valueGroup.Value));
                    }
                }
                else
                {
                    elements = elements.Where(w => w.HasAttribute(nameGroup.Value));
                }
            }
        }

        private void ExecuteIndexer(string indexerSelector, ref IEnumerable<HtmlNode> elements)
        {
            var match = Regex.Match(indexerSelector, indexerPattern);
            if (!match.Success) throw new ArgumentException("Indexer selector did not matched");
            var reverseonlyGroup = match.Groups["reverseonly"];
            if (reverseonlyGroup.Success)
            {
                elements = elements.Reverse();
                return;
            }
            var startGroup = match.Groups["start"];
            if (!startGroup.Success) throw new ArgumentException("Start group in indexer pattern did not matched");
            int start = int.Parse(startGroup.Value);
            int count = 1;
            var countGroup = match.Groups["count"];
            if (countGroup.Success) count = int.Parse(countGroup.Value.TrimStart('.'));
            var reverseGroup = match.Groups["reverse"];
            if (reverseGroup.Success) elements = elements.Reverse();
            elements = elements.Skip(start).Take(count);
        }
    }
}
