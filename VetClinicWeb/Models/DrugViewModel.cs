using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class DrugViewModel
    {
        public int DrugId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "{0} length must be shorter than {1} characters")]
        [Remote(action: "IsDrugUnique", controller: "Drug", AdditionalFields = "manufacturer, drugId")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "{0} length must be shorter than {1} characters")]
        public string Manufacturer { get; set; }
    }
}
