using AutoMapper;
using DataAccess.Access;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinicWeb.Models;

namespace VetClinicWeb.Controllers
{
    public class OfficeController : BaseController<OfficeViewModel>
    {
        private readonly IDataAccess<Office> _officeDataAccess;
        private readonly IDataAccess<Facility> _facilityDataAccess;
        public OfficeController(IMapper mapper,
                                IDataAccess<Facility> facilityDataAccess,
                                IDataAccess<Office> officeDataAccess) : base(mapper)
        {
            _facilityDataAccess = facilityDataAccess;
            _officeDataAccess = officeDataAccess;
            _restrictedInDropdown = new List<string> {"officeid", "facility"};
            AddPropertiesNamesToDropdown();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;
            List<OfficeViewModel> offices = await GetFullOffices();

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                var searched = Search(search, option, offices);
                return View(searched);
            }

            return View(offices);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await UpdateDropdownLists();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OfficeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _officeDataAccess.Insert(_mapper.Map<Office>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    await UpdateDropdownLists();
                    ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                    return View(model);
                }
            }

            await UpdateDropdownLists();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var office = await GetFullOffice(id);
            await UpdateDropdownLists();
            return View(office);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, OfficeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _officeDataAccess.Update(_mapper.Map<Office>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    await UpdateDropdownLists();
                    ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                    return View(model);
                }
            }

            await UpdateDropdownLists();
            return View(model);
        }

        [HttpGet]
        public IActionResult ShowDelete(int id)
        {
            return RedirectToAction("Delete", new { id = id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await GetFullOffice(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, OfficeViewModel collection)
        {
            try
            {
                await _officeDataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                return View(GetFullOffice(id));
            }

            return RedirectToAction("Index");
        }

        public async Task<List<OfficeViewModel>> GetFullOffices()
        {
            var dbOffices = await _officeDataAccess.Get();

            var offices = new List<OfficeViewModel>();
            var facilities = (List<Facility>)await _facilityDataAccess.Get();
            IDictionary<int, Facility> facilitiesDic = facilities.ToDictionary(p => p.FacilityId);

            foreach (var office in dbOffices)
            {
                offices.Add(_mapper.Map<OfficeViewModel>(office));
                offices.Last().FacilityAddress = facilitiesDic[offices.Last().Facility].Address;
            }

            return offices;
        }

        public async Task<OfficeViewModel> GetFullOffice(int id)
        {
            var offices = await GetFullOffices();
            return offices.Where(o => o.OfficeId == id).FirstOrDefault();
        }

        private async Task UpdateDropdownLists()
        {
            var facilities = (List<Facility>)await _facilityDataAccess.Get();
            ViewBag.facilities = new SelectList(facilities, "FacilityId", "Address");
        }
    }
}
