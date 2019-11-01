namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnnunciPreferiti")]
    public partial class AnnunciPreferiti
    {
        public Guid Id { get; set; }

        public Guid IdUser { get; set; }

        public Guid IdAnnuncio { get; set; }

        public virtual Annuncio Annuncio { get; set; }
    }
}
