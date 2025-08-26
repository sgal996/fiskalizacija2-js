using System;
using System.Xml.Linq;
using Fiskalizacija2.Models.Xml;
using Fiskalizacija2.Utils;

namespace Fiskalizacija2.Builders
{
    public static class UblBuilder
    {
        public static ERacun GetERacunFromUbl(string xml)
        {
            return XmlUtils.UsingXmlDocument(xml, doc =>
            {
                var root = doc.Root ?? throw new Exception("Missing root element");
                var type = root.Name.LocalName;
                if (type != "Invoice" && type != "CreditNote")
                {
                    throw new Exception($"Expected 'Invoice' or 'CreditNote' as root element, got '{type}'");
                }
                return ERacun.FromUblElement(root, type);
            });
        }

        public static Racun GetRacunFromUbl(string xml)
        {
            return XmlUtils.UsingXmlDocument(xml, doc =>
            {
                var root = doc.Root ?? throw new Exception("Missing root element");
                var type = root.Name.LocalName;
                if (type != "Invoice" && type != "CreditNote")
                {
                    throw new Exception($"Expected 'Invoice' or 'CreditNote' as root element, got '{type}'");
                }
                return Racun.FromUblElement(root, type);
            });
        }
    }
}
