using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class ServicesInAppointmentViewModel
    {
        public int ServicesInAppointmentId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_,.]+( [a-zA-Z0-9_,.]+)*$", ErrorMessage = "Description should only contain letters, numbers, punctuation marks and spaces.")]
        [StringLength(150, ErrorMessage = "{0} length must shorter than {1}.")]
        public string Diagnosis { get; set; }

        [Required]
        [DisplayName("Appointment")]
        public int AppointmentId { get; set; }

        [Required]
        public int Service { get; set; }

        [DisplayName("Service")]
        public string ServiceName { get; set; }
    }
}
