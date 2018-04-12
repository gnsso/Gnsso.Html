using System;
using System.Collections.Generic;
using System.Linq;

namespace Gnsso.Html
{
    public static class HtmlNodeExtensions
    {
        /// <summary>
        /// Checks whether the element has any attributes.
        /// </summary>
        public static bool HasAnyAttribute(this HtmlAgilityPack.HtmlNode source) => source.HasAttributes;

        /// <summary>
        /// Checks whether the element has any attributes with the specified name.
        /// </summary>
        /// <param name="attributeName">Use wildcards at the beginning or the end, or both, for any value.</param>
        public static bool HasAttribute(this HtmlAgilityPack.HtmlNode source, string attributeName) => source.HasAttribute(attributeName, "*", "");

        /// <summary>
        /// Checks whether the element has any attributes with the specified name and value.
        /// </summary>
        /// <param name="attributeName">Use wildcards at the beginning or the end, or both, for any value.</param>
        /// <param name="attributeValue">Use wildcards at the beginning or the end, or both, for any value.</param>
        public static bool HasAttribute(this HtmlAgilityPack.HtmlNode source, string attributeName, string attributeValue)
        {
            return source.HasAttribute(attributeName, attributeValue, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <param name="attributeValueSelectorOperator"></param>
        /// <returns></returns>
        public static bool HasAttribute(this HtmlAgilityPack.HtmlNode source, string attributeName, string attributeValue, string attributeValueSelectorOperator)
        {
            return source.Attributes.Any(a => a.Name == attributeName) && predicate(source.Attributes[attributeName].Value, attributeValue, attributeValueSelectorOperator);

            bool predicate(string avr, string avi, string op)
            {
                if (avi == "*") return true;
                switch (op)
                {
                    case "|": return avr.Contains(avi);
                    case "!": return avr != avi;
                    case "^": return avr.StartsWith(avi);
                    case "~": return avr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Contains(avi);
                    default: return avr == avi;
                }
            }
        }
        
        /// <summary>
        /// Checks whether the element has id attribute.
        /// </summary>
        public static bool HasIdAttribute(this HtmlAgilityPack.HtmlNode source) => source.HasIdAttribute("*");
        
        /// <summary>
        /// Checks whether the element has id attribute with the specific value.
        /// </summary>
        /// <param name="value">Use wildcards at the beginning or the end, or both, for any value.</param>
        public static bool HasIdAttribute(this HtmlAgilityPack.HtmlNode source, string value) => source.HasAttribute("id", value);
        
        /// <summary>
        /// Checks whether the element has class attribute.
        /// </summary>
        public static bool HasClassAttribute(this HtmlAgilityPack.HtmlNode source) => source.HasClassAttribute("*");
        
        /// <summary>
        /// Checks whether the element has class attribute with the specific value.
        /// </summary>
        /// <param name="@class">Use wildcards at the beginning or the end, or both, for any value.</param>
        public static bool HasClassAttribute(this HtmlAgilityPack.HtmlNode source, string value) => source.HasAttribute("class", value);
        
        /// <summary>
        /// Returns a collection of all descendant nodes that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlNode> SelectDescendants(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, false).Where(w => w.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Returns a collection of all descendant nodes that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlNode> SelectDescendants(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, false).Where(w => w.HasAttribute(attributeName, attributeValue));
        }
        
        /// <summary>
        /// Gets the collection of attributes of matching elements.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlAttribute> SelectDescendantsAttributes(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, false).Where(w => w.HasAttribute(attributeName)).Select(s => s.Attributes[attributeName]);

        }
        
        /// <summary>
        /// Gets the collection of attributes of matching elements.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlAttribute> SelectDescendantsAttributes(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, false).Where(w => w.HasAttribute(attributeName, attributeValue)).Select(s => s.Attributes[attributeName]);
        }
        
