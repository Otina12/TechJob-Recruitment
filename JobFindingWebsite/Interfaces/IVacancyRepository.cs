
using JobFindingWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFindingWebsite.Interfaces
{
    public interface IVacancyRepository
    {
        public Task<List<Vacancy>> GetAllVacancies();
        public Task<List<Vacancy>> GetVacanciesOfCompany(string CompanyId);
        public Task<Vacancy?> GetVacancyById(int VacancyId);
        public bool IncrementViewCount(Vacancy vacancy);
        public Task<List<string?>>? GetAllLocations();
        public bool Add(Vacancy vacancy);
        public bool Update(Vacancy vacancy);
        public bool Remove(Vacancy vacancy);
        public bool Save();
    }
}
