using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IPositionDataAccess
    {
        Task DeletePosition(int positionId);
        Task<Position> GetPosition(int positionId);
        Task<IEnumerable<Position>> GetPositions();
        Task InsertPosition(Position position);
        Task UpdatePosition(Position position);
    }
}