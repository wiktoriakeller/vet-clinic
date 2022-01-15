using DataAccess.Models;
using System.ComponentModel;

namespace VetClinicWeb.Models
{
    public class OfficeViewModel
    {
        public int OfficeId { get; set; }
        [DisplayName("Office number")]
        public int OfficeNumber { get; set; }
        public int Facility { get; set; }
        public string Address{ get; set; }
    }
}
