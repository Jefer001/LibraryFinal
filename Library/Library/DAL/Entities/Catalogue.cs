using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class Catalogue : Entity
    {
        #region Properties
        [Display(Name = "Catálogo")]
        [MaxLength(100)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        public string? Description { get; set; }

        public ICollection<BookCatalogue> BookCatalogues { get; set; }
        #endregion
    }
}
