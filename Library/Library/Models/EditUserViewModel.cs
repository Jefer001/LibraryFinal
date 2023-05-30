﻿using Library.DAL.Entities;
using Library.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class EditUserViewModel : Entity
    {
        #region Properties
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId.Equals(Guid.Empty)
            ? $"https://localhost:7298/images/noimage.png"
            : $"https://sales2023.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Universidad")]
        [NonEmptyGuid(ErrorMessage = "Debes de seleccionar una universidad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid UniversityId { get; set; }

        public IEnumerable<SelectListItem> Universities { get; set; }
        #endregion
    }
}