        /// <summary>
        /// Gets the single descendant node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectSingleDescendant(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, false).SingleOrDefault(s => s.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Gets the single descendant node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectSingleDescendant(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, false).SingleOrDefault(s => s.HasAttribute(attributeName, attributeValue));
        }
        
        /// <summary>
        /// Gets the single attribute of descendant node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectSingleDescendantAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, false).SingleOrDefault(s => s.HasAttribute(attributeName))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the single attribute of descendant node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectSingleDescendantAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, false).SingleOrDefault(s => s.HasAttribute(attributeName, attributeValue))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the first descendant node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectFirstDescendant(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, false).FirstOrDefault(s => s.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Gets the first descendant node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectFirstDescendant(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, false).FirstOrDefault(s => s.HasAttribute(attributeName, attributeValue));
        }
        
        /// <summary>
        /// Gets the first attribute of descendant node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectFirstDescendantAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, false).FirstOrDefault(s => s.HasAttribute(attributeName))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the first attribute of descendant node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectFirstDescendantAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, false).FirstOrDefault(s => s.HasAttribute(attributeName, attributeValue))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Returns a collection of all descendant nodes and self that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlNode> SelectDescendantsWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, true).Where(w => w.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Returns a collection of all descendant nodes and self that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlNode> SelectDescendantsWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, true).Where(w => w.HasAttribute(attributeName, attributeValue));
        }
        
        /// <summary>
        /// Gets the collection of attributes of matching elements.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlAttribute> SelectDescendantsAttributesWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, true).Where(w => w.HasAttribute(attributeName)).Select(s => s.Attributes[attributeName]);
        }
        
        /// <summary>
        /// Gets the collection of attributes of matching elements.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlAttribute> SelectDescendantsAttributesWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, true).Where(w => w.HasAttribute(attributeName, attributeValue)).Select(s => s.Attributes[attributeName]);
        }
        
        /// <summary>
        /// Gets the single descendant node or self that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectSingleDescendantWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, true).SingleOrDefault(s => s.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Gets the single descendant node or self that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectSingleDescendantWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, true).SingleOrDefault(s => s.HasAttribute(attributeName, attributeValue));
        }
        
        /// <summary>
        /// Gets the single attribute of descendant node or self that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectSingleDescendantAttributeWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, true).SingleOrDefault(s => s.HasAttribute(attributeName))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the single attribute of descendant node or self that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectSingleDescendantAttributeWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, true).SingleOrDefault(s => s.HasAttribute(attributeName, attributeValue))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the first descendant node or self that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectFirstDescendantWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, true).FirstOrDefault(s => s.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Gets the first descendant node or self that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectFirstDescendantWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, true).FirstOrDefault(s => s.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Gets the first attribute of descendant node or self that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectFirstDescendantAttributeWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.DescendantsSelector(elementName, true).FirstOrDefault(s => s.HasAttribute(attributeName))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the first attribute of descendant node or self that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectFirstDescendantAttributeWithSelf(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.DescendantsSelector(elementName, true).FirstOrDefault(s => s.HasAttribute(attributeName, attributeValue))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Returns a collection of all children nodes by given name of this element.
        /// </summary>
        public static IEnumerable<HtmlAgilityPack.HtmlNode> SelectChildren(this HtmlAgilityPack.HtmlNode source, string elementName)
        {
            return source.ChildrenSelector(elementName);
        }
        
        /// <summary>
        /// Returns a collection of all children nodes that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlNode> SelectChildren(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.ChildrenSelector(elementName).Where(w => w.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Returns a collection of all children nodes that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlNode> SelectChildren(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.ChildrenSelector(elementName).Where(w => w.HasAttribute(attributeName, attributeValue));
        
        }
        
        /// <summary>
        /// Gets the collection of attributes of matching elements.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlAttribute> SelectChildrenAttributes(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.ChildrenSelector(elementName).Where(w => w.HasAttribute(attributeName)).Select(s => s.Attributes[attributeName]);
        }
        
        /// <summary>
        /// Gets the collection of attributes of matching elements.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to match all elements</param>
        public static IEnumerable<HtmlAgilityPack.HtmlAttribute> SelectChildrenAttributes(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.ChildrenSelector(elementName).Where(w => w.HasAttribute(attributeName, attributeValue)).Select(s => s.Attributes[attributeName]);
        }
        
        /// <summary>
        /// Gets the single child node by given name of this element.
        /// </summary>
        public static HtmlAgilityPack.HtmlNode SelectSingleChild(this HtmlAgilityPack.HtmlNode source, string elementName)
        {
            return source.ChildrenSelector(elementName).SingleOrDefault();
        }
        
        /// <summary>
        /// Gets the single child node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectSingleChild(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.ChildrenSelector(elementName).SingleOrDefault(s => s.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Gets the single child node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectSingleChild(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.ChildrenSelector(elementName).SingleOrDefault(s => s.HasAttribute(attributeName, attributeValue));
        }
        
        /// <summary>
        /// Gets the single attribute of child node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectSingleChildAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.ChildrenSelector(elementName).SingleOrDefault(s => s.HasAttribute(attributeName))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the single attribute of child node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectSingleChildAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.ChildrenSelector(elementName).SingleOrDefault(s => s.HasAttribute(attributeName, attributeValue))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the first child node by given name of this element.
        /// </summary>
        public static HtmlAgilityPack.HtmlNode SelectFirstChild(this HtmlAgilityPack.HtmlNode source, string elementName)
        {
            return source.ChildrenSelector(elementName).FirstOrDefault();
        }
        
        /// <summary>
        /// Gets the first child node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectFirstChild(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.ChildrenSelector(elementName).FirstOrDefault(s => s.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Gets the first child node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectFirstChild(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.ChildrenSelector(elementName).FirstOrDefault(s => s.HasAttribute(attributeName, attributeValue));
        }
        
        /// <summary>
        /// Gets the first attribute of child node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectFirstChildAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.ChildrenSelector(elementName).FirstOrDefault(s => s.HasAttribute(attributeName))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the first attribute of child node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectFirstChildAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.ChildrenSelector(elementName).FirstOrDefault(s => s.HasAttribute(attributeName, attributeValue))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the last child node by given name of this element.
        /// </summary>
        public static HtmlAgilityPack.HtmlNode SelectLastChild(this HtmlAgilityPack.HtmlNode source, string elementName)
        {
            return source.ChildrenSelector(elementName).LastOrDefault();
        }
        
        /// <summary>
        /// Gets the last child node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectLastChild(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.ChildrenSelector(elementName).LastOrDefault(s => s.HasAttribute(attributeName));
        }
        
        /// <summary>
        /// Gets the last child node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlNode SelectLastChild(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.ChildrenSelector(elementName).LastOrDefault(s => s.HasAttribute(attributeName, attributeValue));
        }
        
        /// <summary>
        /// Gets the last attribute of child node that have attribute by given name of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectLastChildAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName)
        {
            return source.ChildrenSelector(elementName).LastOrDefault(s => s.HasAttribute(attributeName))?.Attributes[attributeName];
        }
        
        /// <summary>
        /// Gets the last attribute of child node that have attribute by given name and value of this element.
        /// </summary>
        /// <param name="elementName">Use wildcards(*) to search in all elements</param>
        public static HtmlAgilityPack.HtmlAttribute SelectLastChildAttribute(this HtmlAgilityPack.HtmlNode source, string elementName, string attributeName, string attributeValue)
        {
            return source.ChildrenSelector(elementName).FirstOrDefault(s => s.HasAttribute(attributeName, attributeValue))?.Attributes[attributeName];
        }

        private static IEnumerable<HtmlAgilityPack.HtmlNode> DescendantsSelector(this HtmlAgilityPack.HtmlNode source, string elementName, bool self)
        {
            if (self)
                return elementName == "*" ? source.DescendantsAndSelf() : source.DescendantsAndSelf(elementName);
            else
                return elementName == "*" ? source.Descendants() : source.Descendants(elementName);
        }

        private static IEnumerable<HtmlAgilityPack.HtmlNode> ChildrenSelector(this HtmlAgilityPack.HtmlNode source, string elementName)
        {
            return elementName == "*" ? source.ChildNodes : source.ChildNodes.Where(w => w.Name == elementName);
        }

        public static string DecodedInnerText(this HtmlAgilityPack.HtmlNode source)
        {
            return System.Net.WebUtility.HtmlDecode(source.InnerText.Trim())?.Trim();
        }

        public static HtmlAgilityPack.HtmlNode Purify(this HtmlAgilityPack.HtmlNode source)
        {
            var clone = source.Clone();
            var textNodes = clone.ChildNodes.Where(w => w.NodeType == HtmlAgilityPack.HtmlNodeType.Text).ToArray();
            clone.RemoveAllChildren();
            foreach (var textNode in textNodes)
                clone.AppendChild(textNode);
            return clone;
        }

        public static void FixLinks(this HtmlAgilityPack.HtmlNode source, string baseUrl)
        {
            foreach (var attribute in source.SelectDescendantsAttributes("img", "src"))
                try { attribute.Value = new Uri(new Uri(baseUrl), attribute.Value).AbsoluteUri; } catch { }
            foreach (var attribute in source.SelectDescendantsAttributes("*", "href"))
                try { attribute.Value = new Uri(new Uri(baseUrl), attribute.Value).AbsoluteUri; } catch { }
        }
    }
}
