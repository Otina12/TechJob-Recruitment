using JobFindingWebsite.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFindingWebsite.Models
{
    public class AppliedVacancies
    {
        public Vacancy Vacancy { get; set; } // vacancy is first since we will use it more when it comes to applications
        public int VacancyId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public StatusType Status { get; set; } = StatusType.Pending;
    }
}
