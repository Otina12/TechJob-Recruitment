using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobFindingWebsite.ViewModels;
using JobFindingWebsite.Interfaces;
using JobFindingWebsite.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobFindingWebsite.Data;
using JobFindingWebsite.Data.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobFindingWebsite.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IVacancyRepository _vacancyRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IFrameworkRepository _frameworkRepository;
        private readonly IFavAndSavVacancyRepository _favAndSav;
        private readonly IAccountRepository _accountRepository;

        public VacancyController(IVacancyRepository vacancyRepository, ILanguageRepository languageRepository,
            IFrameworkRepository frameworkRepository, IAccountRepository accountRepository, IFavAndSavVacancyRepository favAndSav)
        {
            _vacancyRepository = vacancyRepository;
            _languageRepository = languageRepository;
            _frameworkRepository = frameworkRepository;
            _favAndSav = favAndSav;
            _accountRepository = accountRepository;
        }

        // Note to self: In the future, add user skills (languages and frameworks) and order vacancies by which vacancies more apply to user.
        public async Task<IActionResult> Index()
        {
            var allLocations = await _vacancyRepository.GetAllLocations()!;
            var allLocationsMinusNull = allLocations.Where(v => v != "");
            var programmingLanguages = await _languageRepository.GetAllLanguages();
            var frameworks = await _frameworkRepository.GetAllFrameworks();
            ViewBag.LanguagesList = new SelectList(programmingLanguages, "Id", "Name");
            ViewBag.FrameworksList = new SelectList(frameworks, "Id", "Name");
            ViewBag.AllLocations = new SelectList(allLocationsMinusNull);
            var allVacancies = await _vacancyRepository.GetAllVacancies();
            return View(allVacancies);
        }

        [Authorize(Roles = "company")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var programmingLanguages = await _languageRepository.GetAllLanguages();
            var frameworks = await _frameworkRepository.GetAllFrameworks();
            ViewBag.LanguagesList = new SelectList(programmingLanguages, "Id", "Name");
            ViewBag.FrameworksList = new SelectList(frameworks, "Id", "Name");

            return View();
        }

        [Authorize(Roles = "company")]
        [HttpPost]
        public IActionResult Create(CreateVacancyViewModel vacancyVM)
        {
            if (!ModelState.IsValid)
            {
                return View(vacancyVM);
            }

            var newVacancy = new Vacancy()
            {
                Title = vacancyVM.Title,
                Description = vacancyVM.Description.Replace("\r\n", "<br />"),
                Salary = vacancyVM.Salary == null ? "" : vacancyVM.Salary.ToString(),
                City = vacancyVM.City == null ? "" : vacancyVM.City,
                JobType = vacancyVM.JobType,
                HoursType = vacancyVM.HoursType,
                Seniority = vacancyVM.Seniority,
                ExpireDate = vacancyVM.ExpireDate,
                ProgrammingLanguages = vacancyVM.ProgrammingLanguages.
                Select(Id => new VacancyProgrammingLanguage { ProgrammingLanguageId = Id }).ToList(),
                Frameworks = vacancyVM.Frameworks.
                Select(Id => new VacancyFramework { FrameworkId = Id }).ToList(),
                CompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Applicants = new List<AppliedVacancies>(),
                Savers = new List<SavedVacancies>()
            };

            _vacancyRepository.Add(newVacancy);

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var programmingLanguages = await _languageRepository.GetAllLanguages();
            var frameworks = await _frameworkRepository.GetAllFrameworks();
            ViewBag.LanguagesList = new SelectList(programmingLanguages, "Id", "Name");
            ViewBag.FrameworksList = new SelectList(frameworks, "Id", "Name");

            var vacancy = await _vacancyRepository.GetVacancyById(Id);
            if (vacancy == null) { return View("Error"); }

            var curCompanyId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (curCompanyId != vacancy.CompanyId)
            {
                return View("Error");
            }

            var editVacancyVM = new EditVacancyViewModel()
            {
                Id = vacancy.Id,
                Title = vacancy.Title,
                Description = vacancy.Description.Replace("<br />", "\r\n"),
                Salary = vacancy.Salary == null ? "" : vacancy.Salary.ToString(),
                City = vacancy.City == null ? "" : vacancy.City.ToString(),
                JobType = vacancy.JobType,
                HoursType = vacancy.HoursType,
                Seniority = vacancy.Seniority,
                ExpireDate = vacancy.ExpireDate,
                ProgrammingLanguages = await _languageRepository.GetLanguageIDsOfVacancy(vacancy),
                Frameworks = await _frameworkRepository.GetFrameworkIDsOfVacancy(vacancy)
            };

            
            return View(editVacancyVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditVacancyViewModel vacancyVM)
        {
            if (!ModelState.IsValid)
            {
                return View(vacancyVM);
            }

            var vacancy = await _vacancyRepository.GetVacancyById(vacancyVM.Id);

            if (!vacancyVM.ProgrammingLanguages.IsNullOrEmpty())
            {
                var languages = new List<ProgrammingLanguage?>();
                var frameworks = new List<Framework?>();

                foreach (var id in vacancyVM.ProgrammingLanguages)
                {
                    var language = await _languageRepository.GetLanguageWithID(id);
                    languages.Add(language);
                }
                foreach (var id in vacancyVM.ProgrammingLanguages)
                {
                    var framework = await _frameworkRepository.GetFrameworkWithID(id);
                    frameworks.Add(framework);
                }
                await _languageRepository.ClearVacancyLanguages(vacancy, languages!);
                await _frameworkRepository.ClearVacancyFrameworks(vacancy, frameworks!);
            }

            vacancy.Id = vacancyVM.Id;
            vacancy.Title = vacancyVM.Title;
            vacancy.Description = vacancyVM.Description.Replace("\r\n", "<br />");
            vacancy.Salary = vacancyVM.Salary == null ? "" : vacancyVM.Salary.ToString();
            vacancy.City = vacancyVM.City == null ? "" : vacancyVM.City.ToString();
            vacancy.JobType = vacancyVM.JobType;
            vacancy.HoursType = vacancyVM.HoursType;
            vacancy.Seniority = vacancyVM.Seniority;
            vacancy.ExpireDate = vacancyVM.ExpireDate;
            vacancy.CompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            vacancy.ProgrammingLanguages = await _languageRepository.GetLanguagesOfVacancyViewModel(vacancyVM);
            vacancy.Frameworks = await _frameworkRepository.GetFrameworksOfVacancyViewModel(vacancyVM);
                
            _vacancyRepository.Update(vacancy);

            return RedirectToAction("Details", "Vacancy", new { id = vacancyVM.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var vacancy = await _vacancyRepository.GetVacancyById(Id);
            _vacancyRepository.IncrementViewCount(vacancy);
            var languages = await _languageRepository.GetLanguagesOfVacancy(vacancy);
            var frameworks = await _frameworkRepository.GetFrameworksOfVacancy(vacancy);

            var isApplied = false;
            var isSaved = false;
            var curUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("user") && curUserId != null)
            {
                var curUser = await _accountRepository.GetUserById(curUserId);
                isApplied = (await _favAndSav.GetAppliedVacancies(curUser!)).Contains(vacancy);
                isSaved = (await _favAndSav.GetSavedVacancies(curUser!)).Contains(vacancy);
            }

            var vacancyDetailsVM = new VacancyDetailsViewModel()
            {
                Vacancy = vacancy,
                Languages = languages,
                Frameworks = frameworks,
                isApplied = isApplied,
                isSaved = isSaved
            };

            return View(vacancyDetailsVM);

        }

        [HttpGet]
        [Authorize(Roles = "company")]
        public async Task<IActionResult> Delete(int Id)
        {
            var vacancy = await _vacancyRepository.GetVacancyById(Id);
            var curCompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (curCompanyId != vacancy.CompanyId) { return View("Error"); }

            return View(vacancy);
        }

        [HttpPost]
        [Authorize(Roles = "company")]
        public async Task<IActionResult> Delete(Vacancy vacancy)
        {
            var curCompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allWhoSaved = await _favAndSav.GetAllSaversOfVacancy(vacancy);
            var allWhoApplied = await _favAndSav.GetAllApplicantsOfVacancy(vacancy);
            foreach (var user in allWhoSaved)
            {
                _favAndSav.RemoveFromSaved(user, vacancy);
            }
            foreach (var user in allWhoApplied)
            {
                _favAndSav.RemoveFromApplied(user, vacancy);
            }

            _vacancyRepository.Remove(vacancy);

            return RedirectToAction("Company", "Profile", new { id = curCompanyId });
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Favorites(string Id)
        {
            var curUser = await _accountRepository.GetUserById(Id);
            var vacancies = await _favAndSav.GetSavedVacancies(curUser);
            return View(vacancies);
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Applied(string Id)
        {
            var curUser = await _accountRepository.GetUserById(Id);
            var vacancies = await _favAndSav.GetAppliedVacancies(curUser);
            return View(vacancies);

        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> SaveToFavorites(int Id)
        {
            var curUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _accountRepository.GetUserById(curUserId!);
            var vacancy = await _vacancyRepository.GetVacancyById(Id);

            if (curUser != null)
            {
                _favAndSav.AddToSaved(curUser, vacancy);
            }

            return RedirectToAction("Favorites", "Vacancy", new { id = curUserId });
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> RemoveFromFavorites(int Id)
        {
            var curUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _accountRepository.GetUserById(curUserId!);
            var vacancy = await _vacancyRepository.GetVacancyById(Id);

            if (curUser != null)
            {
                _favAndSav.RemoveFromSaved(curUser, vacancy);
            }

            return RedirectToAction("Favorites", "Vacancy", new { id = curUserId });
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Apply(int Id)
        {
            var curUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _accountRepository.GetUserById(curUserId!);
            var vacancy = await _vacancyRepository.GetVacancyById(Id);

            if (curUser != null)
            {
                _favAndSav.AddToApplied(curUser, vacancy);
            }

            return RedirectToAction("Applied", "Vacancy", new { id = curUserId });
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Unapply(int Id)
        {
            var curUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curUser = await _accountRepository.GetUserById(curUserId!);
            var vacancy = await _vacancyRepository.GetVacancyById(Id);

            if (curUser != null)
            {
                _favAndSav.RemoveFromApplied(curUser, vacancy);
            }

            return RedirectToAction("Applied", "Vacancy", new { id = curUserId });
        }

        [HttpGet]
        [Authorize(Roles = "company")]
        public async Task<IActionResult> Analytics(int Id)
        {
            var curCompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vacancy = await _vacancyRepository.GetVacancyById(Id);

            if (vacancy.CompanyId != curCompanyId)
            {
                return View("Error");
            }

            var applicants = await _favAndSav.GetAllApplicantsOfVacancy(vacancy);

            var analyticsVM = new VacancyAnalyticsViewModel()
            {
                Vacancy = vacancy,
                Applicants = new List<(AppUser, StatusType)>()
            };

            foreach (var user in applicants)
            {
                (AppUser, StatusType) application = (user, (StatusType)await _favAndSav.GetStatusOfApplicant(user, vacancy));

                analyticsVM.Applicants.Add(application);
            }

            return View(analyticsVM);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeStatus(string userId, int vacancyId, string statusString)
        {
            StatusType status = (StatusType)Enum.Parse(typeof(StatusType), statusString);
            var user = await _accountRepository.GetUserById(userId);
            var vacancy = await _vacancyRepository.GetVacancyById(vacancyId);
            await _favAndSav.SetStatusOfApplicant(user!, vacancy, status);

            return Json(new { success = true, redirectUrl = Url.Action("Analytics", "Vacancy", new { id = vacancyId }) });
        }

        // Language and Framework filtration is configured so that vacancy needs to have at least 1 language and at least 1 framework to be shown.
        // However, if only languages are selected (frameworks are not), then at least 1 language needs to be selected (frameworks don't matter) and vice versa.
        public async Task<IActionResult> Filter(string searchQuery, string languageIds, string frameworkIds, string location, string jobType, string hoursType, string seniority)
        {
            var query = await _vacancyRepository.GetAllVacancies();
            searchQuery = searchQuery != null ? searchQuery : "";
            
            if(searchQuery != "")
            query = query.Where(v => v.Title.ToLower().Contains(searchQuery) || v.Description.ToLower().Contains(searchQuery)).ToList();

            List<int> languageIdList = new List<int>();
            List<int> frameworkIdList = new List<int>();
            if (languageIds != null && !languageIds.Contains("any"))
            {
                languageIdList = !string.IsNullOrEmpty(languageIds) ? languageIds.Split(',').Select(int.Parse).ToList() : new List<int>();
            }
            if (frameworkIds != null && !frameworkIds.Contains("any"))
            {
                frameworkIdList = !string.IsNullOrEmpty(frameworkIds) ? frameworkIds.Split(',').Select(int.Parse).ToList() : new List<int>();
            }

            var vacanciesToRemove = new List<Vacancy>();

            if (!languageIdList.IsNullOrEmpty() && !languageIds!.Contains("any"))
            {
                foreach (var vacancy in query)
                {
                    var languagesOfVacancy = await _languageRepository.GetLanguageIDsOfVacancy(vacancy);
                    if (languagesOfVacancy.IsNullOrEmpty()) { continue; }
                    else if (!languagesOfVacancy.Intersect(languageIdList).Any())
                    {
                        vacanciesToRemove.Add(vacancy);
                    }
                }
            }

            foreach (var vacancyToRemove in vacanciesToRemove)
            {
                query.Remove(vacancyToRemove);
            }

            vacanciesToRemove = new List<Vacancy>();

            if (!frameworkIdList.IsNullOrEmpty() && !frameworkIds!.Contains("any"))
            {
                foreach (var vacancy in query)
                {
                    var frameworksOfVacancy = await _frameworkRepository.GetFrameworkIDsOfVacancy(vacancy);
                    if (frameworksOfVacancy.IsNullOrEmpty()) { continue; }
                    else if (!frameworksOfVacancy.Intersect(frameworkIdList).Any())
                    {
                        vacanciesToRemove.Add(vacancy);
                    }
                }
            }

            foreach (var vacancyToRemove in vacanciesToRemove)
            {
                query.Remove(vacancyToRemove);
            }

            if (!string.IsNullOrEmpty(location) && location != "any")
            {
                query = query.Where(v => v.City?.ToString() == location || v.City?.ToLower().ToString() == "").ToList();
            }

            if (!string.IsNullOrEmpty(jobType))
            {
                query = query.Where(v => v.JobType.ToString() == jobType).ToList();
            }

            if (!string.IsNullOrEmpty(hoursType))
            {
                query = query.Where(v => v.HoursType.ToString() == hoursType).ToList();
            }

            if (!string.IsNullOrEmpty(seniority))
            {
                query = query.Where(v => v.Seniority.ToString() == seniority).ToList();
            }

            return PartialView("_VacanciesPartial", query);
        }
    }
}
