using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class OfficeDataAccess : Access, IOfficeDataAccess
    {
        public OfficeDataAccess(ISQLDataAccess db) : base(db) { }
        public Task<IEnumerable<Office>> GetOffices()
        {
            string sql = "select * from offices";
            return _db.LoadData<Office>(sql);
        }

        public async Task<Office> Get(int OfficeId)
        {
            string sql = "select * from offices where officeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = OfficeId
            });

            var results = await _db.LoadData<Office, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(Office Office)
        {
            string sql = "insert into offices(officeNumber, facility) values(:OfficeNumber, :Facility)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                OfficeNumber = Office.OfficeNumber,
                Facility = Office.Facility
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Office Office)
        {
            string sql = "update Offices set officeNumber = :OfficeNumber, facility = :Facility where officeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = Office.OfficeId,
                OfficeNumber = Office.OfficeNumber,
                Facility = Office.Facility
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int OfficeId)
        {
            string sql = "delete from offices where officeId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = OfficeId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
