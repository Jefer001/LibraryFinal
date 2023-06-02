using Library.Common;
using Library.Models;

namespace Library.Helpers
{
    public interface ILoanHelper
    {
        Task<Response> ProcessLoanAsync(ShowCartViewModel showCartViewModel);
    }
}
