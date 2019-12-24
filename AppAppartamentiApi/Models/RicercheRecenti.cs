namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RicercheRecenti")]
    public partial class RicercheRecenti
    {
        public Guid Id { get; set; }

        public Guid IdAspNetUser { get; set; }

        public int CodiceComune { get; set; }

        public Comuni Comune { get; set; }

        public UserInfo UserInfo { get; set; }
    }
}
