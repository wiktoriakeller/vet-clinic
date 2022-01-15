using Dapper;
using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public class DrugDataAccess : Access, IDataAccess<Drug>
    {
        public DrugDataAccess(ISQLDataAccess db) : base(db) { }

        public Task Delete(int id)
        {
            string sql = "delete from drugs where DrugId = :Id";
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = id
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public async Task<Drug> Get(int id)
        {
            string sql = "select * from drugs where DrugId = :Id";
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = id
            });
            var results = await _db.LoadData<Drug, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task<IEnumerable<Drug>> Get()
        {
            string sql = "select * from drugs";
            return _db.LoadData<Drug>(sql);
        }

        public Task Insert(Drug entity)
        {
            string sql = "insert into drugs(name, manufacturer) values(:Mame, :Manufacturer)";
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Name = entity.Name,
                Manufacturer = entity.Manufacturer
            });
            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Drug entity)
        {
            string sql = "update drugs set name = :Name, manufacturer = :Manufacturer where DrugId = :Id";
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = entity.DrugId,
                Name = entity.Name,
                Manufacturer = entity.Manufacturer
            });
            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
