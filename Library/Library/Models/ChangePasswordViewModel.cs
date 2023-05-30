﻿using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class ChangePasswordViewModel
    {
        #region Properties
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confirmación no son iguales.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmación nueva contraseña")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Confirm { get; set; }
        #endregion
    }
}
