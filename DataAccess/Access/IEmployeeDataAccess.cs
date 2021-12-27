using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IEmployeeDataAccess
    {
        Task DeleteEmployee(int EmployeeId);
        Task<Employee> GetEmployee(int EmployeeId);
        Task<IEnumerable<Employee>> GetEmployees();
        Task InsertEmployee(Employee Employee);
        Task UpdateEmployee(Employee Employee);
    }
}