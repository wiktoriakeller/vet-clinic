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
    public class FacilityController : BaseController
    {
        private readonly IDataAccess<Facility> _facilityDataAccess;
        private List<SelectListItem> _options;

        public FacilityController(IDataAccess<Facility> facilityDataAccess, IMapper mapper) : base(mapper)
        {
            _facilityDataAccess = facilityDataAccess;
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

            var dbFacilities = await _facilityDataAccess.Get();
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FacilityViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _facilityDataAccess.Insert(_mapper.Map<Facility>(model));
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var facility = await _facilityDataAccess.Get(id);
            return View(_mapper.Map<FacilityViewModel>(facility));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, FacilityViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _facilityDataAccess.Update(_mapper.Map<Facility>(model));
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ShowDelete(int id)
        {
            return RedirectToAction("Delete", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var facility = await _facilityDataAccess.Get(id);
            return View(_mapper.Map<FacilityViewModel>(facility));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, FacilityViewModel model)
        {
            try
            {
                await _facilityDataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = $"Facility {GetExceptionMessage(ex.Number)}";
                var facility = await _facilityDataAccess.Get(id);
                return View(_mapper.Map<FacilityViewModel>(facility));
            }

            return RedirectToAction("Index");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsAddressUnique(string address, int facilityId)
        {
            address = address.Trim();
            var results = await _facilityDataAccess.Get();
            bool isAddressInUse = results.FirstOrDefault(x => (x.Address == address && x.FacilityId != facilityId)) == null;

            if (isAddressInUse == false)
                return Json($"Address {address} already in use.");
            else
                return Json(true);
        }
    }
}
