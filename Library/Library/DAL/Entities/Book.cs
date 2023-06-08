﻿using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class Book : Entity
    {
        #region Properties

        public ICollection<LoanDetail> LoanDetails { get; set; }

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

        public ICollection<BookCatalogue> BookCatalogues { get; set; }

        [Display(Name = "Cálogo")]
        public int NumberCatalogue => BookCatalogues == null ? 0 : BookCatalogues.Count;

        public ICollection<BookImage> BookImages { get; set; }

        [Display(Name = "Número Fotos")]
        public int ImagesNumber => BookImages == null ? 0 : BookImages.Count;

        [Display(Name = "Foto")]
        public string ImageFullPath => BookImages == null || BookImages.Count == 0
            ? $"https://localhost:7298/images/noimage.png"
            : BookImages.FirstOrDefault().ImageFullPath;
        #endregion
    }
}
