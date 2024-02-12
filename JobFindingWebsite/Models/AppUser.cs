using System.ComponentModel.DataAnnotations.Schema;

namespace JobFindingWebsite.Models
{
    public class AppUser : Account
    {
        public string? CV { get; set; }
        [NotMapped]
        public IFormFile? CvPDF { get; set; }
        public string? GitHubURL { get; set; }
        public IEnumerable<AppliedVacancies>? AppliedVacancies { get; set; }
        public IEnumerable<SavedVacancies>? SavedVacancies { get; set; }
    }
}
