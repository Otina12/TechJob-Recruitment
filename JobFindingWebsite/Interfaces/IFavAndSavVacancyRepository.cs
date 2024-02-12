using JobFindingWebsite.Data.Enum;
using JobFindingWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFindingWebsite.Interfaces
{
    public interface IFavAndSavVacancyRepository
    {
        public bool addToSaved(AppUser user, Vacancy vacancy);
        public bool addToApplied(AppUser user, Vacancy vacancy);
        public bool removeFromSaved(AppUser user, Vacancy vacancy);
        public bool removeFromApplied(AppUser user, Vacancy vacancy);
        public Task<IEnumerable<Vacancy>> getSavedVacancies(AppUser user);
        public Task<IEnumerable<Vacancy>> getAppliedVacancies(AppUser user);
        public Task<IEnumerable<AppUser>> getAllApplicantsOfVacancy(Vacancy vacancy);
        public Task<IEnumerable<AppUser>> getAllSaversOfVacancy(Vacancy vacancy);
        public Task<StatusType?> getStatusOfApplicant(AppUser user, Vacancy vacancy);
        public Task<bool> setStatusOfApplicant(AppUser user, Vacancy vacancy, StatusType status);
        public bool Save();
    }
}
