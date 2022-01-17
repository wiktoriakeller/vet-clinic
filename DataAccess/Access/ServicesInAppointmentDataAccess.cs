using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class ServicesInAppointmentDataAccess : Access, IDataAccess<ServicesInAppointment>
    {
        public ServicesInAppointmentDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<ServicesInAppointment>> Get()
        {
            string sql = "select * from servicesInAppointments";
            return _db.LoadData<ServicesInAppointment>(sql);
        }

        public async Task<ServicesInAppointment> Get(int servicesInAppointmentId)
        {
            string sql = "select * from servicesInAppointments where servicesInAppointmentId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = servicesInAppointmentId
            });

            var results = await _db.LoadData<ServicesInAppointment, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(ServicesInAppointment servicesInAppointment)
        {
            string sql = "insert into servicesInAppointments(diagnosis, appointmentId, service) values(:Diagnosis, :AppointmentId, :Service)";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Diagnosis = servicesInAppointment.Diagnosis,
                AppointmentId = servicesInAppointment.AppointmentId,
                Service = servicesInAppointment.Service
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Update(ServicesInAppointment servicesInAppointment)
        {
            string sql = "update servicesInAppointments set diagnosis = :Diagnosis, service = :Service where servicesInAppointmentId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = servicesInAppointment.ServicesInAppointmentId,
                Diagnosis = servicesInAppointment.Diagnosis,
                Service = servicesInAppointment.Service
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int servicesInAppointmentId)
        {
            string sql = "delete from servicesInAppointments where servicesInAppointmentId = :Id";

            DynamicParameters dynamicParameters = new DynamicParameters(new
            {
                Id = servicesInAppointmentId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
