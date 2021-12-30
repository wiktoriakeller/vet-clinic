using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class SpeciesViewModel
    {
        public int SpeciesId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z]*", ErrorMessage = "Species name should only contain letters")]
        [StringLength(50, ErrorMessage = "{0} length must be shorter than {1} characters")]
        public string Name { get; set; }

    }
}
