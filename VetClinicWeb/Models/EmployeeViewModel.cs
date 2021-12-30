using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VetClinic.DataAnnotationsValidations;

namespace VetClinicWeb.Models
{
    public class EmployeeViewModel
    {
		[DisplayName("Employee ID")]
		public int EmployeeId { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[RegularExpression(@"[A-Za-z]*", ErrorMessage = "Employee name should only contain letters")]
		[StringLength(50, ErrorMessage = "{0} length must be shorter than {1} characters")]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[RegularExpression(@"[A-Za-z]*", ErrorMessage = "Employee surname should only contain letters")]
		[StringLength(50, ErrorMessage = "{0} length must be shorter than {1} characters")]
		public string Surname { get; set; }
		
		[Required]
		[RegularExpression(@"[1-9]{0,1}[0-9]{0,4}[.]?(?=[0-9])[0-9]{0,2}", ErrorMessage = "Salary should be a positive number with no more then two decimal places in the range of 0.00 - 99999.99")]
		[Remote(action: "IsSalaryInRange", controller: "Employee", AdditionalFields = "Position")]
		public double Salary { get; set; }

		[Required]
		[RegularExpression(@"[1-9]{0,1}[0-9]{0,3}[.]?(?=[0-9])[0-9]{0,2}", ErrorMessage = "Bonus salary should be a positive number with no more then two decimal places in the range of 0.00 - 9999.99")]
		[DisplayName("Bonus salary")]
		public double BonusSalary { get; set; }

		[Required]
		[StringLength(150, ErrorMessage = "{0} length must be between {2} and {1}", MinimumLength = 4)]
		[ValidAddress] 
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
