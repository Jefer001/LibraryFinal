using Library.DAL;
using Library.DAL.Entities;
using Library.Enum;
using Library.Helpers;
using Library.Models;
using Microsoft.AspNetCore.Identity;
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

        #region Register actions
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            AddUserViewModel addUserViewModel = new()
            {
                Id = Guid.Empty,
                Universities = await _dropDownListHelper.GetDDLUniversitiesAsync(),
                UserType = UserType.User,
            };

            return View(addUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel addUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (addUserViewModel.ImageFile != null)
                    imageId = await _azureBlobHelper.UploadAzureBlobAsync(addUserViewModel.ImageFile, "users");

                addUserViewModel.ImageId = imageId;
                addUserViewModel.CreatedDate = DateTime.Now;

                User user = await _userHelpers.AddUserAsync(addUserViewModel);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    await FillDropDownListLocation(addUserViewModel);

                    return View(addUserViewModel);
                }

                //Autologeamos al nuevo usuario que se registra
                LoginViewModel loginViewModel = new()
                {
                    Password = addUserViewModel.Password,
                    Username = addUserViewModel.Username,
                    RememberMe = false
                };

                var login = await _userHelpers.LoginAsync(loginViewModel);

                if (login.Succeeded) return RedirectToAction("Index", "Home");
            }
            await FillDropDownListLocation(addUserViewModel);

            return View(addUserViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser()
        {
            User user = await _userHelpers.GetUserAsync(User.Identity.Name);
            if (user == null) return NotFound();

            EditUserViewModel editUserViewModel = new()
            {
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Universities = await _dropDownListHelper.GetDDLUniversitiesAsync(),
                UniversityId = user.University.Id,
                Id = Guid.Parse(user.Id)
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = editUserViewModel.ImageId;
                if (editUserViewModel.ImageFile != null) imageId = await _azureBlobHelper.UploadAzureBlobAsync(editUserViewModel.ImageFile, "users");

                User user = await _userHelpers.GetUserAsync(User.Identity.Name);

                user.FirstName = editUserViewModel.FirstName;
                user.LastName = editUserViewModel.LastName;
                user.Document = editUserViewModel.Document;
                user.Address = editUserViewModel.Address;
                user.PhoneNumber = editUserViewModel.PhoneNumber;
                user.ImageId = imageId;
                user.University = await _context.Universities.FindAsync(editUserViewModel.UniversityId);

                IdentityResult result = await _userHelpers.UpdateUserAsync(user);
                if (result.Succeeded) return RedirectToAction("Index", "Home");
                else ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
            }
            await FillDropDownListLocation(editUserViewModel);
            return View(editUserViewModel);
        }
        #endregion

        #region Change Password actions
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                if (changePasswordViewModel.OldPassword.Equals(changePasswordViewModel.NewPassword))
                {
                    ModelState.AddModelError(string.Empty, "Debes ingresar una contraseña diferente");
                    return View(changePasswordViewModel);
                }
                User user = await _userHelpers.GetUserAsync(User.Identity?.Name);
                if (user != null)
                {
                    IdentityResult result = await _userHelpers.ChangePasswordAsync(user, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
                    if (result.Succeeded) return RedirectToAction("EditUser");
                    else ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                }
                else ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
            }
            return View(changePasswordViewModel);
        }
        #endregion

        #region Private methods
        private async Task FillDropDownListLocation(AddUserViewModel addUserViewModel)
        {
            addUserViewModel.Universities = await _dropDownListHelper.GetDDLUniversitiesAsync();
        }

        private async Task FillDropDownListLocation(EditUserViewModel editUserViewModel)
        {
            editUserViewModel.Universities = await _dropDownListHelper.GetDDLUniversitiesAsync();
        }
        #endregion
    }
}