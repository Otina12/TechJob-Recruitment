using JobFindingWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFindingWebsite.Interfaces
{
    public interface IAccountRepository
    {
        public Task<IEnumerable<AppUser>> getAllUsers();
        public Task<AppUser?> getUserById(string Id);
        public Task<IEnumerable<Company>> getAllCompanies();
        public Task<Company?> getCompanyById(string Id);
        public Task<Account?> getAccountById(string Id);
        public bool Update(Account account);
        public bool UpdateUser(AppUser user);
        public bool UpdateCompany(Company company);
        public bool Save();

    }
}
