using JobFindingWebsite.Interfaces;
using JobFindingWebsite.Models;
using JobFindingWebsite.Services;
using JobFindingWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobFindingWebsite.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountRepository _accountRepository;

        public ProfileController(IWebHostEnvironment webHostEnvironment, IAccountRepository accountRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Company(string Id)
        {
            var company = _accountRepository.getCompanyById(Id);
            return View(company);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AppUser(string Id)
        {
            var user = _accountRepository.getUserById(Id);
            return View(user);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string Id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId != Id)
            {
                return View("Error");
            }

            EditAccountViewModel viewModel = new EditAccountViewModel();
            EditUserViewModel editUserVM = new EditUserViewModel();
            EditCompanyViewModel editCompanyVM = new EditCompanyViewModel();

            if (User.IsInRole("user"))
            {
                var user = _accountRepository.getUserById(Id);

                editUserVM = new EditUserViewModel()
                {
                    CV = user.CV,
                    CVpdf = user.CvPDF,
                    GitHubURL = user.GitHubURL,
                };
            }
            else if (User.IsInRole("company"))
            {
                var company = _accountRepository.getCompanyById(Id);

                editCompanyVM = new EditCompanyViewModel()
                {
                    Industry = company.CompanyType,
                    NumberOfEmployees = company.NumberOfEmployees
                };
            }

            var account = _accountRepository.getAccountById(Id);


            viewModel = new EditAccountViewModel()
            {
                User = editUserVM,
                Company = editCompanyVM,
                UserName = account.UserName!,
                LinkedInURL = account.LinkedInURL,
                Description = account.Description?.Replace("<br />", "\r\n"),
                ImageUrl = account.ImagePath

            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditAccountViewModel editAccountVM)
        {
            if (!ModelState.IsValid) { return View(editAccountVM); }

            bool curUserisUser = User.IsInRole("user");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string pfpFolder = "";
            string cvFolder = "";

            if (editAccountVM.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(editAccountVM.Image.FileName);
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profile", "pfps", fileName);
                await editAccountVM.Image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                string relativePath = $"/profile/pfps/{fileName}";

                pfpFolder = relativePath;
            }

            if (User.IsInRole("user"))
            {
                if (editAccountVM.User?.CVpdf != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(editAccountVM.User?.CVpdf.FileName);
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profile", "CVs", fileName);
                    await editAccountVM.User!.CVpdf.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    string relativePath = $"/profile/CVs/{fileName}";

                    cvFolder = relativePath;
                }
            }

            if (User.IsInRole("user"))
            {
                var user = _accountRepository.getUserById(currentUserId!);

                if (user == null)
                {
                    return NotFound();
                }
                user.UserName = editAccountVM.UserName;
                user.Description = editAccountVM.Description?.Replace("\r\n", "<br />");
                user.LinkedInURL = editAccountVM.LinkedInURL;
                user.GitHubURL = editAccountVM?.User?.GitHubURL;
                if (editAccountVM!.Image != null) user.ImagePath = pfpFolder;
                if (editAccountVM.User!.CVpdf != null) user.CV = cvFolder;

                _accountRepository.UpdateUser(user);
            }
            else if (User.IsInRole("company"))
            {
                var company = _accountRepository.getCompanyById(currentUserId!);
                if (company == null)
                {
                    return NotFound();
                }

                company.UserName = editAccountVM.UserName;
                company.Description = editAccountVM.Description?.Replace("\r\n", "<br />");
                company.LinkedInURL = editAccountVM.LinkedInURL;
                if (editAccountVM.Image != null) company.ImagePath = pfpFolder;
                company.CompanyType = editAccountVM.Company!.Industry;
                company.NumberOfEmployees = editAccountVM.Company.NumberOfEmployees;

                _accountRepository.UpdateCompany(company);
            }
            if (curUserisUser) { return RedirectToAction("AppUser", "Profile", new { id = currentUserId }); }
            else return RedirectToAction("Company", "Profile", new { id = currentUserId });
        }

    }
}
