using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFindingWebsite.Models
{
    public class Account : IdentityUser
    {
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? LinkedInURL { get; set; }
        public bool isCompany { get; set; }
    }
}
