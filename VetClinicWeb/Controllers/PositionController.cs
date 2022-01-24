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
        public PositionController(IDataAccess<Position> positionDataAccess, IMapper mapper) : base(mapper, positionDataAccess) 
        {
            _restrictedInDropdown = new List<string> { "positionid" };
            AddPropertiesNamesToDropdown();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsSalaryValid(PositionViewModel model)
        {
            double? salaryMin = model.SalaryMin;
            double? salaryMax = model.SalaryMax;

            if (salaryMin != null && salaryMax != null && salaryMax < salaryMin)
                return Json("Maximum salary should be bigger than minimum salary.");

            return Json(true);
        }

        [HttpPost]
        public override async Task<IActionResult> Delete(int id, PositionViewModel model)
        {
            var entity = await _dataAccess.Get(id);
            if (entity.Name == "Veterinarian")
            {
                ViewBag.ErrorMessage = "You can't delete Veterinarian.";
                return View(_mapper.Map<PositionViewModel>(entity));
            }

            try
            {
                await _dataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                return View(_mapper.Map<PositionViewModel>(entity));
            }

            return RedirectToAction("Index");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsPositionUnique(string name, int positionId)
        {
            var results = await _dataAccess.Get();
            bool isPositionInUse = results.FirstOrDefault(x => (x.Name == name && x.PositionId != positionId)) == null;

            if (isPositionInUse == false)
                return Json($"Position {name} is already in use.");
            else
                return Json(true);
        }
    }
}
