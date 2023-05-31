using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class TemporaryLoan : Entity
    {
        #region Properties
        //public ICollection<OrderDetail> OrderDetails { get; set; }

        public User User { get; set; }

        public Book Book { get; set; }

        [DisplayFormat(DataFormatString = "{0}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }
        #endregion
    }
}
