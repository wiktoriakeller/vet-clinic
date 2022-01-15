using Microsoft.AspNetCore.Mvc;
using DataAccess.Access;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;

namespace VetClinicWeb.Controllers
{
    public class BaseOperationsController<T, U> : BaseController<U>
    {
        protected readonly IDataAccess<T> _dataAccess;

        public BaseOperationsController(IMapper mapper, IDataAccess<T> dataAccess) : base(mapper)
        {
            _dataAccess = dataAccess;
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
                search = search.ToLower().Trim();
                option = option.ToLower();
                var searched = new List<U>();
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
                try
                {
                    await _dataAccess.Update(_mapper.Map<T>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch(Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                    return View(model);
                }
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
                ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
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
