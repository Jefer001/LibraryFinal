using Library.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Library.Models
{
    public class ShowCartViewModel
    {
        #region Properties
        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        public ICollection<TemporaryLoan> TemporaryLoans { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Quantity => TemporaryLoans == null ? 0 : TemporaryLoans.Sum(ts => ts.Quantity);
        #endregion
    }
}
