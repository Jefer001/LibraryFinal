﻿using Library.Enum;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AddUserViewModel : EditUserViewModel
    {
        #region Properties
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Username { get; set; }

        [DataType(DataType.Password)] //Me sirve para poner los punticos y evitar que el password se vea en la pantalla
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no son iguales.")] //Me sirve para comparar que el password que ingresé inicialmente sea el mismo que ingreso en esta propiedad
        [Display(Name = "Confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")] //New DataAnnotation
        public string PasswordConfirm { get; set; }

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }
        #endregion
    }
}
