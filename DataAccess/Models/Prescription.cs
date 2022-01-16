using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public string Dosage { get; set; }
        public int DrugId { get; set; }
        public int AppointmentId { get; set; }
    }
}
