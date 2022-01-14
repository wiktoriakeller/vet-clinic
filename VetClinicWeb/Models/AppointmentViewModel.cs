using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }

        [Display(Name = "Date and time")]
        public string AppointmentDate { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        [Remote(action: "IsTimeValid", controller: "Appointment")]
        public string Time { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z0-9.,?() ]*", ErrorMessage = "Cause can only contain letters, numbers or punctuation marks.")]
        [StringLength(150, ErrorMessage = "{0} length must be between {2} and {1}", MinimumLength = 0)]
        public string Cause { get; set; }

        [Required]
        [Remote(action: "IsEmployeeFree", controller: "Appointment", AdditionalFields = "Employee, Date, Time")]
        public int Employee { get; set; }
        
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        [Remote(action: "IsPatientFree", controller: "Appointment", AdditionalFields = "Patient, Date, Time")]
        public int Patient { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Required]
        public int Facility { get; set; }

        [Display(Name = "Facility")]
        public string FacilityAddress { get; set; }

        [Remote(action: "IsOfficeFree", controller: "Appointment", AdditionalFields = "Office, Date, Time, Facility")]
        public int Office { get; set; }

        [Display(Name = "Office Number")]
        public int OfficeNumber { get; set; }  
    }
}
