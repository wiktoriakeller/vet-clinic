using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class DrugViewModel
    {
        public int DrugId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        [Remote(action: "IsDrugUnique", controller: "Drug", AdditionalFields = "Manufacturer, DrugId")]
        [RegularExpression(@"^[a-zA-Z_0-9]+( [a-zA-Z_0-9]+)*$", ErrorMessage = "Drug name should only contain letters, numbers and spaces (no spaces at the end or at the beginning).")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        [Remote(action: "IsDrugUnique", controller: "Drug", AdditionalFields = "Name, DrugId")]
        [RegularExpression(@"^[a-zA-Z_]+( [a-zA-Z_]+)*$", ErrorMessage = "Drug manufacturer name should only contain letters and spaces (no spaces at the end or at the beginning).")]
        public string Manufacturer { get; set; }
    }
}
