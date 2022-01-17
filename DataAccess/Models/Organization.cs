using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set;}
        public string NIP { get; set;}
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string NameNIP { get; set; }
    }
}
