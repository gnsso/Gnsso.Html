using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gnsso.Html
{
    public static class HtmlConvert
    {
        public static string ToValue(string html, string selector)
        {
            var htmlSelector = SelectorCache.GetOrAddProperty(selector);
            return htmlSelector.Execute(html);
        }

        public static string[] ToValues(string html, string objectSelector, string propertySelector)
        {
            var htmlObjectSelector = SelectorCache.GetOrAddObject(objectSelector);
            var htmlNodes = htmlObjectSelector.Execute(html);
            var htmlPropertySelector = SelectorCache.GetOrAddProperty(propertySelector);
            return htmlNodes.Select(htmlPropertySelector.Execute).ToArray();
        }

        public static T ToSingleObject<T>(string html)
        {
            return (T)HtmlConverter.Default.Convert(html, typeof(T));
        }

        public static T[] ToObjects<T>(string html)
        {
            var objects = (T[])HtmlConverter.Default.Convert(html, typeof(T[]));
            return objects;
        }
    }
}
