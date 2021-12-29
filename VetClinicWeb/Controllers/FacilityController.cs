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
                    _options.Add(new SelectListItem { Text = string.Join(" ", Regex.Split(field, @"(?<!^)(?=[A-Z])")) });
            }

            _options.Add(new SelectListItem { Text = "Any" });
        }

        [HttpGet]
        public async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;

            var dbFacilities = await _dataAccess.Get();
            var facilities = new List<FacilityViewModel>();

            foreach (var dbFacility in dbFacilities)
                facilities.Add(_mapper.Map<FacilityViewModel>(dbFacility));

            if(!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                search = search.ToLower();
                option = option.ToLower();
                var searched = new List<FacilityViewModel>();

                if (option == "address" || option == "any")
                    searched.AddRange(facilities.Where(fac => fac.Address.ToLower().Contains(search)));

                if (option == "phone number" || option == "any")
                    searched.AddRange(facilities.Where(fac => fac.PhoneNumber.ToLower().Contains(search)));

                return View(searched);
            }

            return View(facilities);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, FacilityViewModel model)
        {
            try
            {
                await _dataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = $"Facility {GetExceptionMessage(ex.Number)}";
                var entity = await _dataAccess.Get(id);
                return View(_mapper.Map<FacilityViewModel>(entity));
            }

            return RedirectToAction("Index");
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
