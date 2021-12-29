using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IPositionDataAccess
    {
        Task Delete(int positionId);
        Task<Position> Get(int positionId);
        Task<IEnumerable<Position>> Get();
        Task Insert(Position position);
        Task Update(Position position);
    }
}