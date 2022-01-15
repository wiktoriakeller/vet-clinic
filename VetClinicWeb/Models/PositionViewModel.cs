using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using VetClinic.DataAnnotationsValidations;

namespace VetClinicWeb.Models
{
    public class PositionViewModel
    {
        public int PositionId { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z]*", ErrorMessage = "Position name should only contain letters.")]
        [StringLength(50, ErrorMessage = "{0} length must be shorter than {1} characters.")]
        public string Name { get; set; }
        
        [Required]
        [DisplayName("Minimum salary")]
        [Range(typeof(double), "00.00", "99999.99")]
        [RegularExpression(@"[1-9]{0,1}[0-9]{0,4}[.]?(?=[0-9])[0-9]{0,2}", ErrorMessage = "Minimum salary should be a positive number with no more then two decimal places in the range of 0.00 - 99999.99")]
        [Remote(action: "IsSalaryValid", controller: "Position", AdditionalFields = "SalaryMax")]
        public double SalaryMin { get; set; }
        
        [Required]
        [DisplayName("Maximum salary")]
        [Range(typeof(double), "00.00", "99999.99")]
        [RegularExpression(@"[1-9]{0,1}[0-9]{0,4}[.]?(?=[0-9])[0-9]{0,2}", ErrorMessage = "Maximum salary should be a positive number with no more then two decimal places in the range of 0.00 - 99999.99")]
        [Remote(action: "IsSalaryValid", controller: "Position", AdditionalFields = "SalaryMin")]
        public double SalaryMax { get; set; }
    }
}
