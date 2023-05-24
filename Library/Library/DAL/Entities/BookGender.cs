namespace Library.DAL.Entities
{
    public class BookGender : Entity
    {
        #region Properties
        public Book Book { get; set; }

        public LiteraryGender Literary { get; set; }
        #endregion
    }
}
