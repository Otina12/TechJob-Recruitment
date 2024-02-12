using JobFindingWebsite.Models;

namespace JobFindingWebsite.ViewModels
{
    public class VacancyDetailsViewModel
    {
        public Vacancy Vacancy { get; set; }
        public List<ProgrammingLanguage>? Languages { get; set; }
        public List<Framework>? Frameworks { get; set; }
        public bool isApplied { get; set; }
        public bool isSaved { get; set; }
    }
}
