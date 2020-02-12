namespace AppAppartamentiApi
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class AppuntamentoDto
    {

        [Required]
        public Guid IdAnnuncio { get; set; }

        //serve nell'output, nell'input consideriamo il current quindi non deve arrivare da FE
        public Guid? IdRichiedente { get; set; }

        //serve nell'outpur, nell'input va bene passarlo ma se non presente lo ricaviamo dal proprietario dell'annuncio
        public Guid? IdDestinatario { get; set; }

        [Required]
        public DateTime Data { get; set; }

    }
}
