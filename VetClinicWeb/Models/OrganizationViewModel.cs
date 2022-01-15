using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VetClinic.DataAnnotationsValidations;

namespace VetClinicWeb.Models
{
    public class OrganizationViewModel
    {
        public int OrganizationId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z ]*", ErrorMessage = "Organization name should only contain letters and spaces.")]
        [StringLength(100, ErrorMessage = "{0} length must be shorter than {1} characters")]
        public string Name { get; set; }

        [Required]
        [Remote(action: "IsNIPUnique", controller: "Organization", AdditionalFields = "OrganizationId")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "Organization NIP should only contain digits.")]
        [StringLength(10, ErrorMessage = "{0} length must be shorter than {1} characters")]
        public string NIP { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        [ValidAddress]
        public string Address { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [StringLength(9, ErrorMessage = "{0} length must be greater than or equal to {1}", MinimumLength = 9)]
        [RegularExpression("[0-9]{3}[0-9]{3}[0-9]{3}", ErrorMessage = "Phone number should contain 9 digits.")]
        public string PhoneNumber { get; set; }
    }
}
