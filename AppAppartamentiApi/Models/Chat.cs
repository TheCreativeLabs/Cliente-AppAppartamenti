namespace AppAppartamentiApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chat")]
    public partial class Chat
    {
        public Guid Id { get; set; }

        public Guid IdUserMittente { get; set; }

        public Guid IdUserDestinatario { get; set; }

        public Guid IdAnnuncio { get; set; }

        public DateTime DataCreazione { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnnuncioMessaggi> AnnuncioMessaggi { get; set; }

        public virtual UserInfo UserInfoMittente { get; set; }

        public virtual UserInfo UserInfoDestinatario { get; set; }
    }
}
