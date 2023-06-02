﻿using Library.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class EditTemporaryLoanViewModel : Entity
    {
        #region Properties
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0}")]
        [Display(Name = "Cantidad")]
        [Range(0.0000001, float.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la cantidad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Quantity { get; set; }
        #endregion
    }
}
