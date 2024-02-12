using JobFindingWebsite.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace JobFindingWebsite.Models
{
    public class Company : Account
    {
        public CompanyType CompanyType { get; set; }
        [Required]
        public int NumberOfEmployees { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
    }
}
