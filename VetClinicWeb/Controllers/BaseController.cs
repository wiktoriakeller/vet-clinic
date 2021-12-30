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
using System.Reflection;

namespace VetClinicWeb.Controllers
{
    public abstract class BaseController<T> : Controller
    {
        protected readonly IMapper _mapper;
        protected IDictionary<string, string> _propertiesNames;
        protected List<SelectListItem> _options;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;

            _propertiesNames = new Dictionary<string, string>();
            var listOfFieldNames = typeof(T).GetProperties().Select(f => f.Name).ToList();
            var restricted = new List<string> { "id" };
            _options = new List<SelectListItem>();

            foreach (var field in listOfFieldNames)
            {
                if (!restricted.Any(str => field.ToLower().Contains(str)))
                {
                    string name = string.Join(" ", Regex.Split(field, @"(?<!^)(?=[A-Z])"));
                    if (name == "Salary Min")
                        name = "Minimum Salary";

                    if (name == "Salary Max")
                        name = "Maximum Salary";

                    _options.Add(new SelectListItem { Text = name });
                    _propertiesNames[name] = field;
                }
            }

            _options.Add(new SelectListItem { Text = "Any" });
        }

        protected string GetExceptionMessage(int number)
        {
            switch (number)
            {
                case 2292:
                    return "is used in diffrent table";
                default:
                    return "can't be modified or deleted";
            }
        }
    }
}
