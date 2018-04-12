using System;

namespace Gnsso.Html
{
    public class HtmlConverter
    {
        public static readonly HtmlConverter Default = new HtmlConverter();

        public virtual object Convert(string value, Type targetType)
        {
            var targetIsArray = targetType.IsArray;
            if (targetIsArray) targetType = targetType.GetElementType();

            var parser = ParserCache.GetOrAdd(targetType);
            if (parser != null)
            {
                var objects = parser.Parse(value);
                if (!targetIsArray) return objects[0];
                var returnArray = Array.CreateInstance(targetType, objects.Length);
                Array.Copy(objects, returnArray, objects.Length);
                return returnArray;
            }

            return ConverterUtils.Convert(value, targetType);
        }
    }
}
