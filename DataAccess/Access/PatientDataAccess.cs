using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class PatientDataAccess : Access, IDataAccess<Patient>
    {
        public PatientDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<Patient>> Get()
        {
            string sql = "select * from patients";
            return _db.LoadData<Patient>(sql);
        }

        public async Task<Patient> Get(int patientId)
        {
            string sql = "select * from patients where patientId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = patientId
            });

            var results = await _db.LoadData<Patient, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(Patient patient)
        {
            string sql = "insert into patients(name, species, organization, owner) values(:Name, :Species, :Organization, :Owner)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Name = patient.Name,
                Species = patient.Species,
                Organization = patient.Organization,
                Owner = patient.Owner
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(Patient patient)
        {
            string sql = @"
                update patients
                set name = :Name,
                    species = :Species,
                    organization = :Organization,
                    owner = :Owner
                where patientId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = patient.PatientId,
                Name = patient.Name,
                Species = patient.Species,
                Organization = patient.Organization,
                Owner = patient.Owner
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int patientId)
        {
            string sql = "delete from patients where patientId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = patientId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
