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
    public class PositionController : BaseController
    {
        private readonly IPositionDataAccess _positionDataAccess;

        public PositionController(IPositionDataAccess positionDataAccess, IMapper mapper) : base(mapper)
        {
            _positionDataAccess = positionDataAccess;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Position> dbPositions = await _positionDataAccess.Get();
            List<PositionViewModel> positions = new List<PositionViewModel>();

            foreach (Position dbPosition in dbPositions)
                positions.Add(_mapper.Map<PositionViewModel>(dbPosition));

            return View(positions);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PositionViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _positionDataAccess.Insert(_mapper.Map<Position>(model));
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Position position = await _positionDataAccess.Get(id);
            return View(_mapper.Map<PositionViewModel>(position));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PositionViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _positionDataAccess.Update(_mapper.Map<Position>(model));
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
            Position position = await _positionDataAccess.Get(id);
            return View(_mapper.Map<PositionViewModel>(position));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, PositionViewModel model)
        {
            try
            {
                await _positionDataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = $"Position {GetExceptionMessage(ex.Number)}";
                var facility = await _positionDataAccess.Get(id);
                return View(_mapper.Map<PositionViewModel>(facility));
            }
            return RedirectToAction("Index");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsSalaryValid(PositionViewModel model)
        {
            double? salaryMin = model.SalaryMin;
            double? salaryMax = model.SalaryMax;

            if (salaryMin != null && salaryMax != null && salaryMax < salaryMin)
                return Json("Maximum salary should be bigger than minimum salary");

            return Json(true);
        }
    }
}
