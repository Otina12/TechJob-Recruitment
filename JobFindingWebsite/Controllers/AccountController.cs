using JobFindingWebsite.Data;
using JobFindingWebsite.Models;
using JobFindingWebsite.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace JobFindingWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;

        public AccountController(UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            var user = await _userManager.FindByEmailAsync(registerVM.Email);

            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            string newUserId = "";

            if (user == null)
            {
                var newUser = new AppUser
                {
                    Email = registerVM.Email,
                    UserName = registerVM.Email.Split('@')[0],
                    isCompany = false
                };

                var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

                {
                    await _userManager.AddToRoleAsync(newUser, Roles.User);
                    await _signInManager.SignInAsync(newUser, false);
                    newUserId = newUser.Id;
                }
            }

            return RedirectToAction("AppUser", "Profile", new { id = newUserId });
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var user = await _userManager.FindByEmailAsync(loginVM.Email);

            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);

                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["Error"] = "Incorrect password. Please try again";
                    return View(loginVM);
                }
            }

            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginVM);
        }

        public IActionResult Logout()
        {
            var result = _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult RegisterCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCompany(RegisterCompanyViewModel registerCompanyVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            var company = await _userManager.FindByEmailAsync(registerCompanyVM.Email);

            if (company != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerCompanyVM);
            }

            string newCompanyId = "";

            if (company == null)
            {
                var newCompany = new Company
                {
                    CompanyType = registerCompanyVM.CompanyType,
                    NumberOfEmployees = registerCompanyVM.NumberOfEmployees,
                    Email = registerCompanyVM.Email,
                    isCompany = true,
                    UserName = registerCompanyVM.Email.Split('@')[0],
                };

                var newCompanyResponse = await _userManager.CreateAsync(newCompany, registerCompanyVM.Password);

                if (newCompanyResponse.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newCompany, Roles.Company);
                    await _signInManager.SignInAsync(newCompany, false);
                    newCompanyId = newCompany.Id;
                }
            }

            return RedirectToAction("Company", "Profile", new { id = newCompanyId });
        }
    }
}
