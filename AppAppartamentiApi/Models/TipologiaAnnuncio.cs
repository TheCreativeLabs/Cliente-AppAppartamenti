namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipologiaAnnuncio")]
    public partial class TipologiaAnnuncio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipologiaAnnuncio()
        {
            Annuncio = new HashSet<Annuncio>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Descrizione { get; set; }

        [Required]
        [StringLength(32)]
        public string Codice { get; set; }

        public bool Abilitato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Annuncio> Annuncio { get; set; }
    }
}
