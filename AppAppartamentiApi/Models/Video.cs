namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Video")]
    public partial class Video
    {
        public Guid Id { get; set; }

        [Column("Video")]
        [Required]
        public byte[] VideoBytes { get; set; }

        public Guid IdAnnuncio { get; set; }

        public virtual Annuncio Annuncio { get; set; }
    }
}
