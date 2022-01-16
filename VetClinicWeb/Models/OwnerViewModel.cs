using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VetClinic.DataAnnotationsValidations;

namespace VetClinicWeb.Models
{
    public class OwnerViewModel
    {
        public int OwnerId { get; set; }

        [Required]
        [Remote(action: "IsPESELUnique", controller: "Owner", AdditionalFields = "OwnerId")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "Owner PESEL should only contain digits.")]
        [StringLength(11, ErrorMessage = "{0} length must be equal to {1} characters.")]
        public string PESEL { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z]*", ErrorMessage = "Owner name should only contain letters.")]
        [StringLength(50, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z]*", ErrorMessage = "Owner surname should only contain letters.")]
        [StringLength(50, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        public string Surname { get; set; }

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
