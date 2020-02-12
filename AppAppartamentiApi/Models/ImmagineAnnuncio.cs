namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ImmagineAnnuncio")]
    public partial class ImmagineAnnuncio
    {
        public Guid Id { get; set; }

        [Required]
        public byte[] Immagine { get; set; }

        public Guid IdAnnuncio { get; set; }

        public virtual Annuncio Annuncio { get; set; }
    }
}
