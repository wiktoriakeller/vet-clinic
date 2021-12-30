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

        public BaseOperationsController(IMapper mapper, IDataAccess<T> dataAccess) : base(mapper)
        {
            _propertiesNames = new Dictionary<string, string>();
            _dataAccess = dataAccess;
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
