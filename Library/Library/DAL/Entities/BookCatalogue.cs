namespace Library.DAL.Entities
{
    public class BookCatalogue : Entity
    {
        #region Properties
        public Book Book { get; set; }

        public Catalogue Catalogue { get; set; }
        #endregion
    }
}
