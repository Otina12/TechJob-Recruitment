using JobFindingWebsite.Interfaces;
using JobFindingWebsite.Models;
using JobFindingWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobFindingWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IAccountRepository _accountRepository;

        public HomeController(ILogger<HomeController> logger, IVacancyRepository vacancyRepository, IAccountRepository accountRepository)
        {
            _logger = logger;
            _vacancyRepository = vacancyRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var allUsers = await _accountRepository.GetAllUsers();
            var numOfUsers = allUsers.Count();
            var allCompanies = await _accountRepository.GetAllCompanies();
            var numOfCompanies = allCompanies.Count();
            var allVacancies = await _vacancyRepository.GetAllVacancies();
            var numOfVacancies = allVacancies.Count();

            var homeVM = new HomeNumbersViewModel()
            {
                numOfUsers = numOfUsers,
                numOfCompanies = numOfCompanies,
                numOfVacancies = numOfVacancies
            };

            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}