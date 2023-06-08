using Library.Common;
using Library.DAL;
using Library.DAL.Entities;
using Library.Helpers;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        #region Constants
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ILoanHelper _loanHelper;
        #endregion

        #region Builder
        public HomeController(ILogger<HomeController> logger, DataBaseContext context, IUserHelper userHelper, ILoanHelper loanHelper)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
            _loanHelper = loanHelper;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            List<Book>? books = await _context.Books
                .Include(b => b.BookImages)
                .Include(b => b.BookCatalogues)
                .OrderBy(b => b.Name)
                .ToListAsync();

            //Variables de Sesión
            ViewBag.UserFullName = GetUserFullName();

            HomeViewModel homeViewModel = new()
            {
                Books = books
            };

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                homeViewModel.Quantity = await _context.TemporaryLoans
                    .Where(tl => tl.User.Id.Equals(user.Id))
                    .SumAsync(tl => tl.Quantity);
            }

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        #region Book actions
        public async Task<IActionResult> AddBookInCart(Guid? bookId)
        {
            if (bookId == null) return NotFound();

            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            Book book = await _context.Books.FindAsync(bookId);
            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            if (user == null || book == null) return NotFound();

            // Busca una entrada existente en la tabla TemporalSale para este producto y usuario
            TemporaryLoan existingTemporaryLoan = await _context.TemporaryLoans
                .Where(t => t.Book.Id.Equals(bookId) && t.User.Id.Equals(user.Id))
                .FirstOrDefaultAsync();

            if (existingTemporaryLoan != null)
            {
                // Si existe una entrada, incrementa la cantidad
                existingTemporaryLoan.Quantity += 1;
                existingTemporaryLoan.ModifiedDate = DateTime.Now;
            }
            else
            {
                // Si no existe una entrada, crea una nueva
                TemporaryLoan temporaryLoan = new()
                {
                    CreatedDate = DateTime.Now,
                    Book = book,
                    Quantity = 1,
                    User = user
                };

                _context.TemporaryLoans.Add(temporaryLoan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsBook(Guid? bookId)
        {
            Book book = await _context.Books
                .Include(b => b.BookImages)
                .Include(b => b.BookCatalogues)
                .ThenInclude(bc => bc.Catalogue)
                .FirstOrDefaultAsync(b => b.Id.Equals(bookId));

            if (book == null || bookId == null) return NotFound();

            string catalogues = string.Empty;

            foreach (BookCatalogue? catalogue in book.BookCatalogues)
                catalogues += $"{catalogue.Catalogue.Name}, ";

            catalogues = catalogues.Substring(0, catalogues.Length - 2);

            DetailsBookToCartViewModel detailsBookToCartViewModel = new()
            {
                Catalogues = catalogues,
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                BookImages = book.BookImages,
                Quantity = 1,
                Stock = book.Stock
            };

            return View(detailsBookToCartViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsBook(DetailsBookToCartViewModel detailsBookToCartViewModel)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            Book book = await _context.Books.FindAsync(detailsBookToCartViewModel.Id);
            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            if (book == null || user == null) return NotFound();

            // Busca una entrada existente en la tabla TemporalSale para este producto y usuario
            TemporaryLoan existingTemporaryLoan = await _context.TemporaryLoans
                .Where(t => t.Book.Id.Equals(detailsBookToCartViewModel.Id) && t.User.Id.Equals(user.Id))
                .FirstOrDefaultAsync();

            if (existingTemporaryLoan != null)
            {
                // Si existe una entrada, incrementa la cantidad
                existingTemporaryLoan.Quantity += detailsBookToCartViewModel.Quantity;
                existingTemporaryLoan.ModifiedDate = DateTime.Now;
            }
            else
            {
                // Si no existe una entrada, crea una nueva
                TemporaryLoan temporaryLoan = new()
                {
                    Book = book,
                    Quantity = 1,
                    User = user,
                    Remarks = detailsBookToCartViewModel.Remarks,
                };

                _context.TemporaryLoans.Add(temporaryLoan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region 
        [Authorize] //Etiqueta para que solo usuarios logueados puedan acceder a este método.
        public async Task<IActionResult> ShowCartAndConfirm()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null) return NotFound();

            List<TemporaryLoan>? temporaryLoans = await _context.TemporaryLoans
                .Include(tl => tl.Book)
                .ThenInclude(b => b.BookImages)
                .Where(tl => tl.User.Id.Equals(user.Id))
                .ToListAsync();

            ShowCartViewModel showCartViewModel = new()
            {
                User = user,
                TemporaryLoans = temporaryLoans
            };

            return View(showCartViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowCartAndConfirm(ShowCartViewModel showCartViewModel)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null) return NotFound();

            showCartViewModel.User = user;
            showCartViewModel.TemporaryLoans = await _context.TemporaryLoans
                .Include(tl => tl.Book)
                .ThenInclude(b => b.BookImages)
                .Where(tl => tl.User.Id.Equals(user.Id))
            .ToListAsync();

            Response response = await _loanHelper.ProcessLoanAsync(showCartViewModel);
            if (response.IsSuccess) return RedirectToAction(nameof(LoanSuccess));

            ModelState.AddModelError(string.Empty, response.Message);
            return View(showCartViewModel);
        }
        #endregion

        [Authorize]
        public IActionResult LoanSuccess()
        {
            return View();
        }

        #region
        public async Task<IActionResult> DecreaseQuantity(Guid? temporaryLoanId)
        {
            if (temporaryLoanId == null) return NotFound();

            TemporaryLoan temporaryLoan = await _context.TemporaryLoans.FindAsync(temporaryLoanId);
            if (temporaryLoan == null) return NotFound();

            if (temporaryLoan.Quantity > 1)
            {
                temporaryLoan.Quantity--;
                temporaryLoan.ModifiedDate = DateTime.Now;
                _context.TemporaryLoans.Update(temporaryLoan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ShowCartAndConfirm));
        }

        public async Task<IActionResult> IncreaseQuantity(Guid? temporaryLoanId)
        {
            if (temporaryLoanId == null) return NotFound();

            TemporaryLoan temporaryLoan = await _context.TemporaryLoans.FindAsync(temporaryLoanId);
            if (temporaryLoan == null) return NotFound();

            temporaryLoan.Quantity++;
            temporaryLoan.ModifiedDate = DateTime.Now;
            _context.TemporaryLoans.Update(temporaryLoan);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(ShowCartAndConfirm));
        }

        public async Task<IActionResult> DeleteTemporaryLoan(Guid? temporaryLoanId)
        {
            if (temporaryLoanId == null) return NotFound();

            TemporaryLoan temporaryLoan = await _context.TemporaryLoans.FindAsync(temporaryLoanId);
            if (temporaryLoan == null) return NotFound();

            _context.TemporaryLoans.Remove(temporaryLoan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCartAndConfirm));
        }
        #endregion

        #region Edit temporary loan actions
        public async Task<IActionResult> EditTemporaryLoan(Guid? temporaryLoanId)
        {
            if (temporaryLoanId == null) return NotFound();

            TemporaryLoan temporaryLoan = await _context.TemporaryLoans.FindAsync(temporaryLoanId);
            if (temporaryLoan == null) return NotFound();

            EditTemporaryLoanViewModel editTemporaryLoanViewModel = new()
            {
                Id = temporaryLoan.Id,
                Quantity = temporaryLoan.Quantity,
                Remarks = temporaryLoan.Remarks,
                ModifiedDate = DateTime.Now,
            };

            return View(editTemporaryLoanViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTemporaryLoan(Guid? temporaryLoanId, EditTemporaryLoanViewModel editTemporaryLoanViewModel)
        {
            if (temporaryLoanId != editTemporaryLoanViewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    TemporaryLoan temporaryLoan = await _context.TemporaryLoans.FindAsync(temporaryLoanId);
                    temporaryLoan.Quantity = editTemporaryLoanViewModel.Quantity;
                    temporaryLoan.Remarks = editTemporaryLoanViewModel.Remarks;
                    temporaryLoan.ModifiedDate = DateTime.Now;
                    _context.Update(temporaryLoan);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(editTemporaryLoanViewModel);
                }

                return RedirectToAction(nameof(ShowCartAndConfirm));
            }

            return View(editTemporaryLoanViewModel);
        }
        #endregion

        #region Private methods
        private string GetUserFullName()
        {
            return _context.Users
                .Where(u => u.Email.Equals(User.Identity.Name))
                .Select(u => u.FullName.ToUpper())
                .FirstOrDefault();
        }
        #endregion
    }
}