namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Appuntamento")]
    public partial class Appuntamento
    {
        public Guid Id { get; set; }

        public Guid IdAnnuncio { get; set; }

        public Guid IdRichiedente { get; set; }

        public Guid IdDestinatario { get; set; }

        public DateTime Data { get; set; }

        public bool Confermato { get; set; }

        public virtual Annuncio Annuncio { get; set; }
}
}
