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
    public class PrescriptionDataAccess : Access, IPrescriptionDataAccess
    {
        public PrescriptionDataAccess(ISQLDataAccess db) : base(db) { }

        public Task Delete(int appointmentId, int drugId)
        {
            string sql = "delete from prescriptions where appointmentId = :AppointmentId and drugId = :DrugId";
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                AppointmentId = appointmentId,
                DrugId = drugId
            });
            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int appointmentId)
        {
            string sql = "delete from prescriptions where appointmentId = :AppointmentId";
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                AppointmentId = appointmentId
            });
            return _db.SaveData(sql, dynamicParameters);
        }

        public async Task<Prescription> Get(int appointmentId, int drugId)
        {
            string sql = "select * from prescriptions where appointmentId = :AppointmentId and drugId = :DrugId";
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                AppointmentId = appointmentId,
                DrugId = drugId
            });
            var result = await _db.LoadData<Prescription, DynamicParameters>(sql, dynamicParameters);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Prescription>> Get(int appointmentId)
        {
            string sql = "select * from prescriptions where appointmentId = :AppointmentId";
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                AppointmentId = appointmentId
            });
            var results = await _db.LoadData<Prescription, DynamicParameters>(sql, dynamicParameters);

            return results;
        }

        public Task Update(Prescription prescription)
        {
            string sql = "update prescriptions set drugId = :DrugId, dosage = :Dosage where prescriptionId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = prescription.PrescriptionId,
                Dosage = prescription.Dosage,
                DrugId = prescription.DrugId
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Insert(Prescription prescription)
        {
            string sql = "insert into prescriptions(Dosage, DrugId, AppointmentId) values(:dosage, :drugId, :appointmentId)";
            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Dosage = prescription.Dosage,
                DrugId = prescription.DrugId,
                AppointmentId = prescription.AppointmentId
            });
            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
