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
        public float SalaryMin { get; set; }
        public float SalaryMax { get; set; }
    }
}
