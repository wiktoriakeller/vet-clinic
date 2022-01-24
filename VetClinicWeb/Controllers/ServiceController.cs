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
            _restrictedInDropdown = new List<string> { "serviceid", "description", "servicetype" };
            AddPropertiesNamesToDropdown();
        }

        [HttpGet]
        public override async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;
            var dbEntities = await _dataAccess.Get();
            var entities = new List<ServiceViewModel>();

            foreach (var dbEntity in dbEntities)
            {
                entities.Add(_mapper.Map<ServiceViewModel>(dbEntity));
                if(entities.Last().ServiceType == 'E')
                {
                    entities.Last().FullServiceType = "Examination";
                }
                else
                {
                    entities.Last().FullServiceType = "Treatment";
                }
            }


            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                var searched = Search(search, option, entities);
                return View(searched);
            }

            return View(entities);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _dataAccess.Get(id);
            return View(_mapper.Map<ServiceViewModel>(entity));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsServiceNameUnique(string name, int serviceId)
        {
            name = name.Trim();
            var results = await _dataAccess.Get();
            bool isServiceInUse = results.FirstOrDefault(x => (x.Name == name && x.ServiceId != serviceId)) == null;

            if (isServiceInUse == false)
                return Json($"Service {name} already exists.");
            else
                return Json(true);
        }
    }
}
