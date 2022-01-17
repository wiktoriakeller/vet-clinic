using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Position
    {
        public int PositionId { get; set; }
        public string Name { get; set; }
        public double SalaryMin { get; set; }
        public double SalaryMax { get; set; }
    }
}
