namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FasceOrarie")]
    public partial class FasceOrarie
    {
        public Guid Id { get; set; }

        [StringLength(1024)]
        public string Monday { get; set; }

        [StringLength(1024)]
        public string Tuesday { get; set; }

        [StringLength(1024)]
        public string Wednesday { get; set; }

        [StringLength(1024)]
        public string Thursday { get; set; }

        [StringLength(1024)]
        public string Friday { get; set; }

        [StringLength(1024)]
        public string Saturday { get; set; }

        [StringLength(1024)]
        public string Sunday { get; set; }

        public Guid IdAnnuncio { get; set; }

        public virtual Annuncio Annuncio { get; set; }
}
}
