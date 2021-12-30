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
        private readonly List<SelectListItem> _options;

        public FacilityController(IDataAccess<Facility> facilityDataAccess, IMapper mapper) : base(mapper, facilityDataAccess)
        {
            var listOfFieldNames = typeof(Facility).GetProperties().Select(f => f.Name).ToList();
            var restricted = new List<string> { "id" };
            _options = new List<SelectListItem>();

            foreach (var field in listOfFieldNames)
            {
                if(!restricted.Any(str => field.ToLower().Contains(str)))
                {
                    string name = string.Join(" ", Regex.Split(field, @"(?<!^)(?=[A-Z])"));
                    _options.Add(new SelectListItem { Text = name });
                    _propertiesNames[name] = field;
                }
            }

            _options.Add(new SelectListItem { Text = "Any" });
        }

        [HttpGet]
        public async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;
            var dbEntities = await _dataAccess.Get();
            var entities = new List<FacilityViewModel>();

            foreach (var dbFacility in dbEntities)
                entities.Add(_mapper.Map<FacilityViewModel>(dbFacility));

            if(!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                search = search.ToLower();
                option = option.ToLower().Trim();
                var searched = new List<FacilityViewModel>();
                
                foreach(var val in _propertiesNames)
                {
                    if(option == val.Key.ToLower() || option == "any")
                        searched.AddRange(entities.Where(fac => fac.GetType().GetProperty(val.Value).GetValue(fac, null).ToString().ToLower().Contains(search)));
                }

                return View(searched);
            }

            return View(entities);
        }

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
