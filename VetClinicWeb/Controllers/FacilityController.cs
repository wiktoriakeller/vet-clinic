using Microsoft.AspNetCore.Mvc;
using DataAccess.Access;
using DataAccess.Models;
using System.Collections.Generic;
using VetClinicWeb.Models;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using System;

namespace VetClinicWeb.Controllers
{
    public class FacilityController : BaseController
    {
        private readonly IFacilityDataAccess _facilityDataAccess;

        public FacilityController(IFacilityDataAccess facilityDataAccess, IMapper mapper) : base(mapper)
        {
            _facilityDataAccess = facilityDataAccess;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dbFacilities = await _facilityDataAccess.GetFacilities();
            List<FacilityViewModel> facilities = new List<FacilityViewModel>();

            foreach (var dbFacility in dbFacilities)
            {
                facilities.Add(_mapper.Map<FacilityViewModel>(dbFacility));
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
                await _facilityDataAccess.InsertFacility(_mapper.Map<Facility>(model));
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var facility = await _facilityDataAccess.GetFacility(id);
            return View(_mapper.Map<FacilityViewModel>(facility));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, FacilityViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _facilityDataAccess.UpdateFacility(_mapper.Map<Facility>(model));
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
            var facility = await _facilityDataAccess.GetFacility(id);
            return View(_mapper.Map<FacilityViewModel>(facility));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, FacilityViewModel model)
        {
            try
            {
                await _facilityDataAccess.DeleteFacility(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                switch (ex.Number)
                {
                    case 2292:
                        ViewBag.ErrorMessage = "Facility is in use.";
                        break;
                    default:
                        ViewBag.ErrorMessage = "You can't delete this facility.";
                        break;
                }

                var facility = await _facilityDataAccess.GetFacility(id);
                return View(_mapper.Map<FacilityViewModel>(facility));
            }

            return RedirectToAction("Index");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsAddressUnique(string address, int facilityId)
        {
            address = address.Trim();
            var results = await _facilityDataAccess.GetFacilities();
            bool isAddressInUse = results.FirstOrDefault(x => (x.Address == address && x.FacilityId != facilityId)) == null;

            if (isAddressInUse == false)
            {
                return Json($"Address {address} already in use.");
            }
            else
            {
                return Json(true);
            }
        }
    }
}
