using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class OrganizationDataAccess : Access, IDataAccess<Organization>
    {
        public OrganizationDataAccess(ISQLDataAccess db) : base(db) { }

        public async Task<IEnumerable<Organization>> Get()
        {
            string sql = "select * from organizations";
            var results = await _db.LoadData<Organization>(sql);
            results.ToList().ForEach(o => o.NameNIP = $"{o.Name} {o.NIP[..4]}");
            return results;
        }

        public async Task<Organization> Get(int organizationId)
        {
            string sql = "select * from organizations where organizationId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = organizationId
            });

            var results = await _db.LoadData<Organization, DynamicParameters>(sql, dynamicParameters);
            var first = results.First();
            first.NameNIP = $"{first.Name} {first.NIP[..4]}";
            return first;
        }

        public Task Insert(Organization organization)
        {
            string sql = "insert into organizations(name, nip, address, phoneNumber) values(:Name, :NIP, :Address, :PhoneNumber)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Name = organization.Name,
                NIP = organization.NIP,
                Address = organization.Address,
                PhoneNumber = organization.PhoneNumber
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Organization organization)
        {
            string sql = "update organizations set name = :Name, nip = :NIP, address = :Address, phoneNumber = :PhoneNumber where organizationId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = organization.OrganizationId,
                Name = organization.Name,
                NIP = organization.NIP,
                Address = organization.Address,
                PhoneNumber = organization.PhoneNumber
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int organizationId)
        {
            string sql = "delete from organizations where organizationId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = organizationId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
