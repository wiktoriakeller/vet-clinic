using AutoMapper;
using DataAccess.Access;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinicWeb.Models;
using System.Linq;

namespace VetClinicWeb.Controllers
{
    public class OrganizationController : BaseOperationsController<Organization, OrganizationViewModel>
    {
        public OrganizationController(IMapper mapper, IDataAccess<Organization> dataAccess) : base(mapper, dataAccess) 
        {
            _restrictedInDropdown = new List<string> { "organizationid" };
            AddPropertiesNamesToDropdown();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsNIPUnique(string NIP, int organizationId)
        {
            NIP = NIP.Trim();
            var results = await _dataAccess.Get();
            bool isPESELInUse = results.FirstOrDefault(x => (x.NIP == NIP && x.OrganizationId != organizationId)) == null;

            if (isPESELInUse == false)
                return Json($"NIP {NIP} already in use.");
            else
                return Json(true);
        }
    }
}
