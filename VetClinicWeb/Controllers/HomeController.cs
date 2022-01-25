using AutoMapper;
using DataAccess.Access;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VetClinicWeb.Models;

namespace VetClinicWeb.Controllers
{
    public class HomeController : BaseController<FacilityViewModel>
    {

        private readonly IDataAccess<Facility> _facilityDataAccess;
        

        public HomeController(IMapper mapper, IDataAccess<Facility> facilityDataAccess) : base(mapper)
        {
            _facilityDataAccess = facilityDataAccess;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int year = 2022)
        {
            ViewBag.error = "";
            if (year <= 2021)
            {
                year = DateTime.Now.Year;
                ViewBag.error = "Year shouldn't be lower than 2022.";
            }
            else if(year > 2099)
            {
                year = DateTime.Now.Year;
                ViewBag.error = "Year shouldn't be higher than 2099.";
            }

            ViewBag.Year = year;

            var dbEntities = await _facilityDataAccess.Get();
            var entities = new List<FacilityViewModel>();

            foreach (var dbEntity in dbEntities)
            {
                var facility = _mapper.Map<FacilityViewModel>(dbEntity);
                var dataAccess = (IProcedureResultDataAccess)_facilityDataAccess;
                facility.Income = await dataAccess.GetIncome(facility.FacilityId, year);
                entities.Add(facility);
            }
            
            return View(entities);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
