﻿using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class LiteraryGenre : Entity
    {
        #region Properties
        [Display(Name = "Género literario")]
        [MaxLength(100)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        public string? Description { get; set; }

        public ICollection<BookGenre> BookGenre { get; set; }
        #endregion
    }
}
