namespace AppAppartamentiApi
{
    using AppAppartamentiApi.Dto;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class AppuntamentoDettaglioDtoOutput
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

        public byte[] ImagePersonToMeet { get; set; }

        public bool Confermato { get; set; }

        public AnnunciDtoOutput InfoAnnuncio { get; set; }

        public string CoordinateGeograficheAnnuncio { get; set; }

    }
}
