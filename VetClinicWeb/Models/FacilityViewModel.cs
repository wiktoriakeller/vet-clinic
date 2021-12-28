using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using VetClinic.DataAnnotationsValidations;

namespace VetClinicWeb.Models
{
    public class FacilityViewModel
    {
        public int FacilityId { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        [Remote(action: "IsAddressUnique", controller: "Facility", AdditionalFields = "FacilityId")]
        [ValidAddress(ErrorMessage = "Not a valid address.")]
        public string Address { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [StringLength(9, ErrorMessage = "{0} length must be greater than or equal to {1}.", MinimumLength = 9)]
        [RegularExpression("[0-9]{3}[0-9]{3}[0-9]{3}", ErrorMessage = "Provided value is not a phone number.")]
        public string PhoneNumber { get; set; }
    }
}
