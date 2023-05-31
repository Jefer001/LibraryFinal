using Library.DAL.Entities;

namespace Library.Models
{
    public class HomeViewModel
    {
        #region Properties
        public ICollection<Book> Books { get; set; }

        public int Quantity { get; set; }
        #endregion
    }
}
