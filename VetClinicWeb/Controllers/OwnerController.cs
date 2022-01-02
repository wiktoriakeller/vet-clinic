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
    public class OwnerController : BaseOperationsController<Owner, OwnerViewModel>
    {
        public OwnerController(IMapper mapper, IDataAccess<Owner> dataAccess) : base(mapper, dataAccess)
        {
            _restrictedInDropdown = new List<string> { "ownerid" };
            AddPropertiesNamesToDropdown();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsPESELUnique(string PESEL, int ownerId)
        {
            PESEL = PESEL.Trim();
            var results = await _dataAccess.Get();
            bool isPESELInUse = results.FirstOrDefault(x => (x.PESEL == PESEL && x.OwnerId != ownerId)) == null;

            if (isPESELInUse == false)
                return Json($"PESEL {PESEL} already in use.");
            else
                return Json(true);
        }
    }
}
