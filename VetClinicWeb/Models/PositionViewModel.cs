using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class PositionViewModel
    {

        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Minimum salary")]
        public float SalaryMin { get; set; }
        [Required]
        [DisplayName("Maximum salary")]
        public float SalaryMax { get; set; }
    }
}
