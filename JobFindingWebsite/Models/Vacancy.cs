using JobFindingWebsite.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFindingWebsite.Models
{
    public class Vacancy
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Salary { get; set; }
        public string? City { get; set; }
        public JobType JobType { get; set; }
        public HoursType HoursType { get; set; }
        public SeniorityCategory Seniority { get; set; }
        public DateTime ExpireDate { get; set; }
        public int ViewCount { get; set; }
        public List<VacancyProgrammingLanguage>? ProgrammingLanguages { get; set; }
        public List<VacancyFramework>? Frameworks { get; set; }
        [ForeignKey("Company")]
        public string CompanyId { get; set; } = null!;
        public Company Company { get; set; } = null!;
        public IEnumerable<AppliedVacancies>? Applicants { get; set; }
        public IEnumerable<SavedVacancies>? Savers { get; set; }

    }
}
