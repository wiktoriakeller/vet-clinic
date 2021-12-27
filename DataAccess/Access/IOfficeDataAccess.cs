using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IOfficeDataAccess
    {
        Task DeleteOffice(int OfficeId);
        Task<Office> GetOffice(int OfficeId);
        Task<IEnumerable<Office>> GetOffices();
        Task InsertOffice(Office Office);
        Task UpdateOffice(Office Office);
    }
}