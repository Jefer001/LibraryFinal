using Library.Enum;
using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class Loan : Entity
    {
        #region Properties
        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        [Display(Name = "Estado Préstamo")]
        public LoanStatus LoanStatus { get; set; }

        public ICollection<LoanDetail> LoanDetails { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Líneas")]
        public int Lines => LoanDetails == null ? 0 : LoanDetails.Count;

        [DisplayFormat(DataFormatString = "{0}")]
        [Display(Name = "Cantidad")]
        public int Quantity => LoanDetails == null ? 0 : LoanDetails.Sum(sd => sd.Quantity);
        #endregion
    }
}
