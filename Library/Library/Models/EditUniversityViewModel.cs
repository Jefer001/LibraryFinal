using Library.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class EditUniversityViewModel : Entity
    {
        #region Properties
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
        #endregion
    }
}
