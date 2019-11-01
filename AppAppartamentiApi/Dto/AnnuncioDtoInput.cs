using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppAppartamentiApi.Dto
{
    public class AnnuncioDtoInput
    {

        [Required]
        [StringLength(128)]
        public string Comune { get; set; }

        [Required]
        [StringLength(512)]
        public string Indirizzo { get; set; }

        public double? Prezzo { get; set; }

        public double? Superficie { get; set; }

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

        [StringLength(8000)]
        public string Descrizione { get; set; }

        public double? SpesaMensileCondominio { get; set; }

        public Guid? IdTipologiaRiscaldamento { get; set; }

        public Guid? IdTipologiaProprieta { get; set; }

        public Guid? IdTipologiaAnnuncio { get; set; }

        public Guid? IdStatoProprieta { get; set; }

        public Guid? IdClasseEnergetica { get; set; }

        public bool Condizionatori { get; set; }

        public bool Completato { get; set; }

        public bool Cancellato { get; set; }

        [Required]
        public List<byte[]> Immagini { get; set; }
    }
}