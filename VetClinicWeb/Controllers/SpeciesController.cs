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
    public class SpeciesController : BaseOperationsController<Species, SpeciesViewModel>
    {
        public SpeciesController(IDataAccess<Species> specieDataAccess, IMapper mapper) : base(mapper, specieDataAccess) 
        {
            _restrictedInDropdown = new List<string> { "speciesid" };
            AddPropertiesNamesToDropdown();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsSpeciesNameUnique(string name, int speciesId)
        {
            var results = await _dataAccess.Get();
            bool isSpeciesInUse = results.FirstOrDefault(x => (x.Name == name && x.SpeciesId != speciesId)) == null;

            if (isSpeciesInUse == false)
                return Json($"Species {name} already exists.");
            else
                return Json(true);
        }
    }
}
