using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;

namespace Fiskalizacija2.Utils
{
    public static class XmlUtils
    {
        public static string XmlEscape(string? val)
        {
            return SecurityElement.Escape(val) ?? string.Empty;
        }

        public static T UsingXmlDocument<T>(string xml, Func<XDocument, T> fn)
        {
            var doc = XDocument.Parse(xml);
            return fn(doc);
        }

        public static string GetElementContent(XElement parent, string tag)
        {
            var el = parent.Element(GetXName(parent, tag));
            if (el == null)
            {
                throw new Exception($"Element '{tag}' not found in '{parent.Name}'");
            }
            return el.Value;
        }

        public static string? GetOptionalElementContent(XElement parent, string tag)
        {
            var el = parent.Element(GetXName(parent, tag));
            return el?.Value;
        }

        public static T ExtractElement<T>(XElement parent, string tag, Func<XElement, T> fn)
        {
            var el = parent.Element(GetXName(parent, tag));
            if (el == null)
            {
                throw new Exception($"Element '{tag}' not found in '{parent.Name}'");
            }
            return fn(el);
        }

        public static List<T> ExtractElements<T>(XElement parent, string tag, Func<XElement, T> fn)
        {
            return parent.Elements(GetXName(parent, tag)).Select(fn).ToList();
        }

        public static string GetAttributeValue(XElement el, string name)
        {
            var attr = el.Attribute(name);
            if (attr == null)
            {
                throw new Exception($"Attribute '{name}' not found in element '{el.Name}'");
            }
            return attr.Value;
        }

        public static string? GetOptionalAttributeValue(XElement el, string name)
        {
            return el.Attribute(name)?.Value;
        }

        private static XName GetXName(XElement parent, string tag)
        {
            if (tag.Contains(':'))
            {
                var parts = tag.Split(':');
                var ns = parent.GetNamespaceOfPrefix(parts[0]);
                return ns + parts[1];
            }
            return tag;
        }
    }
}
