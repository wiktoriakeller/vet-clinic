using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class ServiceDataAccess : Access, IDataAccess<Service>
    {
        public ServiceDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<Service>> Get()
        {
            string sql = "select * from services";
            return _db.LoadData<Service>(sql);
        }

        public async Task<Service> Get(int serviceId)
        {
            string sql = "select * from services where serviceId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = serviceId
            });

            var results = await _db.LoadData<Service, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(Service service)
        {
            string sql = "insert into services(Name, Price, Description, ServiceType) VALUES(:Name, :Price, :Description, :ServiceType);";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Name = service.Name,
                Price = service.Price,
                Description = service.Description,
                ServiceType = service.ServiceType
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Service service)
        {
            string sql = @"
                update services
                set name = :Name,
                    description = :Description,
                    price = :Price,
                    serviceType = :Owner
                where serviceId = :ServiceType";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = service.ServiceId,
                Name = service.Name,
                Price = service.Price,
                Description = service.Description,
                ServiceType = service.ServiceType
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int serviceId)
        {
            string sql = "delete from services where serviceId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = serviceId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
