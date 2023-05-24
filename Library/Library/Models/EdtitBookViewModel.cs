using Library.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class EdtitBookViewModel : Entity
    {
        #region Properties
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Autor")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Author { get; set; }

        [DisplayFormat(DataFormatString = "{0}")]
        [Display(Name = "Inventario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Stock { get; set; }
        #endregion
    }
}
