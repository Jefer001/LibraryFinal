using Library.Common;
using Library.DAL;
using Library.DAL.Entities;
using Library.Enum;
using Library.Helpers;
using Library.Models;

namespace Library.Services
{
    public class LoanHelper : ILoanHelper
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public LoanHelper(DataBaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Public methods
        public async Task<Response> ProcessLoanAsync(ShowCartViewModel showCartViewModel)
        {
            Response response = await CheckInventoryAsync(showCartViewModel);
            if (!response.IsSuccess) return response;

            Loan loan = new()
            {
                CreatedDate = DateTime.Now,
                User = showCartViewModel.User,
                Remarks = showCartViewModel.Remarks,
                LoanDetails = new List<LoanDetail>(),
                LoanStatus = LoanStatus.Pendiente
            };

            foreach (TemporaryLoan? item in showCartViewModel.TemporaryLoans)
            {
                loan.LoanDetails.Add(new LoanDetail
                {
                    Book = item.Book,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                });

                Book book = await _context.Books.FindAsync(item.Book.Id);
                if (book != null)
                {
                    book.Stock -= item.Quantity;
                    _context.Books.Update(book);
                }

                _context.TemporaryLoans.Remove(item);
            }

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return response;
        }
        #endregion

        #region Private methods
        private async Task<Response> CheckInventoryAsync(ShowCartViewModel showCartViewModel)
        {
            Response response = new()
            {
                IsSuccess = true
            };

            foreach (TemporaryLoan item in showCartViewModel.TemporaryLoans)
            {
                Book book = await _context.Books.FindAsync(item.Book.Id);

                if (book == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"El libro {item.Book.Name}, ya no está disponible";
                    return response;
                }

                if (book.Stock < item.Quantity)
                {
                    response.IsSuccess = false;
                    response.Message = $"Lo sentimos, solo tenemos {item.Quantity} unidades del libro {item.Book.Name}, para tomar su pedido. Dismuya la cantidad.";
                    return response;
                }
            }
            return response;
        }
        #endregion
    }
}
