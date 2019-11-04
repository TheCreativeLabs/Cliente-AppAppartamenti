using AppAppartamentiApi.Dto;
using AppAppartamentiApi.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace AppAppartamentiApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Annunci")]
    public class AnnunciController : ApiController
    {
        private DbDataContext dbDataContext = new DbDataContext();

        // GET api/values
        [HttpGet]
        [Route("Annunci")]
        [ResponseType(typeof(List<AnnunciDtoOutput>))]
        public List<AnnunciDtoOutput> GetAnnunci()
        {
            List<AnnunciDtoOutput> annunci = dbDataContext.Annuncio
                                                .Include(x => x.Comuni)
                                             .Select(annuncio => new AnnunciDtoOutput()
                                             {
                                                 Id = annuncio.Id,
                                                 IdUtente = annuncio.IdUtente,
                                                 DataCreazione = annuncio.DataCreazione,
                                                 DataModifica = annuncio.DataModifica,
                                                 CodiceComune = annuncio.ComuneCodice,
                                                 NomeComune = annuncio.Comuni.NomeComune,
                                                 Indirizzo = annuncio.Indirizzo,
                                                 Prezzo = annuncio.Prezzo,
                                                 Superficie = annuncio.Superficie,
                                                 Descrizione = annuncio.Descrizione,
                                                 TipologiaAnnuncio = annuncio.TipologiaAnnuncio.Descrizione,
                                                 TipologiaProprieta = annuncio.TipologiaProprieta.Descrizione,
                                                 Completato = annuncio.Completato,
                                                 Cancellato = annuncio.Cancellato
                                             }).ToList();

            annunci.ForEach(x =>
            {
                x.ImmaginePrincipale = dbDataContext.ImmagineAnnuncio.Where(i => i.IdAnnuncio == x.Id).Select(i => i.Immagine).FirstOrDefault();
            });

            return annunci;
        }

        // GET api/values
        [HttpGet]
        [Route("AnnunciCurrentUser")]
        [ResponseType(typeof(List<AnnunciDtoOutput>))]
        public List<AnnunciDtoOutput> GetAnnunciByUser()
        {
            var id = new Guid(User.Identity.GetUserId());
            List<AnnunciDtoOutput> annunci = dbDataContext.Annuncio
                                        .Include(x => x.Comuni)
                                        .Where(x => x.IdUtente == id)
                                        .Select(annuncio => new AnnunciDtoOutput()
                                        {
                                            Id = annuncio.Id,
                                            IdUtente = annuncio.IdUtente,
                                            DataCreazione = annuncio.DataCreazione,
                                            DataModifica = annuncio.DataModifica,
                                            CodiceComune = annuncio.ComuneCodice,
                                            NomeComune = annuncio.Comuni.NomeComune,
                                            Indirizzo = annuncio.Indirizzo,
                                            Prezzo = annuncio.Prezzo,
                                            Superficie = annuncio.Superficie,
                                            Descrizione = annuncio.Descrizione,
                                            TipologiaAnnuncio = annuncio.TipologiaAnnuncio.Descrizione,
                                            TipologiaProprieta = annuncio.TipologiaProprieta.Descrizione,
                                            Completato = annuncio.Completato,
                                            Cancellato = annuncio.Cancellato
                                        }).ToList();

            annunci.ForEach(x =>
            {
                x.ImmaginePrincipale = dbDataContext.ImmagineAnnuncio.Where(i => i.IdAnnuncio == x.Id).Select(i => i.Immagine).FirstOrDefault();
            });

            return annunci;
        }

        // POST: api/Evento/EventoCreate
        [HttpPost]
        [Route("AnnuncioCreate", Name = "AnnuncioCreate")]
        [ResponseType(typeof(AnnuncioDtoInput))]
        public async Task<IHttpActionResult> InsertAnnuncio(AnnuncioDtoInput Annuncio)
        {
            //Controllo se il modello è valido
            if (Annuncio == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = Guid.NewGuid();

            //Creo l'annuncio
            Annuncio annuncio = new Annuncio()
            {
                Id = id,
                IdUtente = new Guid(User.Identity.GetUserId()),
                DataCreazione = DateTime.Now,
                DataModifica = DateTime.Now,
                ComuneCodice = Annuncio.CodiceComune,
                Indirizzo = Annuncio.Indirizzo,
                Prezzo = Annuncio.Prezzo,
                Superficie = Annuncio.Superficie,
                UltimoPiano = Annuncio.UltimoPiano,
                Piano = Annuncio.Piano,
                NumeroCameraLetto = Annuncio.NumeroCameraLetto,
                NumeroAltreStanze = Annuncio.NumeroAltreStanze,
                NumeroBagni = Annuncio.NumeroBagni,
                NumeroCucine = Annuncio.NumeroCucine,
                NumeroGarage = Annuncio.NumeroGarage,
                NumeroPostiAuto = Annuncio.NumeroPostiAuto,
                Giardino = Annuncio.Giardino,
                Disponibile = Annuncio.Disponibile,
                Ascensore = Annuncio.Ascensore,
                Balcone = Annuncio.Balcone,
                Piscina = Annuncio.Piscina,
                Cantina = Annuncio.Cantina,
                Descrizione = Annuncio.Descrizione,
                SpesaMensileCondominio = Annuncio.SpesaMensileCondominio,
                IdTipologiaRiscaldamento = Annuncio.IdTipologiaRiscaldamento,
                IdTipologiaProprieta = Annuncio.IdTipologiaProprieta,
                IdTipologiaAnnuncio = Annuncio.IdTipologiaAnnuncio,
                IdStatoProprieta = Annuncio.IdStatoProprieta,
                IdClasseEnergetica = Annuncio.IdClasseEnergetica,
                Condizionatori = Annuncio.Condizionatori,
                Completato = Annuncio.Completato,
                Cancellato = Annuncio.Cancellato
            };

            //Salvo l'annuncio sul DB
            dbDataContext.Annuncio.Add(annuncio);

            foreach (byte[] immagine in Annuncio.Immagini)
            {
                //Creo l'immagine
                ImmagineAnnuncio immagineAnnuncio = new ImmagineAnnuncio()
                {
                    Id = Guid.NewGuid(),
                    Immagine = immagine,
                    IdAnnuncio = annuncio.Id
                };

                dbDataContext.ImmagineAnnuncio.Add(immagineAnnuncio);
            }

            await dbDataContext.SaveChangesAsync();

            //try
            //{

            //}
            //catch (DbUpdateException)
            //{
            //    if (EventoExists(evento.Id))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return Ok(Annuncio);
        }


        // GET api/values
        [HttpGet]
        [Route("ListaTipologiaAnnunci")]
        [ResponseType(typeof(List<TipologiaAnnuncio>))]
        public List<TipologiaAnnuncio> GetListaTipologiaAnnunci()
        {
            List<TipologiaAnnuncio> listaTipologiaAnnunci = dbDataContext.TipologiaAnnuncio.Where(x => x.Abilitato == true).ToList();

            return listaTipologiaAnnunci;
        }

        // GET api/values
        [HttpGet]
        [Route("ListaTipologiaProprieta")]
        [ResponseType(typeof(List<TipologiaProprieta>))]
        public List<TipologiaProprieta> GetListaTipologiaProprieta()
        {
            List<TipologiaProprieta> listaTipologiaProprieta = dbDataContext.TipologiaProprieta.Where(x => x.Abilitato == true).ToList();

            return listaTipologiaProprieta;
        }

        // GET api/values
        [HttpGet]
        [Route("ListaComuni")]
        [ResponseType(typeof(List<Comuni>))]
        public List<Comuni> GetListaComuni(string NomeComune)
        {
            List<Comuni> listaComuni = dbDataContext.Comuni
                                                .Where(x => x.NomeComune.Any(nome => nome.ToString().StartsWith(NomeComune, StringComparison.OrdinalIgnoreCase)))
                                                .Take(30)
                                                .ToList();

            return listaComuni;
        }
    }
}
