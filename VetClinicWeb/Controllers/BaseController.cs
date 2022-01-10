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
                    var name = field.Name;

                    if (field.Name != "PESEL" && field.Name != "NIP")
                        name = string.Join(" ", Regex.Split(field.Name, @"(?<!^)(?=[A-Z])"));

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

        protected List<T> Search(string search, string option, List<T> entities)
        {
            search = search.ToLower().Trim();
            option = option.ToLower();
            var searched = new List<T>();

            foreach (var val in _propertiesNames)
            {
                if (option == val.Key.ToLower() || option == "any")
                {
                    if (val.Value.Item2 == typeof(string))
                        searched.AddRange(entities.Where(entity => entity.GetType().GetProperty(val.Value.Item1).GetValue(entity, null).ToString().ToLower().Contains(search)));
                    else
                        searched.AddRange(entities.Where(entity => entity.GetType().GetProperty(val.Value.Item1).GetValue(entity, null).ToString().ToLower() == search));
                }
            }

            return searched;
        }

        protected string GetExceptionMessage(int number)
        {
            var genericClassName = typeof(T).Name.Replace("ViewModel", "");
            switch (number)
            {
                case 2292:
                    return  $"{genericClassName} is used in diffrent table.";
                case 20001:
                    return "Appointment for this patient for this date and time is already booked.";
                case 20002:
                    return "There are not enough offices or veterinarians to attend to the patient.";
                default:
                    return $"Operation did not complete successfully.";
            }
        }
    }
}
