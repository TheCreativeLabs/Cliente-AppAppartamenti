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

        public enum AnnunciOrder
        {
            PRICE = 0,
            SIZE =1,
            CREATION_DATE = 2
        }

        public enum OrderDirection
        {
            ASC = 0,
            DESC =1
        }

        // GET api/Annunci/Annunci
        /// <summary>
        /// / Restituisce tutti gli annunci, una pagina per volta, con l'ordinamento richiesto.
        /// se orderDirection è  null, l'ordinamento avviene di default in modo DESCendente, cioè dal più grande al più piccolo
        /// se orderBy è null, l'ordinamento avviene di default per Data creazione (DESC)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="idTipologiaAnnuncio"></param>
        /// <param name="idTipologiaProprieta"></param>
        /// <param name="comuneCodice"></param>
        /// <param name="priceMin"></param>
        /// <param name="priceMax"></param>
        /// <param name="houseSizeMin"></param>
        /// <param name="houseSizeMax"></param>
        /// <param name="bedrooms"></param>
        /// <param name="bathrooms"></param>
        /// <param name="kitchens"></param>
        /// <param name="parkingSpaces"></param>
        /// <param name="garages"></param>
        /// <param name="backyard"></param>
        /// <param name="terrace"></param>
        /// <param name="cellar"></param>
        /// <param name="pool"></param>
        /// <param name="elevator"></param>
        /// <param name="orderBy">Valori accettati: PRICE, CREATION_DATE, SIZE. Se parametro mancante o null, default CREATION_DATE </param>
        /// <param name="orderDirection">Valori accettati: ASC, DESC. Se parametro mancante o null, default DESC</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Annunci")]
        [ResponseType(typeof(List<AnnunciDtoOutput>))]
        public List<AnnunciDtoOutput> GetAnnunci(int pageNumber, int pageSize,
                                                       Guid? idTipologiaAnnuncio = null,
                                                       Guid? idTipologiaProprieta = null,
                                                       int? comuneCodice = null,
                                                       int? priceMin = null,
                                                       int? priceMax = null,
                                                       int? houseSizeMin = null,
                                                       int? houseSizeMax = null,
                                                       int? bedrooms = null,
                                                       int? bathrooms = null,
                                                       int? kitchens = null,
                                                       int? parkingSpaces = null,
                                                       int? garages = null,
                                                       bool? backyard = null,
                                                       bool? terrace = null,
                                                       bool? cellar = null,
                                                       bool? pool = null,
                                                       bool? elevator = null,
                                                       AnnunciOrder? orderBy = null,
                                                       OrderDirection? orderDirection = null)
        {

            //List<AnnunciDtoOutput> annunci = dbDataContext.Annuncio
            IQueryable<Annuncio> query = dbDataContext.Annuncio
                                            .Include(x => x.Comuni)
                                            .Where(x => (idTipologiaAnnuncio == null || x.IdTipologiaAnnuncio == idTipologiaAnnuncio)
                                                & (idTipologiaProprieta == null || x.IdTipologiaProprieta == idTipologiaProprieta)
                                                & (comuneCodice == null || x.ComuneCodice == comuneCodice)
                                                & (priceMin == null || x.Prezzo >= priceMin)
                                                & (priceMax == null || x.Prezzo <= priceMax)
                                                & (houseSizeMin == null || x.Superficie >= houseSizeMin)
                                                & (houseSizeMax == null || x.Superficie <= houseSizeMax)
                                                & (bedrooms == null || x.NumeroCameraLetto >= bedrooms)
                                                & (bathrooms == null || x.NumeroBagni >= bathrooms)
                                                & (kitchens == null || x.NumeroCucine >= kitchens)
                                                & (parkingSpaces == null || x.NumeroPostiAuto >= parkingSpaces)
                                                & (garages == null || x.NumeroGarage >= garages)
                                                & (backyard == null || x.Giardino == backyard)
                                                & (terrace == null || x.Balcone == terrace)
                                                & (cellar == null || x.Cantina == cellar)
                                                & (pool == null || x.Piscina == pool)
                                                & (elevator == null || x.Ascensore == elevator)
                                        );


            if (orderBy == null || AnnunciOrder.CREATION_DATE == orderBy)
            {
                if (orderDirection != null && OrderDirection.ASC == orderDirection)
                {
                    query = query.OrderBy(x => x.DataCreazione);
                }
                else
                {
                    query = query.OrderByDescending(x => x.DataCreazione);
                }
            } else if( AnnunciOrder.PRICE == orderBy)
            {
                if (orderDirection != null && OrderDirection.ASC == orderDirection)
                {
                    query = query.OrderBy(x => x.Prezzo);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Prezzo);
                }
            }
            else if (AnnunciOrder.SIZE == orderBy)
            {
                if (orderDirection != null && OrderDirection.ASC == orderDirection)
                {
                    query = query.OrderBy(x => x.Superficie);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Superficie);
                }
            }
            List<AnnunciDtoOutput> annunci = query
                                       //.OrderBy(x =>  x.DataCreazione) //FIXME ORDINAMENTO
                                        .Skip(pageSize * (pageNumber - 1))
                                        .Take(pageSize)
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

            //FIXME MOMENTANEAMENTE COMMENTATO PER NON FARE DOWNLOAD DI TROPPI DATI, SCOMMENTARE PER AVERE LE IMMAGINI

            //annunci.ForEach(x =>
            //{
            //    x.ImmaginePrincipale = dbDataContext.ImmagineAnnuncio.Where(i => i.IdAnnuncio == x.Id).Select(i => i.Immagine).FirstOrDefault();
            //});
            return annunci;
        }

        // GET api/Annunci/AnnunciCurrentUser
        [HttpGet]
        [Route("AnnunciCurrentUser")]
        [ResponseType(typeof(List<AnnunciDtoOutput>))]
        public List<AnnunciDtoOutput> GetAnnunciByUser(int pageNumber, int pageSize)
        {
            var id = new Guid(User.Identity.GetUserId());
            List<AnnunciDtoOutput> annunci = dbDataContext.Annuncio
                                        .Include(x => x.Comuni)
                                        .Where(x => x.IdUtente == id)
                                        .OrderByDescending(x => x.DataCreazione)
                                        .Skip(pageSize*(pageNumber-1))
                                        .Take(pageSize)
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

            //FIXME MOMENTANEAMENTE COMMENTATO PER NON FARE DOWNLOAD DI TROPPI DATI, SCOMMENTARE PER AVERE LE IMMAGINI
            //annunci.ForEach(x =>
            //{
            //    var img = dbDataContext.ImmagineAnnuncio.Where(i => i.IdAnnuncio == x.Id).Select(i => i.Immagine).FirstOrDefault();
            //    x.ImmaginePrincipale = img;
            //});

            return annunci;
        }

        // GET api/Annunci/AnnuncioById/?id=1
        [HttpGet]
        [Route("AnnuncioById")]
        [ResponseType(typeof(AnnuncioDtoOutput))]
        public AnnuncioDtoOutput GetAnnuncioById(Guid Id)
        {
            Annuncio annuncio = dbDataContext.Annuncio
                                        .Include(x => x.Comuni)
                                        .Include(x => x.ImmagineAnnuncio)
                                        .Include(x => x.TipologiaAnnuncio)
                                        .Include(x => x.TipologiaProprieta)
                                        .Include(x => x.TipologiaRiscaldamento)
                                        .SingleOrDefaultAsync(x => x.Id == Id).Result;

            AnnuncioDtoOutput dto = AnnuncioDtoOutput.MapperAnnuncio(annuncio);
            return dto;
        }

        // GET api/Annunci/Annunci
        [HttpGet]
        [Route("Preferiti")]
        [ResponseType(typeof(List<AnnunciDtoOutput>))]
        public async Task<List<AnnunciDtoOutput>> GetAnnunciPreferitiAsync()
        {
            var id = new Guid(User.Identity.GetUserId());

            List<Guid> idAnnunciPreferiti = await dbDataContext.AnnunciPreferiti
                                    .Where(x => x.IdUser == id)
                                    .Select(x => x.IdAnnuncio).ToListAsync();

            List<AnnunciDtoOutput> annunci = dbDataContext.Annuncio
                                                .Include(x => x.Comuni)
                                                .Where(x => idAnnunciPreferiti.Contains(x.Id))
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

        // GET api/Annunci/AnnuncioById/?id=1
        [HttpPost]
        [Route("AggiungiPreferito")]
        public async Task<IHttpActionResult> AggiungiPreferito(Guid IdAnnuncio)
        {
            var id = new Guid(User.Identity.GetUserId());

            AnnunciPreferiti preferito = new AnnunciPreferiti()
            {
                Id = Guid.NewGuid(),
                IdAnnuncio = IdAnnuncio,
                IdUser = id
            };

            dbDataContext.AnnunciPreferiti.Add(preferito);

            await dbDataContext.SaveChangesAsync();
            return Ok(preferito);
        }

        // GET api/Annunci/AnnuncioById/?id=1
        [HttpPost]
        [Route("RimuoviPreferito")]
        public async Task<IHttpActionResult> RimuoviPreferito(Guid IdAnnuncio)
        {
            var id = new Guid(User.Identity.GetUserId());
            
            var annuncio= dbDataContext.AnnunciPreferiti.Where(x => x.IdAnnuncio == IdAnnuncio & x.IdUser == id).FirstOrDefault();
            dbDataContext.AnnunciPreferiti.Remove(annuncio);

            await dbDataContext.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Annunci/AnnuncioCreate
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
                DataScadenza =  DateTime.Now.AddMonths(3),
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

            if (Annuncio.ImmaginePlanimetria != null)
            {
                //Creo l'immaginePlanimetria
                ImmaginePlanimetria immaginePlanimetria = new ImmaginePlanimetria()
                {
                    Id = Guid.NewGuid(),
                    Immagine = Annuncio.ImmaginePlanimetria,
                    IdAnnuncio = annuncio.Id
                };

                dbDataContext.ImmaginePlanimetria.Add(immaginePlanimetria);
            }

            if (Annuncio.Video != null)
            {
                //Creo l'immaginePlanimetria
                Video video = new Video()
                {
                    Id = Guid.NewGuid(),
                    IdAnnuncio = annuncio.Id,
                    VideoBytes = Annuncio.Video
                };

                dbDataContext.Video.Add(video);
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

        // PUT: api/Evento/EventoUpdate/1
        [HttpPut]
        [Route("AnnuncioUpdate/{IdEvento:Guid}")]
        [ResponseType(typeof(AnnuncioDtoOutput))]
        public async Task<IHttpActionResult> UpdateAnnuncio([FromUri]Guid IdAnnuncio, [FromBody]AnnuncioDtoInput Annuncio)
        {
            //Controllo che i parametri siano valorizzati
            if (Annuncio == null || !ModelState.IsValid || (IdAnnuncio == null || IdAnnuncio == Guid.Empty))
            {
                return BadRequest(ModelState);
            }

            //Cerco l'annuncio
            Annuncio annuncio = await dbDataContext.Annuncio.Include(x => x.ImmagineAnnuncio).Where(x => x.Id == IdAnnuncio).FirstOrDefaultAsync();
            if (annuncio == null)
            {
                return NotFound();
            }

            //Modifico l'annuncio
            annuncio.Ascensore = Annuncio.Ascensore;
            annuncio.Balcone = Annuncio.Balcone;
            annuncio.Cancellato = Annuncio.Cancellato;
            annuncio.Cantina = Annuncio.Cantina;
            annuncio.Completato = Annuncio.Completato;
            annuncio.ComuneCodice = Annuncio.CodiceComune;
            annuncio.Condizionatori = Annuncio.Condizionatori;
            annuncio.DataModifica = new DateTime();
            annuncio.Descrizione = Annuncio.Descrizione;
            annuncio.Disponibile = Annuncio.Disponibile;
            annuncio.Giardino = Annuncio.Giardino;
            annuncio.IdStatoProprieta = Annuncio.IdStatoProprieta;
            annuncio.IdTipologiaAnnuncio = Annuncio.IdTipologiaAnnuncio;
            annuncio.IdTipologiaProprieta = Annuncio.IdTipologiaProprieta;
            annuncio.IdTipologiaRiscaldamento = Annuncio.IdTipologiaRiscaldamento;
            //FIXME GESTIRE IMMAGINI annuncio.
            annuncio.Indirizzo = Annuncio.Indirizzo;
            annuncio.NumeroAltreStanze = Annuncio.NumeroAltreStanze;
            annuncio.NumeroBagni = Annuncio.NumeroBagni;
            annuncio.NumeroCameraLetto = Annuncio.NumeroCameraLetto;
            annuncio.NumeroCucine = Annuncio.NumeroCucine;
            annuncio.NumeroGarage = Annuncio.NumeroGarage;
            annuncio.NumeroPostiAuto = Annuncio.NumeroPostiAuto;
            annuncio.Piano = Annuncio.Piano;
            annuncio.Piscina = Annuncio.Piscina;
            annuncio.Prezzo = Annuncio.Prezzo;
            annuncio.SpesaMensileCondominio = Annuncio.SpesaMensileCondominio;
            annuncio.Superficie = Annuncio.Superficie;
            annuncio.UltimoPiano = Annuncio.UltimoPiano;
            annuncio.IdClasseEnergetica = Annuncio.IdClasseEnergetica;

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

            foreach (Guid idImmagineToDelete in Annuncio.IdsImmaginiToDelete) {
                ImmagineAnnuncio imm = await dbDataContext.ImmagineAnnuncio.Where(x => x.Id == idImmagineToDelete).FirstOrDefaultAsync();
                dbDataContext.ImmagineAnnuncio.Remove(imm);
            }

            if (Annuncio.ImmaginePlanimetria != null)
            {
                ImmaginePlanimetria imm;
                imm = await dbDataContext.ImmaginePlanimetria.Where(x => x.IdAnnuncio == annuncio.Id).FirstOrDefaultAsync();
                if (imm == null) {
                    imm = new ImmaginePlanimetria();
                    imm.Id = new Guid();
                }
                imm.IdAnnuncio = annuncio.Id;
                imm.Immagine = Annuncio.ImmaginePlanimetria;
            }

            if (Annuncio.Video != null)
            {
                Video vid;
                vid = await dbDataContext.Video.Where(x => x.IdAnnuncio == annuncio.Id).FirstOrDefaultAsync();
                if (vid == null)
                {
                    vid = new Video();
                    vid.Id = new Guid();
                }
                vid.IdAnnuncio = annuncio.Id;
                vid.VideoBytes = Annuncio.Video;
            }

            try
            {
                //Salvo le modifiche sul DB.
                await dbDataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               throw;
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(AnnuncioDtoOutput.MapperAnnuncio(annuncio));
            //CreatedAtRoute("UpdateEvento", new { id = evento.Id }, evento);
        }

        // DELETE: api/Evento/AnnuncioDelete/5
        //[HttpDelete]
        //[Route("AnnuncioDelete/{id}")]
        //[ResponseType(typeof(AnnuncioDtoOutput))]
        //public async Task<IHttpActionResult> DeleteAnnuncio(Guid Id)
        //{
        //    Annuncio annuncio = await dbDataContext.Annuncio
        //                                        .Include(x => x.Regalo)
        //                                        .Include(x => x.Regalo.Select(y => y.ImmagineRegalo))
        //                                        .Include(x => x.ImmagineAnnuncio)
        //                                        .FirstOrDefaultAsync(x => x.Id == Id);
        //    if (annuncio == null)
        //    {
        //        return NotFound();
        //    }

        //    if (annuncio.ImmagineAnnuncio != null)
        //    {
        //        dbDataContext.ImmagineAnnuncio.Remove(annuncio.ImmagineAnnuncio);
        //    }

        //    if (annuncio.Regalo != null)
        //    {
        //        List<Guid> guidRegali = new List<Guid>();
        //        foreach (Regalo reg in evento.Regalo)
        //        {
        //            //guidRegali.Add(reg.Id);
        //            if (reg.ImmagineRegalo != null)
        //            {
        //                dbDataContext.ImmagineRegalo.Remove(reg.ImmagineRegalo);
        //            }
        //        }

        //        dbDataContext.Regalo.RemoveRange(evento.Regalo);
        //    }

        //    dbDataContext.annuncio.Remove(annuncio);
        //    dbDataContext.SaveChanges();

        //    return Ok();
        //}

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
            List<Comuni> listaComuni = new List<Comuni>();

            if (!(string.IsNullOrEmpty(NomeComune)))
            {
               listaComuni = dbDataContext.Comuni
                                        .Where(x => x.NomeComune.ToUpper().Contains(NomeComune))
                                        .Take(30)
                                        .ToList();
            }

            return listaComuni;
        }

        // GET api/values
        [HttpGet]
        [Route("ListaClasseEnergetica")]
        [ResponseType(typeof(List<ClasseEnergetica>))]
        public List<ClasseEnergetica> GetListaClasseEnergetica()
        {
            List<ClasseEnergetica> listaClasseEnergetica = new List<ClasseEnergetica>();

            listaClasseEnergetica = dbDataContext.ClasseEnergetica
                                        .Where(x => x.Abilitato == true)
                                        .ToList();

            return listaClasseEnergetica;
        }

        // GET api/values
        [HttpGet]
        [Route("ListaTipologiaRiscaldamento")]
        [ResponseType(typeof(List<TipologiaRiscaldamento>))]
        public List<TipologiaRiscaldamento> GetListaTipologiaRiscaldamento()
        {
            List<TipologiaRiscaldamento> listaTipologiaRiscaldamento = new List<TipologiaRiscaldamento>();

            listaTipologiaRiscaldamento = dbDataContext.TipologiaRiscaldamento.ToList();

            return listaTipologiaRiscaldamento;
        }
    }
}
