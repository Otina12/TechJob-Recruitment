using JobFindingWebsite.Data;
using JobFindingWebsite.Interfaces;
using JobFindingWebsite.Models;
using JobFindingWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobFindingWebsite.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly ApplicationDbContext _context;

        public LanguageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProgrammingLanguage>> GetAllLanguages()
        {
            return await _context.ProgrammingLanguages.ToListAsync();
        }
        
        public async Task<List<int>> GetLanguageIDsOfVacancy(Vacancy vacancy)
        {
            var languages = await _context.VacancyProgrammingLanguages
               .Where(vpl => vpl.VacancyId == vacancy.Id)
               .Select(x => x.ProgrammingLanguageId).ToListAsync();
            return languages;
        }

        public async Task<List<ProgrammingLanguage>> GetLanguagesOfVacancy(Vacancy vacancy)
        {
            return await _context.VacancyProgrammingLanguages
                .Where(vpl => vpl.VacancyId == vacancy.Id)
                .Select(x => x.ProgrammingLanguage).ToListAsync();
        }
        public async Task<ProgrammingLanguage?> GetLanguageWithID(int Id)
        {
            var language = await _context.ProgrammingLanguages.FirstOrDefaultAsync(x => x.Id == Id);
            return language;
        }

        public async Task<List<VacancyProgrammingLanguage>> GetLanguagesOfVacancyViewModel(EditVacancyViewModel vacancyVM)
        {
            var vacancy = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == vacancyVM.Id);
            var result = new List<VacancyProgrammingLanguage>();
            var listOfIds = vacancyVM.ProgrammingLanguages;
            
            foreach (var Id in listOfIds )
            {
                var vpl = new VacancyProgrammingLanguage()
                {
                    VacancyId = vacancyVM.Id,
                    Vacancy = vacancy!,
                    ProgrammingLanguageId = Id,
                    ProgrammingLanguage = await _context.ProgrammingLanguages.FirstOrDefaultAsync(l => l.Id == Id)
                };
                result.Add(vpl);
            }

            return result;
        }

        public async Task<bool> ClearVacancyLanguages(Vacancy vacancy, List<ProgrammingLanguage>? languages)
        {
            if(languages.IsNullOrEmpty()) {  return false; }

            var vacancyLanguages = await _context.VacancyProgrammingLanguages.Where(vpl => vpl.VacancyId == vacancy.Id).ToListAsync();
            foreach (var language in languages)
            {
                var vpl = new VacancyProgrammingLanguage()
                {
                    VacancyId = vacancy.Id,
                    Vacancy = vacancy,
                    ProgrammingLanguageId = language.Id,
                    ProgrammingLanguage = language
                };

                if (vacancyLanguages.Contains(vpl))
                {
                    _context.VacancyProgrammingLanguages.Remove(vpl);
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
