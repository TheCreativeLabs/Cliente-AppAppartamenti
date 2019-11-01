namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatoProprieta")]
    public partial class StatoProprieta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StatoProprieta()
        {
            Annuncios = new HashSet<Annuncio>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Descrizione { get; set; }

        public bool Abilitato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Annuncio> Annuncios { get; set; }
    }
}