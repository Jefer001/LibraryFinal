using Library.DAL;
using Library.DAL.Entities;
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

        #region Booking action
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

        public async Task<IActionResult> Loan()
        {
            return _context.LoanDetails != null ?
                        View(await _context.LoanDetails
                        .Include(ld => ld.Loan)
                        .ThenInclude(l => l.User)
                        .Include(ld => ld.Book)
                        .ToListAsync()) :
                        Problem("Entity set 'DataBaseContext.Loan'  is null.");
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            LoanDetail loanDetail = await _context.LoanDetails
                .Include(ld => ld.Book)
                .FirstOrDefaultAsync(ld => ld.Id.Equals(id));

            if (loanDetail == null || id == null) return NotFound();

            loanDetail.Book.Stock += loanDetail.Quantity;

            _context.LoanDetails.Remove(loanDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Confirm(Guid? id)
        {
            LoanDetail loanDetail = await _context.LoanDetails
                .Include(ld => ld.Book)
                .Include(ld => ld.Loan)
                .FirstOrDefaultAsync(ld => ld.Id.Equals(id));

            if (loanDetail == null || id == null) return NotFound();

            loanDetail.Loan.LoanStatus = Enum.LoanStatus.Confirmado;
            loanDetail.CreatedDate = DateTime.Now;
            loanDetail.Deadline = DateTime.Now.AddDays(5);

            _context.LoanDetails.Update(loanDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Return(Guid? id)
        {
            LoanDetail loanDetail = await _context.LoanDetails
                .Include(ld => ld.Book)
                .FirstOrDefaultAsync(ld => ld.Id.Equals(id));

            if (loanDetail == null || id == null) return NotFound();

            loanDetail.Book.Stock += loanDetail.Quantity;

            _context.LoanDetails.Remove(loanDetail);
            await _context.SaveChangesAsync();

            return View();
        }
        #endregion
    }
}
