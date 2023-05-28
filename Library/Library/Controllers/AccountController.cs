using Library.DAL;
using Library.Helpers;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IUserHelper _userHelpers;
        private readonly IDropDownListHelper _dropDownListHelper;
        private readonly IAzureBlobHelper _azureBlobHelper;
        #endregion

        #region Builder
        public AccountController(IUserHelper userHelpers, DataBaseContext context, IDropDownListHelper dropDownListHelper, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            _userHelpers = userHelpers;
            _dropDownListHelper = dropDownListHelper;
            _azureBlobHelper = azureBlobHelper;
        }
        #endregion

        #region Login actions
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelpers.LoginAsync(loginViewModel);

                if (result.Succeeded) return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");

            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelpers.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
