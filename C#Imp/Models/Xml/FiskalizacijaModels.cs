using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Fiskalizacija2.Utils;

namespace Fiskalizacija2.Models.Xml
{
    public class ZaglavljeFiskalizacija
    {
        public string DatumVrijemeSlanja { get; set; } = string.Empty;
        public string VrstaERacuna { get; set; } = string.Empty;

        public string ToXmlString()
        {
            var res = "<efis:Zaglavlje>";
            res += $"<efis:datumVrijemeSlanja>{XmlUtils.XmlEscape(DatumVrijemeSlanja)}</efis:datumVrijemeSlanja>";
            res += $"<efis:vrstaERacuna>{XmlUtils.XmlEscape(VrstaERacuna)}</efis:vrstaERacuna>";
            res += "</efis:Zaglavlje>";
            return res;
        }

        public static ZaglavljeFiskalizacija FromXmlElement(XElement el)
        {
            return new ZaglavljeFiskalizacija
            {
                DatumVrijemeSlanja = XmlUtils.GetElementContent(el, "efis:datumVrijemeSlanja"),
                VrstaERacuna = XmlUtils.GetElementContent(el, "efis:vrstaERacuna")
            };
        }
    }

    public class ERacun
    {
        public string BrojDokumenta { get; set; } = string.Empty;
        public string DatumIzdavanja { get; set; } = string.Empty;
        public string VrstaDokumenta { get; set; } = string.Empty;
        public string ValutaERacuna { get; set; } = string.Empty;
        public bool IndikatorKopije { get; set; }

        public string ToXmlString()
        {
            var res = "<efis:ERacun>";
            res += $"<efis:brojDokumenta>{XmlUtils.XmlEscape(BrojDokumenta)}</efis:brojDokumenta>";
            res += $"<efis:datumIzdavanja>{XmlUtils.XmlEscape(DatumIzdavanja)}</efis:datumIzdavanja>";
            res += $"<efis:vrstaDokumenta>{XmlUtils.XmlEscape(VrstaDokumenta)}</efis:vrstaDokumenta>";
            res += $"<efis:valutaERacuna>{XmlUtils.XmlEscape(ValutaERacuna)}</efis:valutaERacuna>";
            res += $"<efis:indikatorKopije>{(IndikatorKopije ? "true" : "false")}</efis:indikatorKopije>";
            res += "</efis:ERacun>";
            return res;
        }

        public static ERacun FromUblElement(XElement el, string type)
        {
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            return new ERacun
            {
                BrojDokumenta = el.Element(cbc + "ID")?.Value ?? string.Empty,
                DatumIzdavanja = el.Element(cbc + "IssueDate")?.Value ?? string.Empty,
                VrstaDokumenta = type,
                ValutaERacuna = el.Element(cbc + "DocumentCurrencyCode")?.Value ?? string.Empty,
                IndikatorKopije = false
            };
        }
    }

    public class Racun
    {
        public string BrojDokumenta { get; set; } = string.Empty;

        public string ToXmlString()
        {
            var res = "<efis:Racun>";
            res += $"<efis:brojDokumenta>{XmlUtils.XmlEscape(BrojDokumenta)}</efis:brojDokumenta>";
            res += "</efis:Racun>";
            return res;
        }

        public static Racun FromUblElement(XElement el, string type)
        {
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            return new Racun
            {
                BrojDokumenta = el.Element(cbc + "ID")?.Value ?? string.Empty
            };
        }
    }

    public class EvidentirajERacunZahtjev : SerializableRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ZaglavljeFiskalizacija Zaglavlje { get; set; } = new();
        public List<ERacun> ERacun { get; set; } = new();

