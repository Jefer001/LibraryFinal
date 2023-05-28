using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class University : Entity
    {
        #region Properties
        [Display(Name = "Universidad")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to out the corret paths
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId.Equals(Guid.Empty)
            ? $"https://localhost:7298//images/noimage.png"
            : $"http://sales2023.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Usuarios")]
        public ICollection<User> Users { get; set; }
        #endregion
    }
}
