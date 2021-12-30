using AutoMapper;
using DataAccess.Access;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinicWeb.Models;

namespace VetClinicWeb.Controllers
{
    public class SpeciesController : BaseOperationsController<Species, SpeciesViewModel>
    {
        public SpeciesController(IDataAccess<Species> specieDataAccess, IMapper mapper) : base(mapper, specieDataAccess) { }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Species> dbSpecies = await _dataAccess.Get();
            var species = new List<SpeciesViewModel>();

            foreach (Species dbSpecie in dbSpecies)
                species.Add(_mapper.Map<SpeciesViewModel>(dbSpecie));

            return View(species);
        }
    }
}
