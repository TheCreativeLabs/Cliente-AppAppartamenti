namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comuni")]
    public partial class Comuni
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Comuni()
        {
            Annuncio = new HashSet<Annuncio>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodiceComune { get; set; }

        [Required]
        [StringLength(255)]
        public string NomeComune { get; set; }

        [Required]
        [StringLength(255)]
        public string CodiceRegione { get; set; }

        [Required]
        [StringLength(255)]
        public string NomeRegione { get; set; }

        [StringLength(255)]
        public string CodiceUnitaTerritoriale { get; set; }

        [StringLength(255)]
        public string CodiceProvincia { get; set; }

        [StringLength(255)]
        public string ProgressivoComune { get; set; }

        [StringLength(255)]
        public string CodiceComuneAlfanumerico { get; set; }

        public int? CodiceRipartizioneGeografica { get; set; }

        [StringLength(255)]
        public string DenominazioneUnitaTerritorialeSovracomunale { get; set; }

        [StringLength(255)]
        public string CodiceCatastaleComune { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Annuncio> Annuncio { get; set; }
    }
}
