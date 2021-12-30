using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class SpeciesViewModel
    {
        public int SpeciesId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1} characters", MinimumLength = 1)]
        public string Name { get; set; }

    }
}
