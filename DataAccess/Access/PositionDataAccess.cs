using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class PositionDataAccess : Access
    {
        public PositionDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<Position>> GetPositions()
        {
            string sql = "select * from positions";
            return _db.LoadData<Position>(sql);
        }

        public async Task<Position> GetPosition(int positionId)
        {
            string sql = "select * from positions where positionId = :Id";
            
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = positionId
            });

            var results = await _db.LoadData<Position, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task InsertPosition(Position position)
        {
            string sql = "insert into positions(name, salaryMin, salaryMax) values(:Name, :SalaryMin, :SalaryMax)";
            
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Name = position.Name,
                SalaryMin = position.SalaryMin,
                SalaryMax = position.SalaryMax
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task UpdatePosition(Position position)
        {
            string sql = "update positions set name = :Name, salaryMin = :SalaryMin, salaryMax = :SalaryMax where positionId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = position.PositionId,
                Name = position.Name,
                SalaryMin = position.SalaryMin,
                SalaryMax = position.SalaryMax
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task DeletePosition(int positionId)
        {
            string sql = "delete from positions where positionId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = positionId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
