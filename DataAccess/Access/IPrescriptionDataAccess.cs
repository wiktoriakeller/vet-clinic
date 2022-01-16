using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IPrescriptionDataAccess
    {
        Task Delete(int appointmentId, int drugId);
        Task<IEnumerable<Prescription>> Get(int appointmentId);
        Task<Prescription> Get(int appointmentId, int drugId);
        Task Insert(Prescription prescription);
        Task Update(Prescription prescription);
    }
}