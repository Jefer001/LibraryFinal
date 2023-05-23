namespace Library.DAL.Entities
{
    public class BookGenre : Entity
    {
        #region Properties
        public Book Book { get; set; }

        public LiteraryGenre Literary { get; set; }
        #endregion
    }
}
