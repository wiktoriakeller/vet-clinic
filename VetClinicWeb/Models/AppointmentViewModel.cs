using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }

        [DisplayName("Date and time")]
        public string AppointmentDate { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        [Remote(action: "IsTimeValid", controller: "Appointment")]
        public string Time { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z0-9.,?() ]*", ErrorMessage = "Cause can only contain letters, numbers and punctuation marks.")]
        [StringLength(150, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        public string Cause { get; set; }

        [Required]
        public int Facility { get; set; }

        [DisplayName("Facility")]
        public string FacilityAddress { get; set; }

        [Remote(action: "IsOfficeFree", controller: "Appointment", AdditionalFields = "Office, Date, Time, Facility, AppointmentId")]
        public int Office { get; set; }

        [DisplayName("Office Number")]
        public int OfficeNumber { get; set; }

        [Required]
        [Remote(action: "IsEmployeeFree", controller: "Appointment", AdditionalFields = "Employee, Date, Time, AppointmentId")]
        public int Employee { get; set; }
        
        [DisplayName("Employee")]
        public string EmployeeName { get; set; }

        [Required]
        [Remote(action: "IsPatientFree", controller: "Appointment", AdditionalFields = "Patient, Date, Time, AppointmentId")]
        public int Patient { get; set; }

        [DisplayName("Patient")]
        public string PatientName { get; set; }
    }
}
