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
    public class SpeciesDataAccess : Access, IDataAccess<Species>
    {
        public SpeciesDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<Species>> Get()
        {
            string sql = "select * from Species";
            return _db.LoadData<Species>(sql);
        }

        public async Task<Species> Get(int speciesId)
        {
            string sql = "select * from Species where speciesId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = speciesId
            });

            var results = await _db.LoadData<Species, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(Species species)
        {
            string sql = "insert into Species(Name) values(:Name)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Name = species.Name
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Species species)
        {
            string sql = "update Species set name = :name where SpeciesId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = species.SpeciesId,
                name = species.Name
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int speciesId)
        {
            string sql = "delete from Species where SpeciesId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = speciesId
            });

            return _db.SaveData(sql, dynamicParameters);
        }

    }
}
