using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class EmployeeDataAccess : Access, IEmployeeDataAccess
    {
        public EmployeeDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<Employee>> GetEmployees()
        {
            string sql = "select * from employees";
            return _db.LoadData<Employee>(sql);
        }

        public async Task<Employee> Get(int EmployeeId)
        {
            string sql = "select * from employees where employeeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = EmployeeId
            });

            var results = await _db.LoadData<Employee, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(Employee Employee)
        {
            string sql = "insert into employees(name, surname, salary, bonusSalary, address, phoneNumber, position, facility) values(:Name, :Surname, :Salary, :BonusSalary, :Address, :PhoneNumber, :Position, :Facility)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Name = Employee.Name,
                Surname = Employee.Surname,
                Salary = Employee.Salary,
                BonusSalary = Employee.BonusSalary,
                Address = Employee.Address,
                PhoneNumber = Employee.PhoneNumber,
                Position = Employee.Position,
                Facility = Employee.Facility
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Employee Employee)
        {
            string sql = "update employees set name = :Name, surname = :Surname, salary = :Salary, bonusSalary = :BonusSalary, address = :Address, phoneNumber = :PhoneNumber, position = :Position, facility = :Facility where employeeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = Employee.EmployeeId,
                Name = Employee.Name,
                Surname = Employee.Surname,
                Salary = Employee.Salary,
                BonusSalary = Employee.BonusSalary,
                Address = Employee.Address,
                PhoneNumber = Employee.PhoneNumber,
                Position = Employee.Position,
                Facility = Employee.Facility
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int EmployeeId)
        {
            string sql = "delete from employees where employeeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = EmployeeId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
