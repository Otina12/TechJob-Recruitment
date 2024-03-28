using JobFindingWebsite.Data;
using JobFindingWebsite.Interfaces;
using JobFindingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobFindingWebsite.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> getAllCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> getAllUsers()
        {
            return await _context.AppUsers.ToListAsync();
        }

        public async Task<Company?> getCompanyById(string Id)
        {
            return await _context.Companies.Include(c => c.Vacancies).FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<AppUser?> getUserById(string Id)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<Account?> getAccountById(string Id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(c => c.Id == Id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Account account)
        {
            _context.Update(account);
            return Save();
        }

        public bool UpdateUser(AppUser user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UpdateCompany(Company company)
        {
            _context.Update(company);
            return Save();
        }
    }
}
