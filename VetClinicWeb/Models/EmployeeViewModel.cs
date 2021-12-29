using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VetClinicWeb.Models
{
    public class EmployeeViewModel
    {
		[DisplayName("Employee ID")]
		public int EmployeeId { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "{0} length must be shorter than {1} characters")]
		public string Name { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "{0} length must be shorter than {1} characters")]
		public string Surname { get; set; }
		
		[Required]
		[DataType(DataType.Currency)]
		[Remote(action: "isSalaryInRange", controller: "Employee", AdditionalFields = "Position")]
		public double Salary { get; set; }

		[Required]
		[DataType(DataType.Currency)]
		[DisplayName("Bonus salary")]
		public double BonusSalary { get; set; }

		[Required]
		[StringLength(150, ErrorMessage = "{0} length must be shorter than {1} characters")]
		public string Address { get; set; }

		[Required]
		[DisplayName("Phone Number")]
		[StringLength(9, ErrorMessage = "{0} length must be greater than or equal to {1}", MinimumLength = 9)]
		[RegularExpression("[0-9]{3}[0-9]{3}[0-9]{3}", ErrorMessage = "Phone number should contain 9 digits")]
		public string PhoneNumber { get; set; }

		public int Position { get; set; }
		public int Facility { get; set; }

		[DisplayName("Position")]
		public string PositionName { get; set; }
		[DisplayName("Facility")]
		public string FacilityAddress { get; set; }
    }
}
