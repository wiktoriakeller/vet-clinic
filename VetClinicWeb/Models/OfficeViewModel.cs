using DataAccess.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class OfficeViewModel
    {
        public int OfficeId { get; set; }

        [Required]
        [DisplayName("Office number")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[1-9][0-9]{0,4}", ErrorMessage = "Office number can only contain numbers and should be in range 1-99999.")]
        public int OfficeNumber { get; set; }
        
        public int Facility { get; set; }
        
        [DisplayName("Facility")]
        public string FacilityAddress { get; set; }
    }
}
