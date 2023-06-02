using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class LoanDetail : Entity
    {
        #region Properties
        public Loan Loan { get; set; }

        public TemporaryLoan? TemporaryLoan { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        public Book Book { get; set; }

        [DisplayFormat(DataFormatString = "{0}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Quantity { get; set; }
        #endregion
    }
}
