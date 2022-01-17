using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class ServicesInAppointment
    {
        public int ServicesInAppointmentId { get; set; }
        public string Diagnosis { get; set; }
        public int AppointmentId { get; set; }
        public int Service { get; set; }
    }
}
