using Library.DAL;
using Library.DAL.Entities;
using Library.Helpers;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
                            new BookImage 
                            {
                                CreatedDate = DateTime.Now,
                                ImageId = imageId
                            }
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
        public async Task<IActionResult> Details(Guid? bookId)
        {
            if (bookId == null) return NotFound();

            Book book = await _context.Books
                .Include(b => b.BookImages)
                .Include(b => b.BookCatalogues)
                .ThenInclude(bc => bc.Catalogue)
                .FirstOrDefaultAsync(p => p.Id.Equals(bookId));

            if (book == null) return NotFound();

            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(Guid? bookId)
        {
            if (bookId == null) return NotFound();

            Book book = await _context.Books
                .Include(b => b.BookCatalogues)
                .Include(b => b.BookImages)
                .FirstOrDefaultAsync(b => b.Id.Equals(bookId));

            if (book == null) return NotFound();

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Book bookModel)
        {
            Book book = await _context.Books
                .Include(b => b.BookImages)
                .Include(b => b.BookCatalogues)
                .FirstOrDefaultAsync(b => b.Id.Equals(bookModel.Id));

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            foreach (BookImage bookImage in book.BookImages)
                await _azureBlobHelper.DeleteAzureBlobAsync(bookImage.ImageId, "products");

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Image actions
        [HttpGet]
        public async Task<IActionResult> AddImage(Guid? bookId)
        {
            if (bookId == null) return NotFound();

            Book book = await _context.Books.FindAsync(bookId);
            if (book == null) return NotFound();

            AddBookImageViewModel addBookImageView = new()
            {
                BookId = book.Id
            };
            return View(addBookImageView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddBookImageViewModel addBookImageViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync(addBookImageViewModel.ImageFile, "products");

                    Book book = await _context.Books.FindAsync(addBookImageViewModel.BookId);

                    BookImage bookImage = new()
                    {
                        Book = book,
                        ImageId = imageId,
                        CreatedDate = DateTime.Now
                    };

                    _context.Add(bookImage);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { bookId = book.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(addBookImageViewModel);
        }

        public async Task<IActionResult> DeleteImage(Guid? imageId)
        {
            if (imageId == null) return NotFound();

            BookImage bookImage = await _context.BookImages
                .Include(bi => bi.Book)
                .FirstOrDefaultAsync(pi => pi.Id.Equals(imageId));

            if (bookImage == null) return NotFound();

            await _azureBlobHelper.DeleteAzureBlobAsync(bookImage.ImageId, "products");

            _context.BookImages.Remove(bookImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { bookId = bookImage.Book.Id });
        }
        #endregion

        #region Catalogue actions
        [HttpGet]
        public async Task<IActionResult> AddCatalogue(Guid? bookId)
        {
            if (bookId == null) return NotFound();

            Book book = await _context.Books
                .Include(b => b.BookCatalogues)
                .ThenInclude(bc => bc.Catalogue)
                .FirstOrDefaultAsync(p => p.Id.Equals(bookId));

            if (book == null) return NotFound();

            List<Catalogue> catalogues = book.BookCatalogues.Select(bc => new Catalogue
            {
                Id = bc.Catalogue.Id,
                Name = bc.Catalogue.Name
            }).ToList();

            AddBookCatalogueViewModel bookCatalogueViewModel = new()
            {
                BookId = book.Id,
                Catalogues = await _dropDownListHelper.GetDDLCataloguesAsync(catalogues)
            };
            return View(bookCatalogueViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCatalogue(AddBookCatalogueViewModel addBookCatalogueView)
        {
            Book book = await _context.Books
                .Include(b => b.BookCatalogues)
                .ThenInclude(bc => bc.Catalogue)
                .FirstOrDefaultAsync(b => b.Id.Equals(addBookCatalogueView.BookId));

            if (ModelState.IsValid)
            {
                try
                {
                    Catalogue catalogue = await _context.Catalogues.FindAsync(addBookCatalogueView.CatalogueId);

                    if (book == null || catalogue == null) return NotFound();

                    BookCatalogue bookCatalogue = new()
                    {
                        Book = book,
                        Catalogue = catalogue
                    };

                    _context.Add(bookCatalogue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { bookId = book.Id });
                }
                catch (Exception ex)
                {
                    addBookCatalogueView.Catalogues = await _dropDownListHelper.GetDDLCataloguesAsync();
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            List<Catalogue> catalogues = book.BookCatalogues.Select(bc => new Catalogue
            {
                Id = bc.Catalogue.Id,
                Name = bc.Catalogue.Name
            }).ToList();

            addBookCatalogueView.Catalogues = await _dropDownListHelper.GetDDLCataloguesAsync(catalogues);
            return View(addBookCatalogueView);
        }

        public async Task<IActionResult> DeleteCatalogue(Guid? bookId)
        {
            if (bookId == null) return NotFound();

            BookCatalogue bookCatalogue = await _context.BookCatalogues
                .Include(bc => bc.Book)
                .FirstOrDefaultAsync(bc => bc.Id.Equals(bookId));

            if (bookCatalogue == null) return NotFound();

            _context.BookCatalogues.Remove(bookCatalogue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { bookId = bookCatalogue.Book.Id });
        }
        #endregion
    }
}
