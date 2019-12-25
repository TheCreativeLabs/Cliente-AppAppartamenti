namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnnuncioMessaggi")]
    public partial class AnnuncioMessaggi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        public Guid IdAnnuncio { get; set; }

        [Required]
        public Guid IdUserMittente { get; set; }

        [Required]
        public Guid IdUserDestinatario { get; set; }

        [StringLength(8000)]
        public string Messaggio { get; set; }

        [Required]
        public DateTime DataInserimento { get; set; }

        public bool Letto { get; set; }

        [Required]
        public DateTime DataLettura { get; set; }

        public virtual Annuncio Annuncio { get; set; }

        [Required]
        public Guid IdChat { get; set; }

        public virtual Chat Chat { get; set; }
    }
}



