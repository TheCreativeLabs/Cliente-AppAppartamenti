namespace AppAppartamentiApi
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MessaggioDto
    {
        /// <summary>
        /// In input possiamo anche non passarlo perchè è il proprietario dell'annuncio e lo possiamo ricavare tramite idAnnuncio
        /// </summary>
        public Guid IdDestinatario { get; set; }

        public Guid IdMittente { get; set; }

        [StringLength(8000)]
        public string Messaggio { get; set; }

        public DateTime DataInserimento { get; set; }

        public DateTime? DataLettura { get; set; }

        public bool FromMe { get; set; }
    }
}
