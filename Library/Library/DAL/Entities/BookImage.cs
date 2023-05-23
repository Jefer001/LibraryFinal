using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class BookImage : Entity
    {
        #region Properties
        public Book Book { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId.Equals(Guid.Empty)
            ? $"https://localhost:7298/images/noimage.png"
            : $"https://sales2023.blob.core.windows.net/products/{ImageId}";
        #endregion
    }
}
