using AppAppartamentiApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppAppartamentiApi.Dto
{
    public class ImmagineAnnuncioContainer
    {
        public byte[] Immagine { get; set; }

        public ImmagineAnnuncioContainer(byte[] Imm)
        {
            this.Immagine = Imm;
        }
    }

    public class AnnuncioDtoOutput
    {
        

        public AnnuncioDtoOutput(){
            //ImmaginiAnnuncio = new List<byte[]>();
            ImmaginiPlanimetria = new List<byte[]>();
        }

        public static AnnuncioDtoOutput MapperAnnuncio(Annuncio annuncio, bool preferito)
        {
            AnnuncioDtoOutput dto = new AnnuncioDtoOutput() {
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
                Completato = annuncio.Completato,
                Cancellato = annuncio.Cancellato,
                UltimoPiano = annuncio.UltimoPiano,
                Piano = annuncio.Piano,
                NumeroCameraLetto = annuncio.NumeroCameraLetto,
                NumeroAltreStanze = annuncio.NumeroAltreStanze,
                NumeroBagni = annuncio.NumeroBagni,
                NumeroCucine = annuncio.NumeroCucine,
                NumeroGarage = annuncio.NumeroGarage,
                NumeroPostiAuto = annuncio.NumeroPostiAuto,
                Giardino = annuncio.Giardino,
                Disponibile = annuncio.Disponibile,
                Ascensore = annuncio.Ascensore,
                Balcone = annuncio.Balcone,
                Piscina = annuncio.Piscina,
                Cantina = annuncio.Cantina,
                SpesaMensileCondominio = annuncio.SpesaMensileCondominio,
                Condizionatori = annuncio.Condizionatori,
                FlagPreferito = preferito,
                CoordinateGeografiche = annuncio.CoordinateGeografiche
            };

            if (annuncio.TipologiaAnnuncio != null)
            {
                dto.TipologiaAnnuncio = annuncio.TipologiaAnnuncio.Descrizione;
            }
            if (annuncio.TipologiaProprieta != null)
            {
                dto.TipologiaProprieta = annuncio.TipologiaProprieta.Descrizione;
            }
            if (annuncio.TipologiaRiscaldamento != null)
            {
                dto.TipologiaRiscaldamento = annuncio.TipologiaRiscaldamento.Descrizione;
            }
            if (annuncio.StatoProprieta != null)
            {
                dto.StatoProprieta = annuncio.StatoProprieta.Descrizione;
            }
            if (annuncio.ClasseEnergetica != null)
            {
                dto.ClasseEnergetica = annuncio.ClasseEnergetica.Codice;
            }

            //le carichiamo in modo asincrono
            //foreach (var imm in annuncio.ImmagineAnnuncio)
            //{
            //    dto.ImmaginiAnnuncio.Add(imm.Immagine);
                
            //}

            if(annuncio.ImmaginiPlanimetria != null) { 
                foreach (var imm in annuncio.ImmaginiPlanimetria)
                {
                    dto.ImmaginiPlanimetria.Add(imm.Immagine);

                }
            }

            if (annuncio.Video != null)
            {
                Video video = annuncio.Video.OfType<Video>().FirstOrDefault();
                if (video != null) dto.Video = video.VideoBytes;
            }


            if (annuncio.FasceOrarie != null && annuncio.FasceOrarie.Any())
            {
                FasceOrarie fasceOrarie = annuncio.FasceOrarie.FirstOrDefault();
                DisponibilitaOrariaDto disponibilitaOrariaDto = new DisponibilitaOrariaDto();
                disponibilitaOrariaDto.fasceOrarieLunedi = fasceOrarie.Monday;
                disponibilitaOrariaDto.fasceOrarieMartedi = fasceOrarie.Tuesday;
                disponibilitaOrariaDto.fasceOrarieMercoledi = fasceOrarie.Wednesday;
                disponibilitaOrariaDto.fasceOrarieGiovedi = fasceOrarie.Thursday;
                disponibilitaOrariaDto.fasceOrarieVenerdi = fasceOrarie.Friday;
                disponibilitaOrariaDto.fasceOrarieSabato = fasceOrarie.Saturday;
                disponibilitaOrariaDto.fasceOrarieDomenica = fasceOrarie.Sunday;
                dto.DisponibilitaOraria = disponibilitaOrariaDto;
            }

            return dto;
        }

        public Guid Id { get; set; }

        public Guid IdUtente { get; set; }

        public DateTime DataCreazione { get; set; }

        public DateTime? DataModifica { get; set; }

        public int? CodiceComune { get; set; }

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

        public List<byte[]> ImmaginiAnnuncio { get; set; }

        public bool UltimoPiano { get; set; }

        public int Piano { get; set; }

        public int NumeroCameraLetto { get; set; }

        public int NumeroAltreStanze { get; set; }

        public int NumeroBagni { get; set; }

        public int NumeroCucine { get; set; }

        public int NumeroGarage { get; set; }

        public int NumeroPostiAuto { get; set; }

        public bool Giardino { get; set; }

        public bool Disponibile { get; set; }

        public bool Ascensore { get; set; }

        public bool Balcone { get; set; }

        public bool Piscina { get; set; }

        public bool Cantina { get; set; }

        public double? SpesaMensileCondominio { get; set; }

        public string TipologiaRiscaldamento { get; set; }

        public string StatoProprieta { get; set; }

        public string ClasseEnergetica { get; set; }

        public bool Condizionatori { get; set; }

        public byte[] Video { get; set;  }

        public List<byte[]> ImmaginiPlanimetria { get; set; }

        public bool FlagPreferito { get; set; }

        public string CoordinateGeografiche { get; set; }

        public DisponibilitaOrariaDto DisponibilitaOraria { get; set; }
    }
}