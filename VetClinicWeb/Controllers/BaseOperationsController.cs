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
        public virtual async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;
            var dbEntities = await _dataAccess.Get();
            var entities = new List<U>();

            foreach (var dbEntity in dbEntities)
                entities.Add(_mapper.Map<U>(dbEntity));

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                var searched = Search(search, option, entities);
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
                try
                {
                    await _dataAccess.Insert(_mapper.Map<T>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                    return View(model);
                }
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
        public virtual async Task<IActionResult> Delete(int id, U model)
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
