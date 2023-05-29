using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AddUniversityViewModel : EditUniversityViewModel
    {
        [Display(Name = "Foto")]
        public IFormFile? ImageFile { get; set; }
    }
}
