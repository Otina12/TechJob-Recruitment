using JobFindingWebsite.Models;
using JobFindingWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobFindingWebsite.Interfaces
{
    public interface ILanguageRepository
    {
        public Task<List<ProgrammingLanguage>> GetAllLanguages();
        public Task<List<ProgrammingLanguage>> GetLanguagesOfVacancy(Vacancy vacancy);
        public Task<List<VacancyProgrammingLanguage>> GetLanguagesOfVacancyViewModel(EditVacancyViewModel vacancyVM);
        public Task<List<int>> GetLanguageIDsOfVacancy(Vacancy vacancy);
        public Task<ProgrammingLanguage?> GetLanguageWithID(int Id);
        public Task<bool> ClearVacancyLanguages(Vacancy vacancy, List<ProgrammingLanguage> languages);
        public bool Save();

    }
}
