using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Library.Models
{
    public class AddBookCatalogueViewModel
    {
        #region Properties
        public Guid BookId { get; set; }

        [Display(Name = "Catálogo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid CatalogueId { get; set; }

        public IEnumerable<SelectListItem> Catalogues { get; set; }
        #endregion
    }
}
