using JobFindingWebsite.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace JobFindingWebsite.ViewModels
{
    public class EditVacancyViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Salary { get; set; }
        public string? City { get; set; }
        [Required]
        public JobType JobType { get; set; }
        [Required]
        public HoursType HoursType { get; set; }
        [Required]
        public SeniorityCategory Seniority { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
        [Required]
        public List<int> ProgrammingLanguages { get; set; }
        [Required]
        public List<int> Frameworks { get; set; }
    }
}
