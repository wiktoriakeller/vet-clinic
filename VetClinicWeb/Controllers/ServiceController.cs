using Microsoft.AspNetCore.Mvc;
using DataAccess.Access;
using DataAccess.Models;
using System.Collections.Generic;
using VetClinicWeb.Models;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VetClinicWeb.Controllers
{
    public class ServiceController : BaseOperationsController<Service, ServiceViewModel>
    {
        public ServiceController(IMapper mapper, IDataAccess<Service> dataAccess) : base(mapper, dataAccess)
        {
            _restrictedInDropdown = new List<string> { "serviceid", "description" };
            AddPropertiesNamesToDropdown();
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _dataAccess.Get(id);
            return View(_mapper.Map<ServiceViewModel>(entity));
        }
    }
}
