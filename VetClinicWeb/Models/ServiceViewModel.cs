using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicWeb.Models
{
    public class ServiceViewModel
    {
        public int ServiceId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z_]+( [a-zA-Z_]+)*$", ErrorMessage = "Service name should only contain letters and spaces (no spaces at the end or at the beginning).")]
        [StringLength(100, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        public string Name { get; set; }

        [Required]
        [Range(typeof(double), "00.00", "99999.99")]
        [RegularExpression(@"[1-9]{0,1}[0-9]{0,4}[.]?(?=[0-9])[0-9]{0,2}", ErrorMessage = "Price should be a positive number with no more then two decimal places in the range of 0.00 - 99999.99")]
        public double Price { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-z0-9.,?() ]*", ErrorMessage = "Description can only contain letters, numbers and punctuation marks.")]
        [StringLength(250, ErrorMessage = "{0} length must be shorter than or equal to {1} characters.")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Service type")]
        public char ServiceType { get; set; }

        [DisplayName("Service type")]
        public string FullServiceType { get; set; }
    }
}
