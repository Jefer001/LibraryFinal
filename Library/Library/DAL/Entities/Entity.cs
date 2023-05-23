using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Entities
{
    public class Entity
    {
        #region Properties
        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Fecha de creacíon.")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Fecha de entrega.")]
        public DateTime? Deadline { get; set; }
        #endregion
    }
}
