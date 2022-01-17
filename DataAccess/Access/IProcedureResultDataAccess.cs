using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IProcedureResultDataAccess
    {
        Task<double> GetIncome(int facilityId, int year);
    }
}
