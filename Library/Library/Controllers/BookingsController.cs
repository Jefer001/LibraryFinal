﻿using Library.DAL;
using Library.DAL.Entities;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
    }
}
