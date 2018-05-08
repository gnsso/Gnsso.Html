using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Gnsso.Html
{
    public class HtmlParser<TObject>
    {
        private string objectSelector;
        private Func<TObject> objectInitializer;
        private List<Action<HtmlNode, TObject>> propertySetters;

        public HtmlParser(string objectSelector) : this(objectSelector, Expression.Lambda<Func<TObject>>(Expression.New(typeof(TObject))).Compile())
        {
            
        }

        public HtmlParser(string objectSelector, Func<TObject> objectInitializer)
        {
            this.objectSelector = objectSelector;
            this.objectInitializer = objectInitializer;
            propertySetters = new List<Action<HtmlNode, TObject>>();
        }

        public void Set(Expression<Func<TObject, string>> expression, string propertySelector)
        {
            SetInternal(GetPropertyInfo(expression), propertySelector);
        }

        public void Set<TProperty>(Expression<Func<TObject, TProperty>> expression, string propertySelector, Func<string, TProperty> propertyValueParser)
        {
            SetInternal(GetPropertyInfo(expression), propertySelector, propertyValueParser);
        }

        public void SetValue<TProperty>(Expression<Func<TObject, TProperty>> expression, TProperty value)
        {
            AddSetter((htmlNode, currentObject) =>
            {
                var propertyInfo = GetPropertyInfo(expression);
                propertyInfo.SetValue(currentObject, value);
            });
        }

        protected internal void SetInternal(PropertyInfo propertyInfo, string propertySelector)
        {
            SetInternal(propertyInfo, propertySelector, s => s);
        }

        protected internal void SetInternal<TProperty>(PropertyInfo propertyInfo, string propertySelector, Func<string, TProperty> propertyValueParser)
        {
            AddSetter((htmlNode, currentObject) =>
            {
                var htmlPropertySelector = SelectorCache.GetOrAddProperty(propertySelector);
                var executedPropertyValue = htmlPropertySelector.Execute(htmlNode);
                var parsedPropertyValue = propertyValueParser(executedPropertyValue);
                propertyInfo.SetValue(currentObject, parsedPropertyValue);
            });
        }

        protected internal void SetValueInternal<TProperty>(PropertyInfo propertyInfo, TProperty value)
        {
            AddSetter((htmlNode, currentObject) =>
            {
                propertyInfo.SetValue(currentObject, value);
            });
        }

        protected void AddSetter(Action<HtmlNode, TObject> propertySetter)
        {
            propertySetters.Add(propertySetter);
        }
        
        public TObject ParseSingle(string html)
        {
            return ParseInternal(HtmlNode.CreateNode(html), 1).Single();
        }

        public TObject[] Parse(string html)
        {
            return ParseInternal(HtmlNode.CreateNode(html));
        }

        internal TObject[] ParseInternal(HtmlNode htmlNode, int? count = null)
        {
            var returnObjects = new List<TObject>();
            var htmlObjectSelector = SelectorCache.GetOrAddObject(objectSelector);
            var nodes = htmlObjectSelector.Execute(htmlNode);
            foreach (var node in nodes)
            {
                var obj = objectInitializer();
                foreach (var propertySetter in propertySetters)
                {
                    propertySetter(node, obj);
                }
                returnObjects.Add(obj);
                if (count.HasValue && returnObjects.Count == count.Value) break;
            }
            return returnObjects.ToArray();
        }

        private PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            var body = expression.Body as MemberExpression;
            if (body == null) throw new ArgumentNullException(nameof(body));
            var propertyInfo = body.Member as PropertyInfo;
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
            return propertyInfo;
        }
    }

    public sealed class HtmlParser : HtmlParser<object>
    {
        private Type objType;

        public HtmlParser(Type objType, string objSelector) : this(objType, objSelector, Expression.Lambda<Func<object>>(Expression.New(objType)).Compile())
        {
            
        }

        public HtmlParser(Type objType, string objSelector, Func<object> objInitializer) : base(objSelector, objInitializer)
        {
            this.objType = objType;
        }

        public void Set(string propertyName, string propertySelector)
        {
            SetInternal(PropertyCache.GetOrAdd(objType, propertyName), propertySelector);
        }

        public void Set<TProperty>(string propName, string propSelector, Func<string, TProperty> propValueParser)
        {
            SetInternal(PropertyCache.GetOrAdd(objType, propName), propSelector, propValueParser);
        }

        public void SetValue<TProperty>(string propName, TProperty value)
        {
            SetValueInternal(PropertyCache.GetOrAdd(objType, propName), value);
        }

        public static HtmlParser Create(Type objectType, string objectSelector)
        {
            return new HtmlParser(objectType, objectSelector);
        }

        public static HtmlParser Create(Type objectType, string objectSelector, Func<object> objectInitializer)
        {
            return new HtmlParser(objectType, objectSelector, objectInitializer);
        }

        public static HtmlParser<T> Create<T>(string objectSelector)
        {
            return new HtmlParser<T>(objectSelector);
        }

        public static HtmlParser<T> Create<T>(string objectSelector, Func<T> objectInitializer)
        {
            return new HtmlParser<T>(objectSelector, objectInitializer);
        }
    }
}
