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
    public class SpecieDataAccess : Access, ISpecieDataAccess
    {
        public SpecieDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<Specie>> Get()
        {
            string sql = "select * from Species";
            return _db.LoadData<Specie>(sql);
        }

        public async Task<Specie> Get(int specieId)
        {
            string sql = "select * from Species where speciesId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = specieId
            });

            var results = await _db.LoadData<Specie, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(Specie specie)
        {
            string sql = "insert into Species(Name) values(:Name)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Name = specie.Name
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Specie specie)
        {
            string sql = "update Species set name = :name where SpeciesId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = specie.SpeciesId,
                name = specie.Name
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int specieId)
        {
            string sql = "delete from Species where SpeciesId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = specieId
            });

            return _db.SaveData(sql, dynamicParameters);
        }

    }
}
