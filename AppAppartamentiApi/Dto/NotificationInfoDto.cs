namespace AppAppartamentiApi
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class NotificationInfoDto
    {
        [Required]
        public Guid InstallationId { get; set; }

        [Required]
        [StringLength(16)]
        public string OsVersion { get; set; }
    }
}
