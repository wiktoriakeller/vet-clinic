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
    public class PositionController : BaseOperationsController<Position, PositionViewModel>
    {

        public PositionController(IDataAccess<Position> positionDataAccess, IMapper mapper) : base(mapper, positionDataAccess) { }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Position> dbPositions = await _dataAccess.Get();
            List<PositionViewModel> positions = new List<PositionViewModel>();

            foreach (Position dbPosition in dbPositions)
                positions.Add(_mapper.Map<PositionViewModel>(dbPosition));

            return View(positions);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, PositionViewModel model)
        {
            try
            {
                await _dataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = $"Position {GetExceptionMessage(ex.Number)}";
                var facility = await _dataAccess.Get(id);
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
