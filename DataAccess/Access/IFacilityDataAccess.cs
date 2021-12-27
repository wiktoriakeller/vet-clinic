using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IFacilityDataAccess
    {
        Task DeleteFacility(int facilityId);
        Task<IEnumerable<Facility>> GetFacilities();
        Task<Facility> GetFacility(int facilityId);
        Task InsertFacility(Facility facility);
        Task UpdateFacility(Facility facility);
    }
}