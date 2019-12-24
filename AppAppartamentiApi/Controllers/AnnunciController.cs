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

        private int recentiSize = 5;

        public enum AnnunciOrder
        {
            PRICE_ASC = 0,
            PRICE_DESC = 1,
            SIZE_ASC =2,
            SIZE_DESC = 3,
            CREATION_DATE_ASC = 4,
            CREATION_DATE_DESC = 5
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
        public async Task<List<AnnunciDtoOutput>> GetAnnunci(int pageNumber, int pageSize,
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
                                                       int? otherRooms = null,
                                                       bool? backyard = null,
                                                       bool? terrace = null,
                                                       bool? cellar = null,
                                                       bool? pool = null,
                                                       bool? elevator = null,
                                                       bool? airConditioners = null,
                                                       AnnunciOrder? orderBy = null)
        { //FIXME CONDIZIONATORI + ALTRE STANZE

            Guid idCurrent = new Guid(User.Identity.GetUserId());

            //List<AnnunciDtoOutput> annunci = dbDataContext.Annuncio
            IQueryable<Annuncio> query = dbDataContext.Annuncio
                                            .Include(x => x.Comuni)
                                            .Where(x => (idTipologiaAnnuncio == null || x.IdTipologiaAnnuncio == idTipologiaAnnuncio)
                                                & (idTipologiaProprieta == null || x.IdTipologiaProprieta == idTipologiaProprieta)
                                                & (comuneCodice == null || x.ComuneCodice == comuneCodice)
                                                & (priceMin == null || x.Prezzo >= priceMin)
                                                & (priceMax == null || priceMax >= 500000 || x.Prezzo <= priceMax)
                                                & (houseSizeMin == null || x.Superficie >= houseSizeMin)
                                                & (houseSizeMax == null || houseSizeMax >= 300 || x.Superficie <= houseSizeMax)
                                                & (bedrooms == null || x.NumeroCameraLetto >= bedrooms)
                                                & (bathrooms == null || x.NumeroBagni >= bathrooms)
                                                & (kitchens == null || x.NumeroCucine >= kitchens)
                                                & (parkingSpaces == null || x.NumeroPostiAuto >= parkingSpaces)
                                                & (garages == null || x.NumeroGarage >= garages)
                                                & (otherRooms == null || x.NumeroAltreStanze >= otherRooms)
                                                & (backyard == null || backyard == false || x.Giardino == backyard)
                                                & (terrace == null || terrace  == false || x.Balcone == terrace)
                                                & (cellar == null || cellar == false || x.Cantina == cellar)
                                                & (pool == null || pool == false || x.Piscina == pool)
                                                & (elevator == null || elevator == false || x.Ascensore == elevator)
                                                & (airConditioners == null || airConditioners == false || x.Condizionatori == airConditioners)
                                                & x.IdUtente != idCurrent
                                        );

            if (orderBy == null || AnnunciOrder.CREATION_DATE_ASC == orderBy)
            {
                    query = query.OrderBy(x => x.DataCreazione);
            }
            else if(AnnunciOrder.CREATION_DATE_DESC == orderBy)
            {
                query = query.OrderByDescending(x => x.DataCreazione);
            } 
            else if( AnnunciOrder.PRICE_ASC == orderBy)
            {
                    query = query.OrderBy(x => x.Prezzo);
            }
            else if(AnnunciOrder.PRICE_DESC == orderBy)
            {
                query = query.OrderByDescending(x => x.Prezzo);
            }
            else if (AnnunciOrder.SIZE_ASC == orderBy)
            {
                    query = query.OrderBy(x => x.Superficie);
            }
            else if (AnnunciOrder.SIZE_DESC == orderBy)
            {
                query = query.OrderByDescending(x => x.Superficie);
            }
            
            List<AnnunciDtoOutput> annunci = query
                                       //.OrderBy(x =>  x.DataCreazione) 
                                        .Skip(pageSize * (pageNumber - 1))
                                        .Take(pageSize)
                                        //.Join(dbDataContext.AnnunciPreferiti, // the source table of the inner join
                                        //          annuncio => annuncio.Id,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                                        //          prefe => prefe.IdAnnuncio,   // Select the foreign key (the second part of the "on" clause)
                                        //          (annuncio, prefe) => new { Annuncio = annuncio, AnnunciPreferiti = prefe }) // selection
                                        //       .Where(postAndMeta => postAndMeta.AnnunciPreferiti.IdUser == idCurrent)   // where statement
                                             .Select(annJoinPrefe => new AnnunciDtoOutput()
                                             {
                                                 Id = annJoinPrefe.Id,
                                                 IdUtente = annJoinPrefe.IdUtente,
                                                 DataCreazione = annJoinPrefe.DataCreazione,
                                                 DataModifica = annJoinPrefe.DataModifica,
                                                 CodiceComune = annJoinPrefe.ComuneCodice,
                                                 NomeComune = annJoinPrefe.Comuni.NomeComune,
                                                 Indirizzo = annJoinPrefe.Indirizzo,
                                                 Prezzo = annJoinPrefe.Prezzo,
                                                 Superficie = annJoinPrefe.Superficie,
                                                 Descrizione = annJoinPrefe.Descrizione,
                                                 TipologiaAnnuncio = annJoinPrefe.TipologiaAnnuncio.Descrizione,
                                                 TipologiaProprieta = annJoinPrefe.TipologiaProprieta.Descrizione,
                                                 Completato = annJoinPrefe.Completato,
                                                 Cancellato = annJoinPrefe.Cancellato,
                                                 FlagPreferito =  (dbDataContext.AnnunciPreferiti.Where(x => x.IdAnnuncio == annJoinPrefe.Id && x.IdUser == idCurrent).Any())//annJoinPrefe.AnnunciPreferiti != null ? true : false
                                             }).ToList();


            annunci.ForEach(x =>
            {
                x.ImmaginePrincipale = dbDataContext.ImmagineAnnuncio.Where(i => i.IdAnnuncio == x.Id).Select(i => i.Immagine).FirstOrDefault();
            });

            //salvo l'ultima ricerca dell'utente
            if(comuneCodice != null) { 
                RicercheRecenti ricercheRecenti = await dbDataContext.RicercheRecenti.Where(x => x.IdAspNetUser == idCurrent).FirstOrDefaultAsync();
                if (ricercheRecenti == null)
                {
                    ricercheRecenti = new RicercheRecenti()
                    {
                        Id = Guid.NewGuid(),
                        IdAspNetUser = idCurrent
                    };
                }
                ricercheRecenti.CodiceComune = (int) comuneCodice;
                await dbDataContext.SaveChangesAsync();
            }

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

            annunci.ForEach(x =>
            {
                var img = dbDataContext.ImmagineAnnuncio.Where(i => i.IdAnnuncio == x.Id).Select(i => i.Immagine).FirstOrDefault();
                x.ImmaginePrincipale = img;
            });

            return annunci;
        }

        // GET api/Annunci/AnnuncioById/?id=1
        [HttpGet]
        [Route("AnnuncioById")]
        [ResponseType(typeof(AnnuncioDtoOutput))]
        public async Task<AnnuncioDtoOutput> GetAnnuncioById(Guid Id)
        {
            Annuncio annuncio = await dbDataContext.Annuncio
                                        .Include(x => x.Comuni)
                                        .Include(x => x.ImmagineAnnuncio)
                                        .Include(x => x.TipologiaAnnuncio)
                                        .Include(x => x.TipologiaProprieta)
                                        .Include(x => x.TipologiaRiscaldamento)
                                        .Include(x => x.ClasseEnergetica)
                                        .Include(x => x.ImmaginiPlanimetria)
                                        .Include(x => x.FasceOrarie)
                                        .SingleOrDefaultAsync(x => x.Id == Id);

            var idCurrentUser = new Guid(User.Identity.GetUserId());

            AnnunciPreferiti preferiti = await dbDataContext.AnnunciPreferiti
                                            .Where(x => x.IdUser == idCurrentUser && x.IdAnnuncio == Id)
                                            .FirstOrDefaultAsync();
            bool preferito = preferiti != null;

            AnnuncioDtoOutput dto = AnnuncioDtoOutput.MapperAnnuncio(annuncio, preferito);

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

        [HttpGet]
        [Route("RicercheRecentiCurrent")]
        [ResponseType(typeof(List<AnnunciDtoOutput>))]
        public async Task<List<AnnunciDtoOutput>> GetRicercheRecentiCurrentAsync()
        {
            var idCurrent = new Guid(User.Identity.GetUserId());

            int codiceComuneLast = await dbDataContext.RicercheRecenti.Where(x => x.IdAspNetUser == idCurrent).Select(x => x.CodiceComune).FirstOrDefaultAsync();

            List<AnnunciDtoOutput> annunci = dbDataContext.Annuncio
                                                .Include(x => x.Comuni)
                                                .Include(x => x.ImmagineAnnuncio)
                                                .Where(x => x.ComuneCodice == codiceComuneLast && x.IdUtente != idCurrent)
                                                .OrderBy(x => x.DataCreazione)
                                                .Take(recentiSize)
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
                                                 Cancellato = annuncio.Cancellato,
                                                 ImmaginePrincipale = annuncio.ImmagineAnnuncio != null ? annuncio.ImmagineAnnuncio.First().Immagine : null
                                             }).ToList();

           return annunci;
        }

        // GET api/Annunci/AggiungiPreferito/?id=1
        [HttpPost]
        [Route("AggiungiPreferito")]
        public async Task<IHttpActionResult> AggiungiPreferito(Guid IdAnnuncio)
        {
            var id = new Guid(User.Identity.GetUserId());

            AnnunciPreferiti preferito = dbDataContext.AnnunciPreferiti.Where(x => x.IdUser == id && x.IdAnnuncio == IdAnnuncio).FirstOrDefault();

            if (preferito == null)
            {
                preferito = new AnnunciPreferiti()
                {
                    Id = Guid.NewGuid(),
                    IdAnnuncio = IdAnnuncio,
                    IdUser = id
                };

                dbDataContext.AnnunciPreferiti.Add(preferito);

                await dbDataContext.SaveChangesAsync();
            }

            return Ok(preferito);
        }

        // GET api/Annunci/RimuoviPreferito/?id=1
        [HttpPost]
        [Route("RimuoviPreferito")]
        public async Task<IHttpActionResult> RimuoviPreferito(Guid IdAnnuncio)
        {
            var id = new Guid(User.Identity.GetUserId());
            
            var annuncio= dbDataContext.AnnunciPreferiti.Where(x => x.IdAnnuncio == IdAnnuncio & x.IdUser == id).FirstOrDefault();
            if (annuncio != null) { 
                dbDataContext.AnnunciPreferiti.Remove(annuncio);

                await dbDataContext.SaveChangesAsync();
            }

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
                IdStatoProprieta = (Annuncio.IdStatoProprieta  == Guid.Empty ? null : Annuncio.IdStatoProprieta),
                IdClasseEnergetica = Annuncio.IdClasseEnergetica,
                Condizionatori = Annuncio.Condizionatori,
                Completato = Annuncio.Completato,
                Cancellato = Annuncio.Cancellato,
                CoordinateGeografiche = Annuncio.CoordinateGeografiche
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
                foreach (byte[] immagine in Annuncio.ImmaginePlanimetria)
                {
                    //Creo l'immaginePlanimetria
                    ImmaginePlanimetria immaginePlanimetria = new ImmaginePlanimetria()
                    {
                        Id = Guid.NewGuid(),
                        Immagine = immagine,
                        IdAnnuncio = annuncio.Id
                    };

                    dbDataContext.ImmaginePlanimetria.Add(immaginePlanimetria);
                }
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

            if (Annuncio.DisponibilitaOraria != null)
            {
                FasceOrarie fasceOrarie = new FasceOrarie();
                fasceOrarie.Id = Guid.NewGuid();
                fasceOrarie.IdAnnuncio = annuncio.Id;
                fasceOrarie.Monday = Annuncio.DisponibilitaOraria.fasceOrarieLunedi ?? null;
                fasceOrarie.Tuesday = Annuncio.DisponibilitaOraria.fasceOrarieMartedi ?? null;
                fasceOrarie.Wednesday = Annuncio.DisponibilitaOraria.fasceOrarieMercoledi ?? null;
                fasceOrarie.Thursday = Annuncio.DisponibilitaOraria.fasceOrarieGiovedi ?? null;
                fasceOrarie.Friday = Annuncio.DisponibilitaOraria.fasceOrarieVenerdi ?? null;
                fasceOrarie.Saturday = Annuncio.DisponibilitaOraria.fasceOrarieSabato ?? null;
                fasceOrarie.Sunday = Annuncio.DisponibilitaOraria.fasceOrarieDomenica ?? null;

                dbDataContext.FasceOrarie.Add(fasceOrarie);
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
        [Route("AnnuncioUpdate/{IdAnnuncio:Guid}")]
        [ResponseType(typeof(AnnuncioDtoOutput))]
        public async Task<IHttpActionResult> UpdateAnnuncio([FromUri]Guid IdAnnuncio, [FromBody]AnnuncioDtoInput Annuncio)
        {
            //Controllo che i parametri siano valorizzati
            if (Annuncio == null || !ModelState.IsValid || (IdAnnuncio == null || IdAnnuncio == Guid.Empty))
            {
                return BadRequest(ModelState);
            }

            //Cerco l'annuncio
            Annuncio annuncio = await dbDataContext.Annuncio
                                        .Include(x => x.ImmagineAnnuncio)
                                        .Include(x => x.ImmaginiPlanimetria)
                                        .Include(x => x.Video)
                                        .Include(x => x.Comuni)
                                        .Where(x => x.Id == IdAnnuncio).FirstOrDefaultAsync();
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
            annuncio.DataModifica = DateTime.Now;
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
            annuncio.CoordinateGeografiche = Annuncio.CoordinateGeografiche;

            //fixme gestione veloce, elimino e ricreo le immagini
            dbDataContext.ImmagineAnnuncio.RemoveRange(annuncio.ImmagineAnnuncio);

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

            //foreach (Guid idImmagineToDelete in Annuncio.IdsImmaginiToDelete) {
            //    ImmagineAnnuncio imm = await dbDataContext.ImmagineAnnuncio.Where(x => x.Id == idImmagineToDelete).FirstOrDefaultAsync();
            //    dbDataContext.ImmagineAnnuncio.Remove(imm);
            //}

            //fixme gestione veloce, elimino e ricreo le planimetrie
            dbDataContext.ImmaginePlanimetria.RemoveRange(annuncio.ImmaginiPlanimetria);

            if (Annuncio.ImmaginePlanimetria != null)
            {
                foreach (byte[] immagine in Annuncio.ImmaginePlanimetria)
                {
                    //Creo l'immagine
                    ImmaginePlanimetria immaginePlan= new ImmaginePlanimetria()
                    {
                        Id = Guid.NewGuid(),
                        Immagine = immagine,
                        IdAnnuncio = annuncio.Id
                    };

                    dbDataContext.ImmaginePlanimetria.Add(immaginePlan);
                }
            }


            if (Annuncio.Video != null)
            {
                //fixme gestione veloce
                dbDataContext.Video.RemoveRange(annuncio.Video);
                Video vid = new Video();
                vid.Id = Guid.NewGuid();
                vid.IdAnnuncio = annuncio.Id;
                vid.VideoBytes = Annuncio.Video;
                dbDataContext.Video.Add(vid);
            }

            if (Annuncio.DisponibilitaOraria != null)
            {
                FasceOrarie fasce;
                fasce = await dbDataContext.FasceOrarie.Where(x => x.IdAnnuncio == annuncio.Id).FirstOrDefaultAsync();
                if(fasce == null)
                {
                    fasce = new FasceOrarie()
                    {
                        Id = Guid.NewGuid(),
                        IdAnnuncio = annuncio.Id
                    };
                }
                fasce.Monday = Annuncio.DisponibilitaOraria.fasceOrarieLunedi ?? null;
                fasce.Tuesday = Annuncio.DisponibilitaOraria.fasceOrarieMartedi ?? null;
                fasce.Wednesday = Annuncio.DisponibilitaOraria.fasceOrarieMercoledi ?? null;
                fasce.Thursday = Annuncio.DisponibilitaOraria.fasceOrarieGiovedi ?? null;
                fasce.Friday = Annuncio.DisponibilitaOraria.fasceOrarieVenerdi ?? null;
                fasce.Saturday = Annuncio.DisponibilitaOraria.fasceOrarieSabato ?? null;
                fasce.Sunday = Annuncio.DisponibilitaOraria.fasceOrarieDomenica ?? null;

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

            return Ok(AnnuncioDtoOutput.MapperAnnuncio(annuncio, false)); //l'annuncio  è dell'utente corrente quindi non ha senso che sia tra suoi preferiti: passo sempre preferito = false
        }

        //DELETE: api/Evento/AnnuncioDelete/5
        [HttpDelete]
        [Route("AnnuncioDelete/{id}")]
        [ResponseType(typeof(AnnuncioDtoOutput))]
        public async Task<IHttpActionResult> DeleteAnnuncio(Guid Id)
        {
            Annuncio annuncio = await dbDataContext.Annuncio
                                                .Include(x => x.ImmagineAnnuncio)
                                                .Include(x => x.ImmaginiPlanimetria)
                                                .Include(x => x.Appuntamenti)
                                                .Include(x => x.Video)
                                                .Include(x => x.AnnuncioMessaggi)
                                                .FirstOrDefaultAsync(x => x.Id == Id);
            if (annuncio == null)
            {
                return NotFound();
            }

            if (annuncio.ImmagineAnnuncio != null)
            {
                dbDataContext.ImmagineAnnuncio.RemoveRange(annuncio.ImmagineAnnuncio);
            }

            if (annuncio.ImmaginiPlanimetria != null)
            {
                dbDataContext.ImmaginePlanimetria.RemoveRange(annuncio.ImmaginiPlanimetria);
            }

            if (annuncio.Appuntamenti != null)
            {
                dbDataContext.Appuntamento.RemoveRange(annuncio.Appuntamenti);
            }

            if (annuncio.Video != null)
            {
                dbDataContext.Video.RemoveRange(annuncio.Video);
            }

            if (annuncio.AnnuncioMessaggi != null)
            {
                dbDataContext.AnnuncioMessaggi.RemoveRange(annuncio.AnnuncioMessaggi);
            }


            dbDataContext.Annuncio.Remove(annuncio);
            dbDataContext.SaveChanges();

            return Ok();
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
        [ResponseType(typeof(List<ComuneDto>))]
        public List<ComuneDto> GetListaComuni(string NomeComune)
        {
            List<ComuneDto> listaComuni = new List<ComuneDto>();

            if (!(string.IsNullOrEmpty(NomeComune)))
            {
               listaComuni = dbDataContext.Comuni
                                        .Where(x => x.NomeComune.ToUpper().StartsWith(NomeComune.ToUpper()))
                                        .Take(1000)
                                        .Select(comune => new ComuneDto() { 
                                            NomeComune = comune.NomeComune,
                                            CodiceComune = comune.CodiceComune
                                        })
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
