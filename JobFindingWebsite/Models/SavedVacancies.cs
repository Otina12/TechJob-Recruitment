using System.ComponentModel.DataAnnotations.Schema;

namespace JobFindingWebsite.Models
{
    public class SavedVacancies
    {
        public string AppUserId { get; set; } // AppUser is first since we will use it more when it comes to favorites
        public AppUser AppUser { get; set; }
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
    }
}
