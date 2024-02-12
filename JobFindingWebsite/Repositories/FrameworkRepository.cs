using JobFindingWebsite.Data;
using JobFindingWebsite.Interfaces;
using JobFindingWebsite.Models;
using JobFindingWebsite.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobFindingWebsite.Repositories
{
    public class FrameworkRepository : IFrameworkRepository
    {
        private readonly ApplicationDbContext _context;

        public FrameworkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Framework>> GetAllFrameworks()
        {
            return await _context.Frameworks.ToListAsync();
        }

        public async Task<List<int>> GetFrameworkIDsOfVacancy(Vacancy vacancy)
        {
            var frameworks = await _context.VacancyFrameworks
                .Where(vf => vf.VacancyId == vacancy.Id)
                .Select(x => x.FrameworkId).ToListAsync();
            return frameworks;
        }

        public async Task<List<Framework>> GetFrameworksOfVacancy(Vacancy vacancy)
        {
            return await _context.VacancyFrameworks
                .Where(vf => vf.VacancyId == vacancy.Id)
                .Select(x => x.Framework).ToListAsync();
        }

        public async Task<List<VacancyFramework>> GetFrameworksOfVacancyViewModel(EditVacancyViewModel vacancyVM)
        {
            var result = new List<VacancyFramework>();
            var frameworkIds = vacancyVM.Frameworks;
            var vacancy = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == vacancyVM.Id);

            foreach (var Id in frameworkIds)
            {
                var framework = await GetFrameworkWithID(Id);
                var vf = new VacancyFramework()
                {
                    VacancyId = vacancyVM.Id,
                    Vacancy = vacancy!,
                    FrameworkId = Id,
                    Framework = framework!
                };
                result.Add(vf);
            }
            return result;
        }

        public async Task<Framework?> GetFrameworkWithID(int Id)
        {
            var framework = await _context.Frameworks.FirstOrDefaultAsync(f => f.Id == Id);
            return framework;
        }

        public async Task<bool> ClearVacancyFrameworks(Vacancy vacancy, List<Framework> frameworks)
        {
            if(frameworks.IsNullOrEmpty()) { return false; }

            var allFrameworks = await _context.VacancyFrameworks.Where(vf => vacancy.Id == vacancy.Id).ToListAsync();

            foreach(var framework in frameworks)
            {
                var vf = new VacancyFramework()
                {
                    VacancyId = vacancy.Id,
                    Vacancy = vacancy,
                    FrameworkId = framework.Id,
                    Framework = framework
                };

                if (allFrameworks.Contains(vf))
                {
                    _context.VacancyFrameworks.Remove(vf);
                }
            }

            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
