using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VetClinic.DataAnnotationsValidations;

namespace VetClinicWeb.Models
{
    public class PrescriptionViewModel
    {
        public int PrescriptionId { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_]+( [a-zA-Z_]+)*$", ErrorMessage = "Dosage should only contain letters, numbers and spaces.")]
        [StringLength(100, ErrorMessage = "{0} length must shorter than {1}.")]
        public string Dosage { get; set; }

        [Required]
        public int DrugId { get; set; }
        
        public int AppointmentId { get; set; }

        [DisplayName("Drug name")]
        public string DrugName { get; set; }
    }
}
