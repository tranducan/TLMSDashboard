using System.ComponentModel.DataAnnotations;

namespace TLMSDashboard.Models
{
    public class LoginViewModel
    {
        [Required]
        public string User { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Remember me?")]
        public bool RememberMe { get; set;}

        [EmailAddress]
        public string Email { get; set; }
    }
}
