using Library.DAL;
using Library.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class CataloguesController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public CataloguesController(DataBaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Catalogue actions
        // GET: LiteraryGenres
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return _context.Catalogues != null ?
                        View(await _context.Catalogues.ToListAsync()) :
                        Problem("Entity set 'DataBaseContext.Catalogues'  is null.");
        }

        // GET: LiteraryGenres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LiteraryGenres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Catalogue catalogue)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    catalogue.CreatedDate = DateTime.Now;
                    _context.Add(catalogue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un catálogo con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(catalogue);
        }

        // GET: LiteraryGenres/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Catalogues == null) return NotFound();

            var catalogue = await _context.Catalogues.FindAsync(id);

            if (catalogue == null) return NotFound();

            return View(catalogue);
        }

        // POST: LiteraryGenres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Catalogue catalogue)
        {
            if (id != catalogue.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    catalogue.ModifiedDate = DateTime.Now;
                    _context.Update(catalogue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un catálogo con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(catalogue);
        }

        // GET: LiteraryGenres/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Catalogues == null) return NotFound();

            var catalogue = await _context.Catalogues.FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (catalogue == null) return NotFound();

            return View(catalogue);
        }

        // GET: LiteraryGenres/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Catalogues == null) return NotFound();

            var catalogue = await _context.Catalogues.FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (catalogue == null) return NotFound();

            return View(catalogue);
        }

        // POST: LiteraryGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Catalogues == null) return Problem("Entity set 'DataBaseContext.literaryGenres'  is null.");

            var catalogue = await _context.Catalogues.FindAsync(id);

            if (catalogue != null) _context.Catalogues.Remove(catalogue);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
