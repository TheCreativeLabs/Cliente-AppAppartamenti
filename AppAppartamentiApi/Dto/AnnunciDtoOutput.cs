using AppAppartamentiApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppAppartamentiApi.Dto
{
    public class AnnunciDtoOutput
    {

        public AnnunciDtoOutput(){

        }

        public static AnnunciDtoOutput MapperAnnuncio(Annuncio annuncio)
        {
            return new AnnunciDtoOutput() {
                Id = annuncio.Id,
                IdUtente = annuncio.IdUtente,
                DataCreazione = annuncio.DataCreazione,
                DataModifica = annuncio.DataModifica,
                CodiceComune = annuncio.ComuneCodice,
                NomeComune  =annuncio.Comuni.NomeComune,
                Indirizzo = annuncio.Indirizzo,
                Prezzo = annuncio.Prezzo,
                Superficie = annuncio.Superficie,
                Descrizione = annuncio.Descrizione,
                TipologiaAnnuncio = annuncio.TipologiaAnnuncio.Descrizione,
                TipologiaProprieta = annuncio.TipologiaProprieta.Descrizione,
                Completato = annuncio.Completato,
                Cancellato = annuncio.Cancellato
            };
        }

        public Guid Id { get; set; }

        public Guid IdUtente { get; set; }

        public DateTime DataCreazione { get; set; }

        public DateTime? DataModifica { get; set; }

        public int CodiceComune { get; set; }

        public string NomeComune { get; set; }

        [StringLength(512)]
        public string Indirizzo { get; set; }

        public double? Prezzo { get; set; }

        public double? Superficie { get; set; }

        [StringLength(8000)]
        public string Descrizione { get; set; }

        public string TipologiaProprieta { get; set; }

        public string TipologiaAnnuncio { get; set; }

        public bool Completato { get; set; }

        public bool Cancellato { get; set; }

        public byte[] ImmaginePrincipale { get; set; }
    }
}