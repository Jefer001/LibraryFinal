using Library.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class BookingsController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public BookingsController(DataBaseContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            return _context.LoanDetails != null ?
                        View(await _context.LoanDetails
                        .Include(ld => ld.Loan)
                        .ThenInclude(l => l.User)
                        .Include(ld => ld.Book)
                        .ToListAsync()) :
                        Problem("Entity set 'DataBaseContext.LoanDetails'  is null.");
        }
    }
}
