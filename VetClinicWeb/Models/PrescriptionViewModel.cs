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
        [RegularExpression(@"^[a-zA-Z0-9_x]+( [a-zA-Z0-9_x]+)*$", ErrorMessage = "Dosage should only contain letters, numbers, spaces and letter x.")]
        [StringLength(100, ErrorMessage = "{0} length must shorter than {1}.")]
        public string Dosage { get; set; }

        [Required]
        [DisplayName("drug name")]
        public int DrugId { get; set; }
        
        public int AppointmentId { get; set; }

        [DisplayName("Drug name")]
        public string DrugName { get; set; }
    }
}
