using JobFindingWebsite.Data.Enum;
using JobFindingWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFindingWebsite.Interfaces
{
    public interface IFavAndSavVacancyRepository
    {
        public bool AddToSaved(AppUser user, Vacancy vacancy);
        public bool AddToApplied(AppUser user, Vacancy vacancy);
        public bool RemoveFromSaved(AppUser user, Vacancy vacancy);
        public bool RemoveFromApplied(AppUser user, Vacancy vacancy);
        public Task<IEnumerable<Vacancy>> GetSavedVacancies(AppUser user);
        public Task<IEnumerable<Vacancy>> GetAppliedVacancies(AppUser user);
        public Task<IEnumerable<AppUser>> GetAllApplicantsOfVacancy(Vacancy vacancy);
        public Task<IEnumerable<AppUser>> GetAllSaversOfVacancy(Vacancy vacancy);
        public Task<StatusType?> GetStatusOfApplicant(AppUser user, Vacancy vacancy);
        public Task<bool> SetStatusOfApplicant(AppUser user, Vacancy vacancy, StatusType status);
        public bool Save();
    }
}
