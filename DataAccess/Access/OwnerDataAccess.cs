using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class OwnerDataAccess : Access, IDataAccess<Owner>
    {
        public OwnerDataAccess(ISQLDataAccess db) : base(db) { }

        public async Task<IEnumerable<Owner>> Get()
        {
            string sql = "select * from owners";
            var results = await _db.LoadData<Owner>(sql);
            results.ToList().ForEach(o => o.NameSurnamePESEL = $"{o.Name} {o.Surname} {o.PESEL[..4]}");
            return results;
        }

        public async Task<Owner> Get(int ownerId)
        {
            string sql = "select * from owners where ownerId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = ownerId
            });

            var results = await _db.LoadData<Owner, DynamicParameters>(sql, dynamicParameters);
            var first = results.First();
            first.NameSurnamePESEL = $"{first.Name} {first.Surname} {first.PESEL[..4]}";
            return first;
        }

        public Task Insert(Owner owner)
        {
            string sql = "insert into owners(pesel, name, surname, address, phoneNumber) values(:PESEL, :Name, :Surname, :Address, :PhoneNumber)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                PESEL = owner.PESEL,
                Name = owner.Name,
                Surname = owner.Surname,
                Address = owner.Address,
                PhoneNumber = owner.PhoneNumber
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Owner owner)
        {
            string sql = @"
                update owners
                set pesel = :PESEL,
                    name = :Name,
                    surname = :Surname,
                    address = :Address,
                    phoneNumber = :PhoneNumber
                where ownerId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = owner.OwnerId,
                PESEL = owner.PESEL,
                Name = owner.Name,
                Surname = owner.Surname,
                Address = owner.Address,
                PhoneNumber = owner.PhoneNumber
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int ownerId)
        {
            string sql = "delete from owners where ownerId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = ownerId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
