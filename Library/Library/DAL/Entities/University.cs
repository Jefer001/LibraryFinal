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

        [Display(Name = "Usuarios")]
        public ICollection<User> Users { get; set; }
        #endregion
    }
}
