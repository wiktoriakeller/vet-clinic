using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VetClinic.DataAnnotationsValidations;

namespace VetClinicWeb.Models
{
    public class PatientViewModel
    {
        public int PatientId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z]*", ErrorMessage = "Patient name should only contain letters.")]
        [StringLength(50, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        public string Name { get; set; }
        
        public int Species { get; set; }

        [OrganizationOrOwner]
        public int? Organization { get; set; }

        [OrganizationOrOwner]
        public int? Owner { get; set; }

        [DisplayName("Species")]
        public string SpeciesName { get; set; }

        [DisplayName("Organization")]
        public string OrganizationName { get; set; }

        [DisplayName("Owner")]
        public string OwnerName { get; set; }
    }
}
