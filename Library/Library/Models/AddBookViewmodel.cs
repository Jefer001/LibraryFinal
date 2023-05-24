using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AddBookViewmodel : EdtitBookViewModel
    {
        #region Properties
        [Display(Name = "Catálogo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid CatalogueId { get; set; }

        public IEnumerable<SelectListItem> Catalogues { get; set; }

        [Display(Name = "Foto")]
        public IFormFile? ImageFile { get; set; }
        #endregion
    }
}
