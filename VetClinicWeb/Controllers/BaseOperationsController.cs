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
    public class BaseOperationsController<T, U> : BaseController
    {
        protected IDictionary<string, string> _propertiesNames;
        protected readonly IDataAccess<T> _dataAccess;
        protected List<SelectListItem> _options;

        public BaseOperationsController(IMapper mapper, IDataAccess<T> dataAccess) : base(mapper)
        {
            _propertiesNames = new Dictionary<string, string>();
            _dataAccess = dataAccess;

            var listOfFieldNames = typeof(U).GetProperties().Select(f => f.Name).ToList();
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

        [HttpGet]
        public async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;
            var dbEntities = await _dataAccess.Get();
            var entities = new List<U>();

            foreach (var dbEntity in dbEntities)
                entities.Add(_mapper.Map<U>(dbEntity));

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                search = search.ToLower();
                option = option.ToLower();
                var searched = new List<U>();

                foreach (var val in _propertiesNames)
                {
                    if (option == val.Key.ToLower() || option == "any")
                        searched.AddRange(entities.Where(entity => entity.GetType().GetProperty(val.Value).GetValue(entity, null).ToString().ToLower().Contains(search)));
                }

                return View(searched);
            }

            return View(entities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(U model)
        {
            if (ModelState.IsValid)
            {
                await _dataAccess.Insert(_mapper.Map<T>(model));
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var entity = await _dataAccess.Get(id);
            return View(_mapper.Map<U>(entity));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, U model)
        {
            if (ModelState.IsValid)
            {
                await _dataAccess.Update(_mapper.Map<T>(model));
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ShowDelete(int id)
        {
            return RedirectToAction("Delete", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, U model)
        {
            try
            {
                await _dataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = $"{typeof(T).Name} {GetExceptionMessage(ex.Number)}";
                var entity = await _dataAccess.Get(id);
                return View(_mapper.Map<U>(entity));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _dataAccess.Get(id);
            return View(_mapper.Map<U>(entity));
        }
    }
}
