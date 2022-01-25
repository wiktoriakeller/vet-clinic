using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class EmployeeDataAccess : Access, IDataAccess<Employee>, IVeterinariansAccess, IUpdateEmployeeSalary
    {
        public EmployeeDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<Employee>> Get()
        {
            string sql = "select * from employees";
            return _db.LoadData<Employee>(sql);
        }

        public async Task<Employee> Get(int employeeId)
        {
            string sql = "select * from employees where employeeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = employeeId
            });

            var results = await _db.LoadData<Employee, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(Employee employee)
        {
            string sql = "insert into employees(name, surname, salary, bonusSalary, address, phoneNumber, position, facility) values(:Name, :Surname, :Salary, :BonusSalary, :Address, :PhoneNumber, :Position, :Facility)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Name = employee.Name,
                Surname = employee.Surname,
                Salary = employee.Salary,
                BonusSalary = employee.BonusSalary,
                Address = employee.Address,
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position,
                Facility = employee.Facility
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Employee employee)
        {
            string sql = "update employees set name = :Name, surname = :Surname, salary = :Salary, bonusSalary = :BonusSalary, address = :Address, phoneNumber = :PhoneNumber, position = :Position, facility = :Facility where employeeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = employee.EmployeeId,
                Name = employee.Name,
                Surname = employee.Surname,
                Salary = employee.Salary,
                BonusSalary = employee.BonusSalary,
                Address = employee.Address,
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position,
                Facility = employee.Facility
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task UpdateSalary(double salary, int employeeId)
        {
            string sql = "update employees set salary = :Salary where employeeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = employeeId,
                Salary = salary
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int employeeId)
        {
            string sql = "delete from employees where employeeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = employeeId
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task<IEnumerable<Employee>> GetVeterinarians()
        {
            string sql = @"select e.employeeId, e.name, e.surname, e.salary, e.bonusSalary, e.address, e.phoneNumber, e.position, e.facility
                        from employees e inner join positions p on e.position = p.positionId where p.NAME = 'Veterinarian'";
            return _db.LoadData<Employee>(sql);
        }
    }
}
