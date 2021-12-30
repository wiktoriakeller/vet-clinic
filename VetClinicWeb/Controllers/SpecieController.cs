using AutoMapper;
using DataAccess.Access;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinicWeb.Models;

namespace VetClinicWeb.Controllers
{
    public class SpecieController : BaseOperationsController<Specie, SpecieViewModel>
    {
        public SpecieController(IDataAccess<Specie> specieDataAccess, IMapper mapper) : base(mapper, specieDataAccess) { }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Specie> dbSpecies = await _dataAccess.Get();
            var species = new List<SpecieViewModel>();

            foreach (Specie dbSpecie in dbSpecies)
                species.Add(_mapper.Map<SpecieViewModel>(dbSpecie));

            return View(species);
        }
    }
}
