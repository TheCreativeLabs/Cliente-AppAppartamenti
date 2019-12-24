namespace AppAppartamentiApi
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class UserInfoDto
    {

        [StringLength(256)]
        public string Nome { get; set; }

        [StringLength(256)]
        public string Cognome { get; set; }

        public DateTime? DataDiNascita { get; set; }

        public byte[] FotoProfilo { get; set; }

        [Required]
        public Guid IdAspNetUser { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }

        public Guid? InstallationId { get; set; }

        [StringLength(16)]
        public string OsVersion { get; set; }
    }
}
