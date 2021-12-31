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
        protected List<string> _restrictedInDropdown;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected void AddPropertiesNamesToDropdown()
        {
            _propertiesNames = new Dictionary<string, Tuple<string, Type>>();
            var listOfFields = typeof(T).GetProperties();
            _options = new List<SelectListItem>();

            foreach (var field in listOfFields)
            {
                if (!_restrictedInDropdown.Any(str => field.Name.ToLower() == str))
                {
                    string name = string.Join(" ", Regex.Split(field.Name, @"(?<!^)(?=[A-Z])"));
                    
                    if (name == "Salary Min")
                        name = "Minimum Salary";
                    else if (name == "Salary Max")
                        name = "Maximum Salary";
                    else if (name == "Position Name")
                        name = "Position";
                    else if (name == "Facility Address")
                        name = "Facility";

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
