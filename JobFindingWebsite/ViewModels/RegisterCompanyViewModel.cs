using JobFindingWebsite.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace JobFindingWebsite.ViewModels
{
    public class RegisterCompanyViewModel
    {
        [Display(Name = "Company Type")]
        [Required(ErrorMessage = "Company type is required")]
        public CompanyType CompanyType {  get; set; }
        [Display(Name = "Number of Employees")]
        [Required(ErrorMessage = "Number of employees is required")]
        public int NumberOfEmployees { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter the password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirming password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
