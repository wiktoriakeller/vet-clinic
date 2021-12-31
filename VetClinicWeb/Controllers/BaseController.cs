using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace VetClinicWeb.Controllers
{
    public abstract class BaseController<T> : Controller
    {
        protected readonly IMapper _mapper;
        protected IDictionary<string, Tuple<string, Type>> _propertiesNames;
        protected List<SelectListItem> _options;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;

            _propertiesNames = new Dictionary<string, Tuple<string, Type>>();
            var listOfFields = typeof(T).GetProperties();
            var restricted = new List<string> { "id" };
            _options = new List<SelectListItem>();

            foreach (var field in listOfFields)
            {
                if (!restricted.Any(str => field.Name.ToLower().Contains(str)))
                {
                    string name = string.Join(" ", Regex.Split(field.Name, @"(?<!^)(?=[A-Z])"));
                    if (name == "Salary Min")
                        name = "Minimum Salary";

                    if (name == "Salary Max")
                        name = "Maximum Salary";

                    _options.Add(new SelectListItem { Text = name });
                    _propertiesNames[name] = Tuple.Create(field.Name, field.PropertyType);
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
