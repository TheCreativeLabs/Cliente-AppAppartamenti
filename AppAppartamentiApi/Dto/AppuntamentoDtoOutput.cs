namespace AppAppartamentiApi
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class AppuntamentoDtoOutput
    {
        [Required]
        public Guid IdAppuntamento { get; set; }

        [Required]
        public Guid IdAnnuncio { get; set; }

        public Guid IdRichiedente { get; set; }

        public Guid IdDestinatario { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public string NameAndSurnamePersonToMeet { get; set; }
        
        [Required]
        public string Indirizzo { get; set; }

        public int? CodiceComune { get; set; }

        [Required]
        public string NomeComune { get; set; }

        public bool Confermato { get; set; }

    }
}
