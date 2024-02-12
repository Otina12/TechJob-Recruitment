
using JobFindingWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFindingWebsite.Interfaces
{
    public interface IVacancyRepository
    {
        public Task<List<Vacancy>> getAllVacancies();
        public List<Vacancy> getVacanciesOfCompany(string CompanyId);
        public Task<Vacancy> getVacancyById(int VacancyId);
        public bool IncrementViewCount(Vacancy vacancy);
        public Task<List<string?>>? getAllLocations();
        public bool Add(Vacancy vacancy);
        public bool Update(Vacancy vacancy);
        public bool Remove(Vacancy vacancy);
        public bool Save();
    }
}
