using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class SpeciesViewModel
    {
        public int SpeciesId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z_]+( [a-zA-Z_]+)*$", ErrorMessage = "Species name should only contain letters and spaces (no spaces at the end or at the beginning).")]
        [StringLength(50, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        public string Name { get; set; }

    }
}
