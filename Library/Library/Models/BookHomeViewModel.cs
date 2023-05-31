using Library.DAL.Entities;

namespace Library.Models
{
    public class BookHomeViewModel
    {
        #region Properties
        public ICollection<Book> Books { get; set; }
        #endregion
    }
}
