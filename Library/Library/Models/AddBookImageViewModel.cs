using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AddBookImageViewModel
    {
        #region Properties
        public Guid BookId { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public IFormFile ImageFile { get; set; }
        #endregion
    }
}
