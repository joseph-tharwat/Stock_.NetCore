using Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockProj.Data.Identity;
using StockTrading.Models;

namespace StockProj.Controllers
{
    [Route("[Controller]/[Action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        { 
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountModel account)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = account.UserName
                };
                IdentityResult result = await _userManager.CreateAsync(user, account.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(account);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountModel account)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser? user = await _userManager.FindByNameAsync(account.UserName);
                if (user == null)
                {
                    return View(account);
                }
                bool isCorrect = await _userManager.CheckPasswordAsync(user, account.Password);
                if (isCorrect == true)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(account);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
