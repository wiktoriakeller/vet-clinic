using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IOfficeDataAccess
    {
        Task Delete(int OfficeId);
        Task<Office> Get(int OfficeId);
        Task<IEnumerable<Office>> GetOffices();
        Task Insert(Office Office);
        Task Update(Office Office);
    }
}