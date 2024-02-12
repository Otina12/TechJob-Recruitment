using JobFindingWebsite.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JobFindingWebsite.Models;

namespace JobFindingWebsite.ViewModels
{
    public class CreateVacancyViewModel
    {
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Salary { get; set; }
        public string? City { get; set; }
        [Required]
        public JobType JobType { get; set; }
        [Required]
        public HoursType HoursType { get; set; }
        public SeniorityCategory Seniority { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
        
        [Required]
        public List<int> ProgrammingLanguages { get; set; }
        [Required]
        public List<int> Frameworks { get; set; }
    }
}
