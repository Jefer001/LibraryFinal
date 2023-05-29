using Library.DAL;
using Library.DAL.Entities;
using Library.Helpers;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class UniversitiesController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IAzureBlobHelper _azureBlobHelper;
        #endregion

        #region Builder
        public UniversitiesController(DataBaseContext context, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            _azureBlobHelper = azureBlobHelper;
        }
        #endregion

        #region University actions
        // GET: Universities
        public async Task<IActionResult> Index()
        {
            return _context.Universities != null ?
                        View(await _context.Universities.ToListAsync()) :
                        Problem("Entity set 'DataBaseContext.Universities'  is null.");
        }

        // GET: Universities/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUniversityViewModel addUniversityViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;
                    if (addUniversityViewModel.ImageFile != null)
                        imageId = await _azureBlobHelper.UploadAzureBlobAsync(addUniversityViewModel.ImageFile, "products");

                    University university = new()
                    {
                        Name = addUniversityViewModel.Name,
                        ImageId = imageId,
                        CreatedDate = DateTime.Now
                    };

                    _context.Add(university);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe una universidad con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(addUniversityViewModel);
        }

        // GET: Universities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            University university = await _context.Universities.FindAsync(id);
            if (university == null) return NotFound();

            EditUniversityViewModel editUniversityViewModel = new()
            {
                Id = university.Id,
                Name = university.Name
            };

            return View(editUniversityViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditUniversityViewModel editUniversityViewModel)
        {
            if (id != editUniversityViewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    University university = await _context.Universities.FindAsync(editUniversityViewModel.Id);

                    university.Name = editUniversityViewModel.Name;
                    university.ModifiedDate = DateTime.Now;

                    _context.Update(university);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe una universidad con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(editUniversityViewModel);
        }

        // GET: Universities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Universities == null) return NotFound();

            var university = await _context.Universities.FirstOrDefaultAsync(u => u.Id.Equals(id));

            if (university == null) return NotFound();

            return View(university);
        }

        // GET: Universities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Universities == null) return NotFound();

            var university = await _context.Universities.FirstOrDefaultAsync(u => u.Id.Equals(id));

            if (university == null) return NotFound();

            return View(university);
        }

        // POST: Universities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Universities == null) return Problem("Entity set 'DataBaseContext.Universities'  is null.");

            var university = await _context.Universities.FindAsync(id);

            if (university != null) _context.Universities.Remove(university);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UniversityExists(Guid id)
        {
            return _context.Universities.Any(e => e.Id == id);
        }
        #endregion
    }
}
