using System;
using System.ComponentModel.DataAnnotations;

namespace AppAppartamentiWebCoreMvc.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string BirthName { get; set; }

        public byte[] ImmagineProfilo { get; set; }

        public DateTime? DataNascita { get; set; }
    }
}
