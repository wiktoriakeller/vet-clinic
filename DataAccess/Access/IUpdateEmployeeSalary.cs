using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IUpdateEmployeeSalary
    {
        public Task UpdateSalary(double salary, int id);
    }
}
