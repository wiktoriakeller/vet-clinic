using Microsoft.AspNetCore.Mvc;
using DataAccess.Access;
using DataAccess.Models;
using System.Collections.Generic;
using VetClinicWeb.Models;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VetClinicWeb.Controllers
{
    public class FacilityController : BaseOperationsController<Facility, FacilityViewModel>
    {
        public FacilityController(IDataAccess<Facility> facilityDataAccess, IMapper mapper) : base(mapper, facilityDataAccess) { }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsAddressUnique(string address, int facilityId)
        {
            address = address.Trim();
            var results = await _dataAccess.Get();
            bool isAddressInUse = results.FirstOrDefault(x => (x.Address == address && x.FacilityId != facilityId)) == null;

            if (isAddressInUse == false)
                return Json($"Address {address} already in use.");
            else
                return Json(true);
        }
    }
}
