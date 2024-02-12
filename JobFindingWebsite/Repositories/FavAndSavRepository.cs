using JobFindingWebsite.Data;
using JobFindingWebsite.Interfaces;
using JobFindingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using JobFindingWebsite.Data.Enum;

namespace JobFindingWebsite.Repositories
{
    public class FavAndSavRepository : IFavAndSavVacancyRepository
    {
        private readonly ApplicationDbContext _context;

        public FavAndSavRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool addToSaved(AppUser user, Vacancy vacancy)
        {
            var savedVacancy = new SavedVacancies { AppUser = user, AppUserId = user.Id, Vacancy = vacancy, VacancyId = vacancy.Id };
            if(!_context.SavedVacancies.Contains(savedVacancy))
            {
                _context.SavedVacancies.Add(savedVacancy);
            }
            return Save();
        }

        public bool addToApplied(AppUser user, Vacancy vacancy)
        {
            var appliedVacancy = new AppliedVacancies { AppUser = user, AppUserId = user.Id, Vacancy = vacancy, VacancyId = vacancy.Id };
            if (!_context.AppliedVacancies.Contains(appliedVacancy))
            {
                _context.AppliedVacancies.Add(appliedVacancy);
            }
            return Save();
        }

        public bool removeFromSaved(AppUser user, Vacancy vacancy)
        {
            var savedVacancy = _context.SavedVacancies.FirstOrDefault(sv => sv.AppUserId == user.Id && sv.VacancyId == vacancy.Id);
            if(savedVacancy != null)
            {
                _context.SavedVacancies.Remove(savedVacancy);
                return Save();
            }

            return false;
        }

        public bool removeFromApplied(AppUser user, Vacancy vacancy)
        {
            var appliedVacancy = _context.AppliedVacancies.FirstOrDefault(sv => sv.AppUserId == user.Id && sv.VacancyId == vacancy.Id);
            if (appliedVacancy != null)
            {
                _context.AppliedVacancies.Remove(appliedVacancy);
                return Save();
            }

            return false;
        }

        public async Task<IEnumerable<Vacancy>> getSavedVacancies(AppUser user)
        {
            var result = await _context.SavedVacancies.Where(sv => sv.AppUserId == user.Id)
                .Include(sv => sv.Vacancy)
                .ThenInclude(v => v.Company)
                .Select(sv => sv.Vacancy).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Vacancy>> getAppliedVacancies(AppUser user)
        {
            var result = await _context.AppliedVacancies.Where(sv => sv.AppUserId == user.Id)
                .Include(sv => sv.Vacancy)
                .ThenInclude(v => v.Company)
                .Select(sv => sv.Vacancy).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<AppUser>> getAllApplicantsOfVacancy(Vacancy vacancy)
        {
            var result = await _context.AppliedVacancies
                .Where(av => av.VacancyId == vacancy.Id)
                .Select(av => av.AppUser).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<AppUser>> getAllSaversOfVacancy(Vacancy vacancy)
        {
            var result = await _context.SavedVacancies
                .Where(av => av.VacancyId == vacancy.Id)
                .Select(av => av.AppUser).ToListAsync();
            return result;
        }

        public async Task<StatusType?> getStatusOfApplicant(AppUser user, Vacancy vacancy)
        {
            var application = await _context.AppliedVacancies
                .FirstOrDefaultAsync(a => a.AppUserId == user.Id && a.VacancyId == vacancy.Id);

            if (application == null) return null;
            else return application.Status;
        }
        public async Task<bool> setStatusOfApplicant(AppUser user, Vacancy vacancy, StatusType status)
        {
            var application = await _context.AppliedVacancies
                .FirstOrDefaultAsync(a => a.AppUserId == user.Id && a.VacancyId == vacancy.Id);

            if (application == null)
            {
                return false;
            }

            application.Status = status;
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }


    }
}
