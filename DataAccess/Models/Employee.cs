using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Employee
    {
		public int EmployeeId { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public double Salary { get; set; }
		public double BonusSalary { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Position { get; set; }
		public string Facility { get; set; }
	}
}
