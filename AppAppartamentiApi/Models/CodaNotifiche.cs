namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CodaNotifiche")]
    public partial class CodaNotifiche
    {
        public Guid Id { get; set; }

        public Guid IdRichiedente { get; set; }

        public Guid IdDestinatario { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        [Required]
        [StringLength(128)]
        public string Message { get; set; }

        public bool Sent { get; set; }

        public DateTime? SentDate { get; set; }
    }
}
