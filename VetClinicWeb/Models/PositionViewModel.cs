using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class PositionViewModel
    {
        public int PositionId { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z]*", ErrorMessage = "Position name should only contain letters.")]
        public string Name { get; set; }
        
        [Required]
        [DisplayName("Minimum salary")]
        [DataType(DataType.Currency)]
        public float SalaryMin { get; set; }
        
        [Required]
        [DisplayName("Maximum salary")]
        [DataType(DataType.Currency)]
        public float SalaryMax { get; set; }
    }
}
