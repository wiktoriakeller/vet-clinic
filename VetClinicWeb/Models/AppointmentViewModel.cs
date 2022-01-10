using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VetClinic.DataAnnotationsValidations;

namespace VetClinicWeb.Models
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }

        [Display(Name = "Date")]
        public string AppointmentDate { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        public string Cause { get; set; }

        [Required]
        public int Employee { get; set; }
        
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        public int Patient { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Required]
        public int Facility { get; set; }

        [Display(Name = "Facility")]
        public string FacilityAddress { get; set; }

        public int Office { get; set; }

        [Display(Name = "Office Number")]
        public int OfficeNumber { get; set; }  
    }
}
