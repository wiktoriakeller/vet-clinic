using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string AppointmentDate { get; set; }
        public string Cause { get; set; }
        public int Office { get; set; }
        public int Employee { get; set; }
        public int Patient { get; set; }
    }
}
