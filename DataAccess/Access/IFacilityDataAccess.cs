using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IFacilityDataAccess
    {
        Task Delete(int facilityId);
        Task<IEnumerable<Facility>> Get();
        Task<Facility> Get(int facilityId);
        Task Insert(Facility facility);
        Task Update(Facility facility);
    }
}