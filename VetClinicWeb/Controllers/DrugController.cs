using AutoMapper;
using DataAccess.Access;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinicWeb.Models;

namespace VetClinicWeb.Controllers
{
    public class DrugController : BaseOperationsController<Drug, DrugViewModel>
    {
        public DrugController(IMapper mapper, IDataAccess<Drug> dataAccess) : base(mapper, dataAccess)
        {
            _restrictedInDropdown = new List<string> { "drugid" };
            AddPropertiesNamesToDropdown();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsDrugUnique(DrugViewModel model)
        {
            var results = await _dataAccess.Get();
            bool isDrugInUse = results.FirstOrDefault(x => (x.Name == model.Name && x.Manufacturer == model.Manufacturer && x.DrugId != model.DrugId)) == null;

            if (isDrugInUse == false)
                return Json($"This drug already exists.");
            else
                return Json(true);
        }
    }
}
