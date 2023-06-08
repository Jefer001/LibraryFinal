﻿using Library.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class DetailsBookToCartViewModel : Entity
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

        [Display(Name = "Catálogos")]
        public string Catalogues { get; set; }

        public ICollection<BookImage> BookImages { get; set; }

        [DisplayFormat(DataFormatString = "{0}")]
        [Display(Name = "Cantidad")]
        [Range(0, int.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la cantidad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }
        #endregion
    }
}
