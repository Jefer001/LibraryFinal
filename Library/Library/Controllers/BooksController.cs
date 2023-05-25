using Library.DAL;
using Library.DAL.Entities;
using Library.Helpers;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IAzureBlobHelper _azureBlobHelper;
        private readonly IDropDownListHelper _dropDownListHelper;
        #endregion

        #region Builder
        public BooksController(DataBaseContext context, IAzureBlobHelper azureBlobHelper, IDropDownListHelper dropDownListHelper)
        {
            _context = context;
            _azureBlobHelper = azureBlobHelper;
            _dropDownListHelper = dropDownListHelper;
        }
        #endregion

        #region Book actions
        // GET: Books
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books
                .Include(b => b.BookImages)
                .Include(b => b.BookCatalogues)
                .ThenInclude(bc => bc.Catalogue)
                .ToListAsync());
                        
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            AddBookViewmodel addBookViewmodel = new()
            {
                Catalogues = await _dropDownListHelper.GetDDLCataloguesAsync(),
            };

            return View(addBookViewmodel);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddBookViewmodel addBookViewmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;
                    if (addBookViewmodel.ImageFile != null)
                        imageId = await _azureBlobHelper.UploadAzureBlobAsync(addBookViewmodel.ImageFile, "products");

                    Book book = new()
                    {
                        CreatedDate = DateTime.Now,
                        Name = addBookViewmodel.Name,
                        Author = addBookViewmodel.Author,
                        Stock = addBookViewmodel.Stock,
                        BookCatalogues = new List<BookCatalogue>()
                    {
                        new BookCatalogue
                        {
                            Catalogue = await _context.Catalogues.FindAsync(addBookViewmodel.CatalogueId)
                        }
                    }
                    };

                    if (imageId != Guid.Empty)
                    {
                        book.BookImages = new List<BookImage>()
                        {
                            new BookImage { ImageId = imageId }
                        };
                    }

                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un libro con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            addBookViewmodel.Catalogues = await _dropDownListHelper.GetDDLCataloguesAsync();
            return View(addBookViewmodel);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(Guid? bookId)
        {
            if (bookId == null) return NotFound();

            Book book = await _context.Books.FindAsync(bookId);
            if (book == null) return NotFound();

            EdtitBookViewModel edtitBookViewModel = new()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Stock = book.Stock
            };
            return View(edtitBookViewModel);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? Id, EdtitBookViewModel edtitBookViewModel)
        {
            if (Id != edtitBookViewModel.Id) return NotFound();

            try
            {
                Book book = await _context.Books.FindAsync(edtitBookViewModel.Id);

                book.Name = edtitBookViewModel.Name;
                book.Author = edtitBookViewModel.Author;
                book.Stock = edtitBookViewModel.Stock;
                book.ModifiedDate = DateTime.Now;

                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    ModelState.AddModelError(string.Empty, "Ya existe un libro con el mismo nombre.");
                else
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(edtitBookViewModel);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Books == null) return NotFound();

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id.Equals(id));

            if (book == null) return NotFound();

            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Books == null) return NotFound();

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id.Equals(id));

            if (book == null) return NotFound();

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Books == null) return Problem("Entity set 'DataBaseContext.Books'  is null.");

            var book = await _context.Books.FindAsync(id);

            if (book != null) _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(Guid id)
        {
            return (_context.Books?.Any(b => b.Id.Equals(id))).GetValueOrDefault();
        }
        #endregion
    }
}
