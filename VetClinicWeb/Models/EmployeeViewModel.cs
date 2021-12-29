using DataAccess.Models;
using System.ComponentModel;

namespace VetClinicWeb.Models
{
    public class EmployeeViewModel
    {
		[DisplayName("Employee ID")]
		public int EmployeeId { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public double Salary { get; set; }
		[DisplayName("Bonus salary")]
		public double BonusSalary { get; set; }
		public string Address { get; set; }
		[DisplayName("Phone number")]
		public string PhoneNumber { get; set; }
		public int Position { get; set; }
		public int Facility { get; set; }

		[DisplayName("Position")]
		public string PositionName { get; set; }
		[DisplayName("Facility")]
		public string FacilityAddress { get; set; }
    }
}
