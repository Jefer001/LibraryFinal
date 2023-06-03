using Library.DAL;
using Library.DAL.Entities;
using Library.Enum;
using Library.Helpers;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class UsersController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IUserHelper _userHelpers;
        private readonly IDropDownListHelper _dropDownListHelper;
        private readonly IAzureBlobHelper _azureBlobHelper;
        #endregion

        #region Builder
        public UsersController(DataBaseContext context, IUserHelper userHelpers, IDropDownListHelper dropDownListHelper, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            _userHelpers = userHelpers;
            _dropDownListHelper = dropDownListHelper;
            _azureBlobHelper = azureBlobHelper;
        }
        #endregion
        
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users
                        .Include(u => u.University)
                        .ToListAsync()) :
                        Problem("Entity set 'DataBaseContext.Users'  is null.");
        }

        #region Admin actions
        [HttpGet]
        public async Task<IActionResult> CreateAdmin()
        {
            AddUserViewModel addUserViewModel = new()
            {
                Id = Guid.Empty,
                Universities = await _dropDownListHelper.GetDDLUniversitiesAsync(),
                UserType = UserType.Admin
            };
            return View(addUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(AddUserViewModel addUserViewModel)
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
                return RedirectToAction("Index", "Users");
            }
            await FillDropDownListLocation(addUserViewModel);
            return View(addUserViewModel);
        }
        #endregion

        #region Librarian action
        [HttpGet]
        public async Task<IActionResult> CreateLibrarian()
        {
            AddUserViewModel addUserViewModel = new()
            {
                Id = Guid.Empty,
                Universities = await _dropDownListHelper.GetDDLUniversitiesAsync(),
                UserType = UserType.Librarian
            };
            return View(addUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLibrarian(AddUserViewModel addUserViewModel)
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
                return RedirectToAction("Index", "Home");
            }
            await FillDropDownListLocation(addUserViewModel);
            return View(addUserViewModel);
        }
        #endregion

        #region Private methods
        private async Task FillDropDownListLocation(AddUserViewModel addUserViewModel)
        {
            addUserViewModel.Universities = await _dropDownListHelper.GetDDLUniversitiesAsync();
        }
        #endregion
    }
}
