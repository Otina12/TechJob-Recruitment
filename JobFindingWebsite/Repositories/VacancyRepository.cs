using JobFindingWebsite.Data;
using JobFindingWebsite.Interfaces;
using JobFindingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobFindingWebsite.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly ApplicationDbContext _context;

        public VacancyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vacancy>> getAllVacancies()
        {
            return await _context.Vacancies.Include(v => v.Company).Where(v => DateTime.Compare(v.ExpireDate, DateTime.Now) > 0).ToListAsync();
        }

        public List<Vacancy> getVacanciesOfCompany(string Id)
        {
            return _context.Vacancies.Where(v => v.CompanyId == Id).Where(v => DateTime.Compare(v.ExpireDate, DateTime.Now) > 0).ToList();
        }

        public async Task<Vacancy?> getVacancyById(int VacancyId)
        {
            var vacancy = await _context.Vacancies.FirstOrDefaultAsync(x => x.Id == VacancyId);
            if(vacancy == null || DateTime.Compare(vacancy.ExpireDate, DateTime.Now) < 0)
            {
                return null;
            }

            return vacancy;
        }

        public bool IncrementViewCount(Vacancy vacancy)
        {
            _context.Vacancies.FirstOrDefault(v => v.Id == vacancy.Id)!.ViewCount += 1;
            return Save();
        }

        public Task<List<string?>>? getAllLocations()
        {
            var locations = _context.Vacancies.Select(x => x.City).Distinct().Where(c => c != null).ToListAsync();
            return locations;
        }
        public bool Add(Vacancy vacancy)
        {
            _context.Vacancies.Add(vacancy);
            return Save();
        }
        public bool Remove(Vacancy vacancy)
        {
            _context.Vacancies.Remove(vacancy);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Vacancy vacancy)
        {
            _context.Update(vacancy);
            return Save();
        }
    }
}
