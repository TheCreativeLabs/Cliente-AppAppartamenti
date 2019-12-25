namespace AppAppartamentiApi
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MessaggioDto
    {

        [Required]
        public Guid IdAnnuncio { get; set; }


        /// <summary>
        /// In input possiamo anche non passarlo perchè è il proprietario dell'annuncio e lo possiamo ricavare tramite idAnnuncio
        /// </summary>
        public Guid IdUserDestinatario { get; set; }

        [StringLength(8000)]
        public string Messaggio { get; set; }

        /// <summary>
        /// Se IdChat è null, il messaggio è il primo di una nuova chat e verrà creata la chat
        /// </summary>
        public Guid? IdChat { get; set; }
    }
}
