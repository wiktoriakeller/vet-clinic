using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class FacilityDataAccess : Access, IDataAccess<Facility>
    {
        public FacilityDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<Facility>> Get()
        {
            string sql = "select * from facilities";
            return _db.LoadData<Facility>(sql);
        }

        public async Task<Facility> Get(int facilityId)
        {
            string sql = "select * from facilities where facilityId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = facilityId
            });

            var results = await _db.LoadData<Facility, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(Facility facility)
        {
            string sql = "insert into facilities(address, phoneNumber) values(:Address, :PhoneNumber)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Address = facility.Address,
                PhoneNumber = facility.PhoneNumber
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Facility facility)
        {
            string sql = "update facilities set address = :Address, phoneNumber = :PhoneNumber where facilityId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = facility.FacilityId,
                Address = facility.Address,
                PhoneNumber = facility.PhoneNumber
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int facilityId)
        {
            string sql = "delete from facilities where facilityId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = facilityId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
