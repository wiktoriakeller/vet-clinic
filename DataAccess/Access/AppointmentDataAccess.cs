using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbAccess;
using DataAccess.Models;
using Dapper;

namespace DataAccess.Access
{
    public class AppointmentDataAccess : Access, IDataAccess<Appointment>, IProcedureDataAccess<Appointment, Facility>
    {
        public AppointmentDataAccess(ISQLDataAccess db) : base(db) { }

        public Task<IEnumerable<Appointment>> Get()
        {
            string sql = "select appointmentId, to_char(appointmentDate, 'DD/MM/YYYY HH24:MI') as appointmentDate, cause, office, employee, patient from appointments";
            return _db.LoadData<Appointment>(sql);
        }

        public async Task<Appointment> Get(int appointmentId)
        {
            string sql = "select * from appointments where appointmentId = :Id";

            var dynamicParameters = new DynamicParameters(new
            {
                Id = appointmentId
            });

            var results = await _db.LoadData<Appointment, DynamicParameters>(sql, dynamicParameters);
            return results.First();
        }

        public Task Insert(Appointment appointment)
        {
            string sql = @"
                insert into appointments(appointmentDate, cause, office, employee, patient)
                values(:AppointmentDate, :Casue, :Office, :Employee, :Patient)";

            var dynamicParameters = new DynamicParameters(new
            {
                Id = appointment.AppointmentId,
                AppointmentDate = appointment.AppointmentDate,
                Casue = appointment.Cause,
                Office = appointment.Office,
                Employee = appointment.Employee,
                Patient = appointment.Patient
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Insert(Appointment appointment, Facility facility)
        {
            string sql = $"call INF145189.BookAppointment(to_timestamp('{appointment.AppointmentDate}', 'DD/MM/YYYY HH24:MI'), '{appointment.Cause}', {facility.FacilityId}, {appointment.Patient})";
            return _db.ExecuteProcedure(sql);
        }

        public Task Update(Appointment appointment)
        {
            string sql = @"
                update appointments
                set appointmentDate = to_timestamp('" + appointment.AppointmentDate + @"', 'DD/MM/YYYY HH24:MI'),
                    cause = :Casue,
                    office = :Office,
                    employee = :Employee,
                    patient = :Patient
                where appointmentId = :Id";

            var dynamicParameters = new DynamicParameters(new
            {
                Id = appointment.AppointmentId,
                AppointmentDate = appointment.AppointmentDate,
                Casue = appointment.Cause,
                Office = appointment.Office,
                Employee = appointment.Employee,
                Patient = appointment.Patient
            });

            return _db.SaveData(sql, dynamicParameters);
        }

        public Task Delete(int appointmentId)
        {
            string sql = "delete from appointments where appointmentId = :Id";

            var dynamicParameters = new DynamicParameters(new
            {
                Id = appointmentId
            });

            return _db.SaveData(sql, dynamicParameters);
        }
    }
}