        public string ToXmlString()
        {
            var res = $"<efis:EvidentirajERacunZahtjev efis:id=\"{XmlUtils.XmlEscape(Id)}\">";
            res += Zaglavlje.ToXmlString();
            foreach (var e in ERacun)
            {
                res += e.ToXmlString();
            }
            res += "</efis:EvidentirajERacunZahtjev>";
            return res;
        }
    }

    public class EvidentirajIsporukuZaKojuNijeIzdanERacunZahtjev : SerializableRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ZaglavljeFiskalizacija Zaglavlje { get; set; } = new();
        public List<Racun> Racun { get; set; } = new();

        public string ToXmlString()
        {
            var res = $"<efis:EvidentirajIsporukuZaKojuNijeIzdanERacunZahtjev efis:id=\"{XmlUtils.XmlEscape(Id)}\">";
            res += Zaglavlje.ToXmlString();
            foreach (var r in Racun)
            {

                // Placeholder - racun serialization not implemented

            }
            res += "</efis:EvidentirajIsporukuZaKojuNijeIzdanERacunZahtjev>";
            return res;
        }
    }

    public class EvidentirajNaplatuZahtjev : SerializableRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ZaglavljeFiskalizacija Zaglavlje { get; set; } = new();
        public List<Naplata> Naplata { get; set; } = new();

        public string ToXmlString()
        {
            var res = $"<efis:EvidentirajNaplatuZahtjev efis:id=\"{XmlUtils.XmlEscape(Id)}\">";
            res += Zaglavlje.ToXmlString();
            foreach (var n in Naplata)
            {
                res += n.ToXmlString();
            }

            res += "</efis:EvidentirajNaplatuZahtjev>";
            return res;
        }
    }

    public class EvidentirajOdbijanjeZahtjev : SerializableRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ZaglavljeFiskalizacija Zaglavlje { get; set; } = new();
        public List<Odbijanje> Odbijanje { get; set; } = new();

        public string ToXmlString()
        {
            var res = $"<efis:EvidentirajOdbijanjeZahtjev efis:id=\"{XmlUtils.XmlEscape(Id)}\">";
            res += Zaglavlje.ToXmlString();

            foreach (var o in Odbijanje)
            {
                res += o.ToXmlString();
            }

            res += "</efis:EvidentirajOdbijanjeZahtjev>";
            return res;
        }
    }

    public class Naplata
    {
        public string BrojDokumenta { get; set; } = string.Empty;
        public string DatumIzdavanja { get; set; } = string.Empty;
        public string OibPorezniBrojIzdavatelja { get; set; } = string.Empty;
        public string OibPorezniBrojPrimatelja { get; set; } = string.Empty;
        public string DatumNaplate { get; set; } = string.Empty;
        public decimal NaplaceniIznos { get; set; }
        public string NacinPlacanja { get; set; } = string.Empty;

        public string ToXmlString()
        {
            var res = "<efis:Naplata>";
            res += $"<efis:brojDokumenta>{XmlUtils.XmlEscape(BrojDokumenta)}</efis:brojDokumenta>";
            res += $"<efis:datumIzdavanja>{XmlUtils.XmlEscape(DatumIzdavanja)}</efis:datumIzdavanja>";
            res += $"<efis:oibPorezniBrojIzdavatelja>{XmlUtils.XmlEscape(OibPorezniBrojIzdavatelja)}</efis:oibPorezniBrojIzdavatelja>";
            res += $"<efis:oibPorezniBrojPrimatelja>{XmlUtils.XmlEscape(OibPorezniBrojPrimatelja)}</efis:oibPorezniBrojPrimatelja>";
            res += $"<efis:datumNaplate>{XmlUtils.XmlEscape(DatumNaplate)}</efis:datumNaplate>";
            res += $"<efis:naplaceniIznos>{NaplaceniIznos:F2}</efis:naplaceniIznos>";
            res += $"<efis:nacinPlacanja>{XmlUtils.XmlEscape(NacinPlacanja)}</efis:nacinPlacanja>";
            res += "</efis:Naplata>";
            return res;
        }
    }

    public class Odbijanje
    {
        public string BrojDokumenta { get; set; } = string.Empty;
        public string DatumIzdavanja { get; set; } = string.Empty;
        public string OibPorezniBrojIzdavatelja { get; set; } = string.Empty;
        public string OibPorezniBrojPrimatelja { get; set; } = string.Empty;
        public string DatumOdbijanja { get; set; } = string.Empty;
        public string VrstaRazlogaOdbijanja { get; set; } = string.Empty;
        public string RazlogOdbijanja { get; set; } = string.Empty;

        public string ToXmlString()
        {
            var res = "<efis:Odbijanje>";
            res += $"<efis:brojDokumenta>{XmlUtils.XmlEscape(BrojDokumenta)}</efis:brojDokumenta>";
            res += $"<efis:datumIzdavanja>{XmlUtils.XmlEscape(DatumIzdavanja)}</efis:datumIzdavanja>";
            res += $"<efis:oibPorezniBrojIzdavatelja>{XmlUtils.XmlEscape(OibPorezniBrojIzdavatelja)}</efis:oibPorezniBrojIzdavatelja>";
            res += $"<efis:oibPorezniBrojPrimatelja>{XmlUtils.XmlEscape(OibPorezniBrojPrimatelja)}</efis:oibPorezniBrojPrimatelja>";
            res += $"<efis:datumOdbijanja>{XmlUtils.XmlEscape(DatumOdbijanja)}</efis:datumOdbijanja>";
            res += $"<efis:vrstaRazlogaOdbijanja>{XmlUtils.XmlEscape(VrstaRazlogaOdbijanja)}</efis:vrstaRazlogaOdbijanja>";
            res += $"<efis:razlogOdbijanja>{XmlUtils.XmlEscape(RazlogOdbijanja)}</efis:razlogOdbijanja>";
            res += "</efis:Odbijanje>";
            return res;
        }
    }
    public class Naplata { }
    public class Odbijanje { }

}
