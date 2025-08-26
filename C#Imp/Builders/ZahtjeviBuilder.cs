using System;
using System.Collections.Generic;
using Fiskalizacija2.Models.Xml;

namespace Fiskalizacija2.Builders
{
    public static class ZahtjeviBuilder
    {
        public static EvidentirajERacunZahtjev GetEvidentirajERacunZahtjev(string vrsta, ERacun eracun)
        {
            return GetEvidentirajERacunZahtjev(vrsta, new[] { eracun });
        }

        public static EvidentirajERacunZahtjev GetEvidentirajERacunZahtjev(string vrsta, IEnumerable<ERacun> eracun)
        {
            return new EvidentirajERacunZahtjev
            {
                Id = Guid.NewGuid().ToString(),
                Zaglavlje = new ZaglavljeFiskalizacija
                {
                    DatumVrijemeSlanja = TimeUtils.GetCurrentDateTimeString(),
                    VrstaERacuna = vrsta
                },
                ERacun = new List<ERacun>(eracun)
            };
        }

        public static EvidentirajIsporukuZaKojuNijeIzdanERacunZahtjev GetEvidentirajIsporukuZaKojuNijeIzdanERacunZahtjev(IEnumerable<Racun> racun)
        {
            return new EvidentirajIsporukuZaKojuNijeIzdanERacunZahtjev
            {
                Id = Guid.NewGuid().ToString(),
                Zaglavlje = new ZaglavljeFiskalizacija
                {
                    DatumVrijemeSlanja = TimeUtils.GetCurrentDateTimeString(),
                    VrstaERacuna = "IR"
                },
                Racun = new List<Racun>(racun)
            };
        }

        public static EvidentirajNaplatuZahtjev GetEvidentirajNaplatuZahtjev(IEnumerable<Naplata> naplata)
        {
            return new EvidentirajNaplatuZahtjev
            {
                Id = Guid.NewGuid().ToString(),
                Zaglavlje = new ZaglavljeFiskalizacija
                {
                    DatumVrijemeSlanja = TimeUtils.GetCurrentDateTimeString()
                },
                Naplata = new List<Naplata>(naplata)
            };
        }

        public static EvidentirajOdbijanjeZahtjev GetEvidentirajOdbijanjeZahtjev(IEnumerable<Odbijanje> odbijanje)
        {
            return new EvidentirajOdbijanjeZahtjev
            {
                Id = Guid.NewGuid().ToString(),
                Zaglavlje = new ZaglavljeFiskalizacija
                {
                    DatumVrijemeSlanja = TimeUtils.GetCurrentDateTimeString()
                },
                Odbijanje = new List<Odbijanje>(odbijanje)
            };
        }
    }
}
