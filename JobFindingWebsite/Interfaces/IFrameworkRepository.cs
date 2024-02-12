using JobFindingWebsite.Models;
using JobFindingWebsite.ViewModels;

namespace JobFindingWebsite.Interfaces
{
    public interface IFrameworkRepository
    {
        public Task<List<Framework>> GetAllFrameworks();
        public Task<List<Framework>> GetFrameworksOfVacancy(Vacancy vacancy);
        public Task<List<int>> GetFrameworkIDsOfVacancy(Vacancy vacancy);
        public Task<List<VacancyFramework>> GetFrameworksOfVacancyViewModel(EditVacancyViewModel vacancyVM);
        public Task<Framework?> GetFrameworkWithID(int Id);
        public Task<bool> ClearVacancyFrameworks(Vacancy vacancy, List<Framework> languages);
        public bool Save();
    }
}
