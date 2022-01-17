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
        [RegularExpression(@"^[a-zA-Z_]+( [a-zA-Z_]+)*$", ErrorMessage = "Organization name should only contain letters and spaces (no spaces at the end or at the beginning).")]
        [StringLength(100, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        public string Name { get; set; }

        [Required]
        [Remote(action: "IsNIPUnique", controller: "Organization", AdditionalFields = "OrganizationId")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "Organization NIP should only contain digits.")]
        [StringLength(10, ErrorMessage = "{0} length must be equal to {1} characters.", MinimumLength = 10)]
        public string NIP { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        [ValidAddress]
        public string Address { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [StringLength(9, ErrorMessage = "{0} length must be equal to {1}.", MinimumLength = 9)]
        [RegularExpression("[0-9]{3}[0-9]{3}[0-9]{3}", ErrorMessage = "Phone number should contain 9 digits.")]
        public string PhoneNumber { get; set; }
    }
}
