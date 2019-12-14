using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppAppartamentiApi.Dto
{
    public class AnnuncioDtoInput
    {
        [Required]
        [DisplayName("Codice comune")]
        public int CodiceComune { get; set; }

        [Required]
        [StringLength(512)]
        public string Indirizzo { get; set; }

        [Required]
        public double Prezzo { get; set; }

        [Required]
        public double Superficie { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool UltimoPiano { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Piano { get; set; }

        [Required]
        [DefaultValue(0)]
        public int NumeroCameraLetto { get; set; }

        [Required]
        [DefaultValue(0)]
        public int NumeroAltreStanze { get; set; }

        [Required]
        [DefaultValue(0)]
        public int NumeroBagni { get; set; }

        [Required]
        [DefaultValue(0)]
        public int NumeroCucine { get; set; }

        [Required]
        [DefaultValue(0)]
        public int NumeroGarage { get; set; }

        [Required]
        [DefaultValue(0)]
        public int NumeroPostiAuto { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Giardino { get; set; }

        [Required]
        [DefaultValue(false)]

        public bool Disponibile { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Ascensore { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Balcone { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Piscina { get; set; }
        
        [Required]
        [DefaultValue(false)]
        public bool Cantina { get; set; }

        [StringLength(8000)]
        public string Descrizione { get; set; }

        [Required]
        public double SpesaMensileCondominio { get; set; }

        [Required]
        public Guid IdTipologiaRiscaldamento { get; set; }

        [Required]
        public Guid IdTipologiaProprieta { get; set; }

        [Required]
        public Guid IdTipologiaAnnuncio { get; set; }

        public Guid? IdStatoProprieta { get; set; }

        [Required]
        public Guid IdClasseEnergetica { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Condizionatori { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Completato { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Cancellato { get; set; }

        [Required]
        public List<byte[]> Immagini { get; set; }

        public List<byte[]> ImmaginePlanimetria { get; set; }

        public byte[] Video { get; set; }

        [Required]
        public string CoordinateGeografiche { get; set; }

        public DisponibilitaOrariaDtoInput DisponibilitaOraria { get; set; }
    }
}