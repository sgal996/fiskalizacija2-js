using System;
using System.Collections.Generic;
using System.Linq;
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
        public string? DatumDospijecaPlacanja { get; set; }
        public string? DatumIsporuke { get; set; }
        public string VrstaPoslovnogProcesa { get; set; } = string.Empty;
        public string? ReferencaNaUgovor { get; set; }
        public List<PrethodniERacun>? PrethodniERacun { get; set; }
        public Izdavatelj Izdavatelj { get; set; } = new("efis");
        public Primatelj Primatelj { get; set; } = new("efis");
        public List<PrijenosSredstava>? PrijenosSredstava { get; set; }
        public DokumentUkupanIznos DokumentUkupanIznos { get; set; } = new("efis");
        public List<RaspodjelaPdv> RaspodjelaPdv { get; set; } = new();
        public List<StavkaERacuna> StavkaERacuna { get; set; } = new();
        public bool IndikatorKopije { get; set; }

        public string ToXmlString()
        {
            var res = "<efis:ERacun>";
            res += $"<efis:brojDokumenta>{XmlUtils.XmlEscape(BrojDokumenta)}</efis:brojDokumenta>";
            res += $"<efis:datumIzdavanja>{XmlUtils.XmlEscape(DatumIzdavanja)}</efis:datumIzdavanja>";
            res += $"<efis:vrstaDokumenta>{XmlUtils.XmlEscape(VrstaDokumenta)}</efis:vrstaDokumenta>";
            res += $"<efis:valutaERacuna>{XmlUtils.XmlEscape(ValutaERacuna)}</efis:valutaERacuna>";
            if (DatumDospijecaPlacanja != null)
            {
                res += $"<efis:datumDospijecaPlacanja>{XmlUtils.XmlEscape(DatumDospijecaPlacanja)}</efis:datumDospijecaPlacanja>";
            }
            if (DatumIsporuke != null)
            {
                res += $"<efis:datumIsporuke>{XmlUtils.XmlEscape(DatumIsporuke)}</efis:datumIsporuke>";
            }
            res += $"<efis:vrstaPoslovnogProcesa>{XmlUtils.XmlEscape(VrstaPoslovnogProcesa)}</efis:vrstaPoslovnogProcesa>";
            if (ReferencaNaUgovor != null)
            {
                res += $"<efis:referencaNaUgovor>{XmlUtils.XmlEscape(ReferencaNaUgovor)}</efis:referencaNaUgovor>";
            }
            if (PrethodniERacun != null)
            {
                foreach (var p in PrethodniERacun)
                {
                    res += p.ToXmlString();
                }
            }
            res += Izdavatelj.ToXmlString();
            res += Primatelj.ToXmlString();
            if (PrijenosSredstava != null)
            {
                foreach (var ps in PrijenosSredstava)
                {
                    res += ps.ToXmlString();
                }
            }
            res += DokumentUkupanIznos.ToXmlString();
            foreach (var pdv in RaspodjelaPdv)
            {
                res += pdv.ToXmlString();
            }
            foreach (var stavka in StavkaERacuna)
            {
                res += stavka.ToXmlString();
            }
            res += $"<efis:indikatorKopije>{(IndikatorKopije ? "true" : "false")}</efis:indikatorKopije>";
            res += "</efis:ERacun>";
            return res;
        }

        public static ERacun FromUblElement(XElement el, string type)
        {
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";

            var res = new ERacun
            {
                BrojDokumenta = el.Element(cbc + "ID")?.Value ?? string.Empty,
                DatumIzdavanja = el.Element(cbc + "IssueDate")?.Value ?? string.Empty,
                VrstaDokumenta = type,
                ValutaERacuna = el.Element(cbc + "DocumentCurrencyCode")?.Value ?? string.Empty,
                DatumDospijecaPlacanja = el.Element(cac + "PaymentMeans")?.Element(cbc + "PaymentDueDate")?.Value,
                DatumIsporuke = el.Element(cac + "Delivery")?.Element(cbc + "ActualDeliveryDate")?.Value,
                VrstaPoslovnogProcesa = el.Element(cbc + "BuyerReference")?.Value ?? string.Empty,
                ReferencaNaUgovor = el.Element(cac + "ContractDocumentReference")?.Element(cbc + "ID")?.Value,
                PrethodniERacun = PrethodniERacun.FromUblElement(el),
                Izdavatelj = Izdavatelj.FromUblElement(el, "efis"),
                Primatelj = Primatelj.FromUblElement(el, "efis"),
                PrijenosSredstava = PrijenosSredstava.FromUblElement(el, "efis"),
                DokumentUkupanIznos = DokumentUkupanIznos.FromUblElement(el, "efis"),
                RaspodjelaPdv = RaspodjelaPdv.FromUblElement(el, "efis"),
                StavkaERacuna = StavkaERacuna.FromUblElement(el),
                IndikatorKopije = el.Element(cbc + "CopyIndicator")?.Value == "true"
            };

            return res;
        }
    }

    public class Racun
    {
        public string BrojDokumenta { get; set; } = string.Empty;
        public string DatumIzdavanja { get; set; } = string.Empty;
        public string VrstaDokumenta { get; set; } = string.Empty;
        public string ValutaRacuna { get; set; } = string.Empty;
        public string? DatumDospijecaPlacanja { get; set; }
        public string? DatumIsporuke { get; set; }
        public string VrstaPoslovnogProcesa { get; set; } = string.Empty;
        public string? ReferencaNaUgovor { get; set; }
        public List<PrethodniRacun>? PrethodniRacun { get; set; }
        public Izdavatelj Izdavatelj { get; set; } = new("eizv");
        public Primatelj Primatelj { get; set; } = new("eizv");
        public List<PrijenosSredstava>? PrijenosSredstava { get; set; }
        public DokumentUkupanIznos DokumentUkupanIznos { get; set; } = new("eizv");
        public List<RaspodjelaPdv> RaspodjelaPdv { get; set; } = new();
        public List<StavkaRacuna> StavkaRacuna { get; set; } = new();
        public bool IndikatorKopije { get; set; }

        public string ToXmlString()
        {
            var res = "<eizv:Racun>";
            res += $"<eizv:brojDokumenta>{XmlUtils.XmlEscape(BrojDokumenta)}</eizv:brojDokumenta>";
            res += $"<eizv:datumIzdavanja>{XmlUtils.XmlEscape(DatumIzdavanja)}</eizv:datumIzdavanja>";
            res += $"<eizv:vrstaDokumenta>{XmlUtils.XmlEscape(VrstaDokumenta)}</eizv:vrstaDokumenta>";
            res += $"<eizv:valutaRacuna>{XmlUtils.XmlEscape(ValutaRacuna)}</eizv:valutaRacuna>";
            if (DatumDospijecaPlacanja != null)
            {
                res += $"<eizv:datumDospijecaPlacanja>{XmlUtils.XmlEscape(DatumDospijecaPlacanja)}</eizv:datumDospijecaPlacanja>";
            }
            if (DatumIsporuke != null)
            {
                res += $"<eizv:datumIsporuke>{XmlUtils.XmlEscape(DatumIsporuke)}</eizv:datumIsporuke>";
            }
            res += $"<eizv:vrstaPoslovnogProcesa>{XmlUtils.XmlEscape(VrstaPoslovnogProcesa)}</eizv:vrstaPoslovnogProcesa>";
            if (ReferencaNaUgovor != null)
            {
                res += $"<eizv:referencaNaUgovor>{XmlUtils.XmlEscape(ReferencaNaUgovor)}</eizv:referencaNaUgovor>";
            }
            if (PrethodniRacun != null)
            {
                foreach (var p in PrethodniRacun)
                {
                    res += p.ToXmlString();
                }
            }
            res += Izdavatelj.ToXmlString();
            res += Primatelj.ToXmlString();
            if (PrijenosSredstava != null)
            {
                foreach (var ps in PrijenosSredstava)
                {
                    res += ps.ToXmlString();
                }
            }
            res += DokumentUkupanIznos.ToXmlString();
            foreach (var pdv in RaspodjelaPdv)
            {
                res += pdv.ToXmlString();
            }
            foreach (var stavka in StavkaRacuna)
            {
                res += stavka.ToXmlString();
            }
            res += $"<eizv:indikatorKopije>{(IndikatorKopije ? "true" : "false")}</eizv:indikatorKopije>";
            res += "</eizv:Racun>";
            return res;
        }

        public static Racun FromUblElement(XElement el, string type)
        {
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";

            var res = new Racun
            {
                BrojDokumenta = el.Element(cbc + "ID")?.Value ?? string.Empty,
                DatumIzdavanja = el.Element(cbc + "IssueDate")?.Value ?? string.Empty,
                VrstaDokumenta = type,
                ValutaRacuna = el.Element(cbc + "DocumentCurrencyCode")?.Value ?? string.Empty,
                DatumDospijecaPlacanja = el.Element(cac + "PaymentMeans")?.Element(cbc + "PaymentDueDate")?.Value,
                DatumIsporuke = el.Element(cac + "Delivery")?.Element(cbc + "ActualDeliveryDate")?.Value,
                VrstaPoslovnogProcesa = el.Element(cbc + "BuyerReference")?.Value ?? string.Empty,
                ReferencaNaUgovor = el.Element(cac + "ContractDocumentReference")?.Element(cbc + "ID")?.Value,
                PrethodniRacun = PrethodniRacun.FromUblElement(el),
                Izdavatelj = Izdavatelj.FromUblElement(el, "eizv"),
                Primatelj = Primatelj.FromUblElement(el, "eizv"),
                PrijenosSredstava = PrijenosSredstava.FromUblElement(el, "eizv"),
                DokumentUkupanIznos = DokumentUkupanIznos.FromUblElement(el, "eizv"),
                RaspodjelaPdv = RaspodjelaPdv.FromUblElement(el, "eizv"),
                StavkaRacuna = StavkaRacuna.FromUblElement(el),
                IndikatorKopije = el.Element(cbc + "CopyIndicator")?.Value == "true"
            };

            return res;
        }
    }

    public class PrethodniERacun
    {
        public string BrojDokumenta { get; set; } = string.Empty;
        public string DatumIzdavanja { get; set; } = string.Empty;

        public string ToXmlString()
        {
            var res = "<efis:PrethodniERacun>";
            res += $"<efis:brojDokumenta>{XmlUtils.XmlEscape(BrojDokumenta)}</efis:brojDokumenta>";
            res += $"<efis:datumIzdavanja>{XmlUtils.XmlEscape(DatumIzdavanja)}</efis:datumIzdavanja>";
            res += "</efis:PrethodniERacun>";
            return res;
        }

        public static List<PrethodniERacun>? FromUblElement(XElement el)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            var list = new List<PrethodniERacun>();
            foreach (var br in el.Elements(cac + "BillingReference"))
            {
                var doc = br.Element(cac + "InvoiceDocumentReference");
                if (doc != null)
                {
                    list.Add(new PrethodniERacun
                    {
                        BrojDokumenta = doc.Element(cbc + "ID")?.Value ?? string.Empty,
                        DatumIzdavanja = doc.Element(cbc + "IssueDate")?.Value ?? string.Empty
                    });
                }
            }
            return list.Count > 0 ? list : null;
        }
    }

    public class PrethodniRacun
    {
        public string BrojDokumenta { get; set; } = string.Empty;
        public string DatumIzdavanja { get; set; } = string.Empty;

        public string ToXmlString()
        {
            var res = "<eizv:PrethodniRacun>";
            res += $"<eizv:brojDokumenta>{XmlUtils.XmlEscape(BrojDokumenta)}</eizv:brojDokumenta>";
            res += $"<eizv:datumIzdavanja>{XmlUtils.XmlEscape(DatumIzdavanja)}</eizv:datumIzdavanja>";
            res += "</eizv:PrethodniRacun>";
            return res;
        }

        public static List<PrethodniRacun>? FromUblElement(XElement el)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            var list = new List<PrethodniRacun>();
            foreach (var br in el.Elements(cac + "BillingReference"))
            {
                var doc = br.Element(cac + "InvoiceDocumentReference");
                if (doc != null)
                {
                    list.Add(new PrethodniRacun
                    {
                        BrojDokumenta = doc.Element(cbc + "ID")?.Value ?? string.Empty,
                        DatumIzdavanja = doc.Element(cbc + "IssueDate")?.Value ?? string.Empty
                    });
                }
            }
            return list.Count > 0 ? list : null;
        }
    }

    public class Izdavatelj
    {
        private readonly string _prefix;
        public string Ime { get; set; } = string.Empty;
        public string OibPorezniBroj { get; set; } = string.Empty;
        public string OibOperatera { get; set; } = string.Empty;

        public Izdavatelj(string prefix)
        {
            _prefix = prefix;
        }

        public string ToXmlString()
        {
            var res = $"<{_prefix}:Izdavatelj>";
            res += $"<{_prefix}:ime>{XmlUtils.XmlEscape(Ime)}</{_prefix}:ime>";
            res += $"<{_prefix}:oibPorezniBroj>{XmlUtils.XmlEscape(OibPorezniBroj)}</{_prefix}:oibPorezniBroj>";
            res += $"<{_prefix}:oibOperatera>{XmlUtils.XmlEscape(OibOperatera)}</{_prefix}:oibOperatera>";
            res += $"</{_prefix}:Izdavatelj>";
            return res;
        }

        public static Izdavatelj FromUblElement(XElement el, string prefix)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            var party = el.Element(cac + "AccountingSupplierParty")?.Element(cac + "Party");
            return new Izdavatelj(prefix)
            {
                Ime = party?.Element(cac + "PartyLegalEntity")?.Element(cbc + "RegistrationName")?.Value ?? string.Empty,
                OibPorezniBroj = party?.Element(cac + "PartyTaxScheme")?.Element(cbc + "CompanyID")?.Value ?? string.Empty,
                OibOperatera = party?.Element(cac + "Contact")?.Element(cbc + "ID")?.Value ?? string.Empty
            };
        }
    }

    public class Primatelj
    {
        private readonly string _prefix;
        public string Ime { get; set; } = string.Empty;
        public string OibPorezniBroj { get; set; } = string.Empty;

        public Primatelj(string prefix)
        {
            _prefix = prefix;
        }

        public string ToXmlString()
        {
            var res = $"<{_prefix}:Primatelj>";
            res += $"<{_prefix}:ime>{XmlUtils.XmlEscape(Ime)}</{_prefix}:ime>";
            res += $"<{_prefix}:oibPorezniBroj>{XmlUtils.XmlEscape(OibPorezniBroj)}</{_prefix}:oibPorezniBroj>";
            res += $"</{_prefix}:Primatelj>";
            return res;
        }

        public static Primatelj FromUblElement(XElement el, string prefix)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            var party = el.Element(cac + "AccountingCustomerParty")?.Element(cac + "Party");
            return new Primatelj(prefix)
            {
                Ime = party?.Element(cac + "PartyLegalEntity")?.Element(cbc + "RegistrationName")?.Value ?? string.Empty,
                OibPorezniBroj = party?.Element(cac + "PartyTaxScheme")?.Element(cbc + "CompanyID")?.Value ?? string.Empty
            };
        }
    }

    public class PrijenosSredstava
    {
        private readonly string _prefix;
        public string IdentifikatorRacunaZaPlacanje { get; set; } = string.Empty;
        public string? NazivRacunaZaPlacanje { get; set; }
        public string? IdentifikatorPruzateljaPlatnihUsluga { get; set; }

        public PrijenosSredstava(string prefix)
        {
            _prefix = prefix;
        }

        public string ToXmlString()
        {
            var res = $"<{_prefix}:PrijenosSredstava>";
            res += $"<{_prefix}:identifikatorRacunaZaPlacanje>{XmlUtils.XmlEscape(IdentifikatorRacunaZaPlacanje)}</{_prefix}:identifikatorRacunaZaPlacanje>";
            if (NazivRacunaZaPlacanje != null)
            {
                res += $"<{_prefix}:nazivRacunaZaPlacanje>{XmlUtils.XmlEscape(NazivRacunaZaPlacanje)}</{_prefix}:nazivRacunaZaPlacanje>";
            }
            if (IdentifikatorPruzateljaPlatnihUsluga != null)
            {
                res += $"<{_prefix}:identifikatorPruzateljaPlatnihUsluga>{XmlUtils.XmlEscape(IdentifikatorPruzateljaPlatnihUsluga)}</{_prefix}:identifikatorPruzateljaPlatnihUsluga>";
            }
            res += $"</{_prefix}:PrijenosSredstava>";
            return res;
        }

        public static List<PrijenosSredstava>? FromUblElement(XElement el, string prefix)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            var list = new List<PrijenosSredstava>();
            foreach (var pm in el.Elements(cac + "PaymentMeans"))
            {
                var account = pm.Element(cac + "PayeeFinancialAccount");
                if (account != null)
                {
                    list.Add(new PrijenosSredstava(prefix)
                    {
                        IdentifikatorRacunaZaPlacanje = account.Element(cbc + "ID")?.Value ?? string.Empty,
                        NazivRacunaZaPlacanje = account.Element(cbc + "Name")?.Value,
                        IdentifikatorPruzateljaPlatnihUsluga = account.Element(cac + "FinancialInstitutionBranch")?.Element(cbc + "ID")?.Value
                    });
                }
            }
            return list.Count > 0 ? list : null;
        }
    }

    public class DokumentUkupanIznos
    {
        private readonly string _prefix;
        public decimal Neto { get; set; }
        public decimal? Popust { get; set; }
        public decimal IznosBezPdv { get; set; }
        public decimal Pdv { get; set; }
        public decimal IznosSPdv { get; set; }
        public decimal? PlaceniIznos { get; set; }
        public decimal IznosKojiDospijevaZaPlacanje { get; set; }

        public DokumentUkupanIznos(string prefix)
        {
            _prefix = prefix;
        }

        public string ToXmlString()
        {
            var res = $"<{_prefix}:DokumentUkupanIznos>";
            res += $"<{_prefix}:neto>{Neto:F2}</{_prefix}:neto>";
            if (Popust != null)
            {
                res += $"<{_prefix}:popust>{Popust:F2}</{_prefix}:popust>";
            }
            res += $"<{_prefix}:iznosBezPdv>{IznosBezPdv:F2}</{_prefix}:iznosBezPdv>";
            res += $"<{_prefix}:pdv>{Pdv:F2}</{_prefix}:pdv>";
            res += $"<{_prefix}:iznosSPdv>{IznosSPdv:F2}</{_prefix}:iznosSPdv>";
            if (PlaceniIznos != null)
            {
                res += $"<{_prefix}:placeniIznos>{PlaceniIznos:F2}</{_prefix}:placeniIznos>";
            }
            res += $"<{_prefix}:iznosKojiDospijevaZaPlacanje>{IznosKojiDospijevaZaPlacanje:F2}</{_prefix}:iznosKojiDospijevaZaPlacanje>";
            res += $"</{_prefix}:DokumentUkupanIznos>";
            return res;
        }

        public static DokumentUkupanIznos FromUblElement(XElement el, string prefix)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            var total = el.Element(cac + "LegalMonetaryTotal");
            var taxTotal = el.Element(cac + "TaxTotal");
            var res = new DokumentUkupanIznos(prefix)
            {
                Neto = decimal.Parse(total?.Element(cbc + "LineExtensionAmount")?.Value ?? "0"),
                IznosBezPdv = decimal.Parse(total?.Element(cbc + "TaxExclusiveAmount")?.Value ?? "0"),
                Pdv = decimal.Parse(taxTotal?.Element(cbc + "TaxAmount")?.Value ?? "0"),
                IznosSPdv = decimal.Parse(total?.Element(cbc + "TaxInclusiveAmount")?.Value ?? "0"),
                PlaceniIznos = total?.Element(cbc + "PrepaidAmount") != null ? decimal.Parse(total.Element(cbc + "PrepaidAmount").Value) : (decimal?)null,
                IznosKojiDospijevaZaPlacanje = decimal.Parse(total?.Element(cbc + "PayableAmount")?.Value ?? "0")
            };
            var popust = el.Element(cac + "AllowanceCharge")?.Element(cbc + "Amount")?.Value;
            if (decimal.TryParse(popust, out var p))
            {
                res.Popust = p;
            }
            return res;
        }
    }

    public class RaspodjelaPdv
    {
        private readonly string _prefix;
        public string KategorijaPdv { get; set; } = string.Empty;
        public decimal OporeziviIznos { get; set; }
        public decimal IznosPoreza { get; set; }
        public decimal? Stopa { get; set; }
        public string? RazlogOslobodenja { get; set; }
        public string? TekstRazlogaOslobodenja { get; set; }

        public RaspodjelaPdv(string prefix)
        {
            _prefix = prefix;
        }

        public string ToXmlString()
        {
            var res = $"<{_prefix}:RaspodjelaPdv>";
            res += $"<{_prefix}:kategorijaPdv>{XmlUtils.XmlEscape(KategorijaPdv)}</{_prefix}:kategorijaPdv>";
            res += $"<{_prefix}:oporeziviIznos>{OporeziviIznos:F2}</{_prefix}:oporeziviIznos>";
            res += $"<{_prefix}:iznosPoreza>{IznosPoreza:F2}</{_prefix}:iznosPoreza>";
            if (Stopa != null)
            {
                res += $"<{_prefix}:stopa>{Stopa:F2}</{_prefix}:stopa>";
            }
            if (RazlogOslobodenja != null)
            {
                res += $"<{_prefix}:razlogOslobodenja>{XmlUtils.XmlEscape(RazlogOslobodenja)}</{_prefix}:razlogOslobodenja>";
            }
            if (TekstRazlogaOslobodenja != null)
            {
                res += $"<{_prefix}:tekstRazlogaOslobodenja>{XmlUtils.XmlEscape(TekstRazlogaOslobodenja)}</{_prefix}:tekstRazlogaOslobodenja>";
            }
            res += $"</{_prefix}:RaspodjelaPdv>";
            return res;
        }

        public static List<RaspodjelaPdv> FromUblElement(XElement el, string prefix)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            var list = new List<RaspodjelaPdv>();
            foreach (var subtotal in el.Elements(cac + "TaxTotal").Elements(cac + "TaxSubtotal"))
            {
                var cat = subtotal.Element(cac + "TaxCategory");
                list.Add(new RaspodjelaPdv(prefix)
                {
                    KategorijaPdv = cat?.Element(cbc + "ID")?.Value ?? string.Empty,
                    OporeziviIznos = decimal.Parse(subtotal.Element(cbc + "TaxableAmount")?.Value ?? "0"),
                    IznosPoreza = decimal.Parse(subtotal.Element(cbc + "TaxAmount")?.Value ?? "0"),
                    Stopa = cat?.Element(cbc + "Percent") != null ? decimal.Parse(cat.Element(cbc + "Percent").Value) : (decimal?)null,
                    RazlogOslobodenja = cat?.Element(cbc + "TaxExemptionReasonCode")?.Value,
                    TekstRazlogaOslobodenja = cat?.Element(cbc + "TaxExemptionReason")?.Value
                });
            }
            return list;
        }
    }

    public class StavkaERacuna
    {
        public decimal Kolicina { get; set; }
        public string JedinicaMjere { get; set; } = string.Empty;
        public decimal ArtiklNetoCijena { get; set; }
        public decimal? ArtiklOsnovnaKolicina { get; set; }
        public string? ArtiklJedinicaMjereZaOsnovnuKolicinu { get; set; }
        public string ArtiklKategorijaPdv { get; set; } = string.Empty;
        public decimal ArtiklStopaPdv { get; set; }
        public string ArtiklNaziv { get; set; } = string.Empty;

        public string ToXmlString()
        {
            var res = "<efis:StavkaERacuna>";
            res += $"<efis:kolicina>{Kolicina:F2}</efis:kolicina>";
            res += $"<efis:jedinicaMjere>{XmlUtils.XmlEscape(JedinicaMjere)}</efis:jedinicaMjere>";
            res += $"<efis:artiklNetoCijena>{ArtiklNetoCijena:F2}</efis:artiklNetoCijena>";
            if (ArtiklOsnovnaKolicina != null)
            {
                res += $"<efis:artiklOsnovnaKolicina>{ArtiklOsnovnaKolicina:F2}</efis:artiklOsnovnaKolicina>";
            }
            if (ArtiklJedinicaMjereZaOsnovnuKolicinu != null)
            {
                res += $"<efis:artiklJedinicaMjereZaOsnovnuKolicinu>{XmlUtils.XmlEscape(ArtiklJedinicaMjereZaOsnovnuKolicinu)}</efis:artiklJedinicaMjereZaOsnovnuKolicinu>";
            }
            res += $"<efis:artiklKategorijaPdv>{XmlUtils.XmlEscape(ArtiklKategorijaPdv)}</efis:artiklKategorijaPdv>";
            res += $"<efis:artiklStopaPdv>{ArtiklStopaPdv:F2}</efis:artiklStopaPdv>";
            res += $"<efis:artiklNaziv>{XmlUtils.XmlEscape(ArtiklNaziv)}</efis:artiklNaziv>";
            res += "</efis:StavkaERacuna>";
            return res;
        }

        private static List<StavkaERacuna> StavkeFromLines(IEnumerable<XElement> lines, string qtyName)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            var list = new List<StavkaERacuna>();
            foreach (var line in lines)
            {
                var item = line.Element(cac + "Item");
                var taxCat = item?.Element(cac + "ClassifiedTaxCategory");
                var price = line.Element(cac + "Price");
                list.Add(new StavkaERacuna
                {
                    Kolicina = decimal.Parse(line.Element(cbc + qtyName)?.Value ?? "0"),
                    JedinicaMjere = line.Element(cbc + qtyName)?.Attribute("unitCode")?.Value ?? string.Empty,
                    ArtiklNetoCijena = decimal.Parse(price?.Element(cbc + "PriceAmount")?.Value ?? "0"),
                    ArtiklOsnovnaKolicina = price?.Element(cbc + "BaseQuantity") != null ? decimal.Parse(price.Element(cbc + "BaseQuantity").Value) : (decimal?)null,
                    ArtiklJedinicaMjereZaOsnovnuKolicinu = price?.Element(cbc + "BaseQuantity")?.Attribute("unitCode")?.Value,
                    ArtiklKategorijaPdv = taxCat?.Element(cbc + "ID")?.Value ?? string.Empty,
                    ArtiklStopaPdv = decimal.Parse(taxCat?.Element(cbc + "Percent")?.Value ?? "0"),
                    ArtiklNaziv = item?.Element(cbc + "Name")?.Value ?? string.Empty
                });
            }
            return list;
        }

        public static List<StavkaERacuna> FromUblElement(XElement el)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            return StavkeFromLines(el.Elements(cac + "InvoiceLine"), "InvoicedQuantity");
        }
    }

    public class StavkaRacuna
    {
        public decimal Kolicina { get; set; }
        public string JedinicaMjere { get; set; } = string.Empty;
        public decimal ArtiklNetoCijena { get; set; }
        public decimal? ArtiklOsnovnaKolicina { get; set; }
        public string? ArtiklJedinicaMjereZaOsnovnuKolicinu { get; set; }
        public string ArtiklKategorijaPdv { get; set; } = string.Empty;
        public decimal ArtiklStopaPdv { get; set; }
        public string ArtiklNaziv { get; set; } = string.Empty;

        public string ToXmlString()
        {
            var res = "<eizv:StavkaRacuna>";
            res += $"<eizv:kolicina>{Kolicina:F2}</eizv:kolicina>";
            res += $"<eizv:jedinicaMjere>{XmlUtils.XmlEscape(JedinicaMjere)}</eizv:jedinicaMjere>";
            res += $"<eizv:artiklNetoCijena>{ArtiklNetoCijena:F2}</eizv:artiklNetoCijena>";
            if (ArtiklOsnovnaKolicina != null)
            {
                res += $"<eizv:artiklOsnovnaKolicina>{ArtiklOsnovnaKolicina:F2}</eizv:artiklOsnovnaKolicina>";
            }
            if (ArtiklJedinicaMjereZaOsnovnuKolicinu != null)
            {
                res += $"<eizv:artiklJedinicaMjereZaOsnovnuKolicinu>{XmlUtils.XmlEscape(ArtiklJedinicaMjereZaOsnovnuKolicinu)}</eizv:artiklJedinicaMjereZaOsnovnuKolicinu>";
            }
            res += $"<eizv:artiklKategorijaPdv>{XmlUtils.XmlEscape(ArtiklKategorijaPdv)}</eizv:artiklKategorijaPdv>";
            res += $"<eizv:artiklStopaPdv>{ArtiklStopaPdv:F2}</eizv:artiklStopaPdv>";
            res += $"<eizv:artiklNaziv>{XmlUtils.XmlEscape(ArtiklNaziv)}</eizv:artiklNaziv>";
            res += "</eizv:StavkaRacuna>";
            return res;
        }

        public static List<StavkaRacuna> FromUblElement(XElement el)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            return StavkaERacuna.FromUblElement(el).ConvertAll(s => new StavkaRacuna
            {
                Kolicina = s.Kolicina,
                JedinicaMjere = s.JedinicaMjere,
                ArtiklNetoCijena = s.ArtiklNetoCijena,
                ArtiklOsnovnaKolicina = s.ArtiklOsnovnaKolicina,
                ArtiklJedinicaMjereZaOsnovnuKolicinu = s.ArtiklJedinicaMjereZaOsnovnuKolicinu,
                ArtiklKategorijaPdv = s.ArtiklKategorijaPdv,
                ArtiklStopaPdv = s.ArtiklStopaPdv,
                ArtiklNaziv = s.ArtiklNaziv
            });
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
                res += r.ToXmlString();
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
}
