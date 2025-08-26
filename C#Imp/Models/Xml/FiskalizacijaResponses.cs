using System;
using System.Xml.Linq;
using Fiskalizacija2.Utils;

namespace Fiskalizacija2.Models.Xml
{
    internal static class OdgovorParser
    {
        public static OdgovorContent Parse(XElement el)
        {
            var ns = el.Name.Namespace;
            var odgovor = new OdgovorContent
            {
                IdZahtjeva = el.Element(ns + "idZahtjeva")?.Value ?? string.Empty,
                PrihvacenZahtjev = (el.Element(ns + "prihvacenZahtjev")?.Value ?? string.Empty)
                    .Equals("true", StringComparison.OrdinalIgnoreCase)
            };
            var greskaEl = el.Element(ns + "Greska");
            if (greskaEl != null)
            {
                odgovor.Greska = new GreskaContent
                {
                    Sifra = greskaEl.Element(ns + "sifra")?.Value ?? string.Empty,
                    RedniBrojZapisa = greskaEl.Element(ns + "redniBrojZapisa")?.Value ?? string.Empty,
                    Opis = greskaEl.Element(ns + "opis")?.Value ?? string.Empty
                };
            }
            return odgovor;
        }
    }

    public class EvidentirajERacunOdgovor : ParsedResponse
    {
        public string Id { get; set; } = string.Empty;
        public string DatumVrijemeSlanja { get; set; } = string.Empty;
        public OdgovorContent Odgovor { get; set; } = new();

        public static EvidentirajERacunOdgovor FromXmlElement(XElement el)
        {
            var ns = el.Name.Namespace;
            return new EvidentirajERacunOdgovor
            {
                Id = el.Attribute(ns + "id")?.Value ?? string.Empty,
                DatumVrijemeSlanja = el.Element(ns + "datumVrijemeSlanja")?.Value ?? string.Empty,
                Odgovor = OdgovorParser.Parse(el.Element(ns + "Odgovor")!)
            };
        }
    }

    public class EvidentirajNaplatuOdgovor : ParsedResponse
    {
        public string Id { get; set; } = string.Empty;
        public string DatumVrijemeSlanja { get; set; } = string.Empty;
        public OdgovorContent Odgovor { get; set; } = new();

        public static EvidentirajNaplatuOdgovor FromXmlElement(XElement el)
        {
            var ns = el.Name.Namespace;
            return new EvidentirajNaplatuOdgovor
            {
                Id = el.Attribute(ns + "id")?.Value ?? string.Empty,
                DatumVrijemeSlanja = el.Element(ns + "datumVrijemeSlanja")?.Value ?? string.Empty,
                Odgovor = OdgovorParser.Parse(el.Element(ns + "Odgovor")!)
            };
        }
    }

    public class EvidentirajOdbijanjeOdgovor : ParsedResponse
    {
        public string Id { get; set; } = string.Empty;
        public string DatumVrijemeSlanja { get; set; } = string.Empty;
        public OdgovorContent Odgovor { get; set; } = new();

        public static EvidentirajOdbijanjeOdgovor FromXmlElement(XElement el)
        {
            var ns = el.Name.Namespace;
            return new EvidentirajOdbijanjeOdgovor
            {
                Id = el.Attribute(ns + "id")?.Value ?? string.Empty,
                DatumVrijemeSlanja = el.Element(ns + "datumVrijemeSlanja")?.Value ?? string.Empty,
                Odgovor = OdgovorParser.Parse(el.Element(ns + "Odgovor")!)
            };
        }
    }

    public class EvidentirajIsporukuZaKojuNijeIzdanERacunOdgovor : ParsedResponse
    {
        public string Id { get; set; } = string.Empty;
        public string DatumVrijemeSlanja { get; set; } = string.Empty;
        public OdgovorContent Odgovor { get; set; } = new();

        public static EvidentirajIsporukuZaKojuNijeIzdanERacunOdgovor FromXmlElement(XElement el)
        {
            var ns = el.Name.Namespace;
            return new EvidentirajIsporukuZaKojuNijeIzdanERacunOdgovor
            {
                Id = el.Attribute(ns + "id")?.Value ?? string.Empty,
                DatumVrijemeSlanja = el.Element(ns + "datumVrijemeSlanja")?.Value ?? string.Empty,
                Odgovor = OdgovorParser.Parse(el.Element(ns + "Odgovor")!)
            };
        }
    }
}
