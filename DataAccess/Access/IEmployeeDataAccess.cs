using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IEmployeeDataAccess
    {
        Task Delete(int EmployeeId);
        Task<Employee> Get(int EmployeeId);
        Task<IEnumerable<Employee>> GetEmployees();
        Task Insert(Employee Employee);
        Task Update(Employee Employee);
    }
}