using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
                    string fieldName;
                    MemberInfo property = typeof(T).GetProperty(field.Name);
                    var displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;

                    if (displayName != null)
                        fieldName = displayName;
                    else
                        fieldName = field.Name;

                    _options.Add(new SelectListItem { Text = fieldName });
                    _propertiesNames[fieldName] = Tuple.Create(field.Name, field.PropertyType);
                }
            }

            _options.Add(new SelectListItem { Text = "Any" });
        }

        protected List<T> Search(string search, string option, List<T> entities)
        {
            search = search.ToLower().Trim();
            option = option.ToLower();
            var searched = new List<T>();
            string idPropertyName = "";

            if(entities.Count > 0)
            {
                var listOfFields = typeof(T).GetProperties();
                idPropertyName = listOfFields.SingleOrDefault(field => field.Name.ToLower().Contains("id")).Name;
            }

            if(idPropertyName != "")
            {
                foreach(var entity in entities)
                {
                    foreach(var val in _propertiesNames)
                    {
                        if(option == val.Key.ToLower() || option == "any")
                        {
                            var propertyVal = entity.GetType().GetProperty(val.Value.Item1).GetValue(entity, null).ToString().ToLower();
                            var contains = searched.Any(s => s.GetType().GetProperty(idPropertyName).GetValue(s, null).ToString() == entity.GetType().GetProperty(idPropertyName).GetValue(entity, null).ToString());

                            if ((val.Value.Item2 == typeof(string) && propertyVal.Contains(search)) 
                                || (val.Value.Item2 != typeof(string) && propertyVal == search) 
                                && !contains)
                            {
                                searched.Add(entity);
                                break;
                            }
                        }
                    }
                }
            }

            return searched;
        }

        protected string GetExceptionMessage(int number)
        {
            var genericClassName = typeof(T).Name.Replace("ViewModel", "");
            switch (number)
            {
                case 2291:
                    return "Operation did not complete successfully due to the empty values in forms.";
                case 2292:
                    return  $"{genericClassName} is used in diffrent table.";
                case 20001:
                    return "This patient has another appointment at the same time.";
                case 20002:
                    return "There are not enough offices or veterinarians to attend to the patient.";
                default:
                    return $"Operation did not complete successfully.";
            }
        }
    }
}
