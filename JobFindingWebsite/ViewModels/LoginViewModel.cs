using System.ComponentModel.DataAnnotations;

namespace JobFindingWebsite.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter the password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
