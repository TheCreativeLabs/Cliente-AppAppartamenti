using System;

namespace AppAppartamentiWebCoreMvc.Models
{
    public class FilterModalViewModel
    {
        public Guid IdTipologiaProprieta { get; set; }

        public Guid IdTipologiaAnnuncio { get; set; }

        public int PrezzoMin { get; set; }

        public int PrezzoMax { get; set; }

        public int DimensioneMin { get; set; }

        public int DimensioneMax { get; set; }

        public int NumeroBagni { get; set; }

        public int NumeroAltreStanze { get; set; }

        public int NumeroCamereLetto { get; set; }

        public int NumeroCucine { get; set; }

        public int NumeroGarage { get; set; }

        public int NumeroPostiAuto { get; set; }

        public bool Ascensore { get; set; }

        public bool Cantina { get; set; }

        public bool Giardino { get; set; }

        public bool Piscina { get; set; }
    }
}
