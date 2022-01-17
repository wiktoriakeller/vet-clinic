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
        public SpeciesController(IDataAccess<Species> specieDataAccess, IMapper mapper) : base(mapper, specieDataAccess) 
        {
            _restrictedInDropdown = new List<string> { "speciesid" };
            AddPropertiesNamesToDropdown();
        }
    }
}
