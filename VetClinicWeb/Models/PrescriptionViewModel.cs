using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class PrescriptionViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must shorter than {1}.")]
        public string Dosage { get; set; }

        public int DrugId { get; set; }
        public int AppointmentId { get; set; }

        public string DrugName { get; set; }
    }
}