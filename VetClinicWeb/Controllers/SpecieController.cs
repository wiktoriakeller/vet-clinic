using AutoMapper;
using DataAccess.Access;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VetClinicWeb.Models;

namespace VetClinicWeb.Controllers
{
    public class SpecieController : BaseController
    {
        private readonly ISpecieDataAccess _specieDataAccess;
        public SpecieController(ISpecieDataAccess SpecieDataAccess, IMapper mapper) : base(mapper)
        {
            _specieDataAccess = SpecieDataAccess;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Specie> dbSpecies = await _specieDataAccess.Get();
            List<SpecieViewModel> species = new List<SpecieViewModel>();

            foreach (Specie dbSpecie in dbSpecies)
                species.Add(_mapper.Map<SpecieViewModel>(dbSpecie));

            return View(species);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpecieViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _specieDataAccess.Insert(_mapper.Map<Specie>(model));
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Specie specie = await _specieDataAccess.Get(id);
            return View(_mapper.Map<SpecieViewModel>(specie));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, SpecieViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _specieDataAccess.Update(_mapper.Map<Specie>(model));
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Specie specie = await _specieDataAccess.Get(id);
            return View(_mapper.Map<SpecieViewModel>(specie));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, SpecieViewModel model)
        {
            try
            {
                await _specieDataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = $"Specie {GetExceptionMessage(ex.Number)}";
                var specie = await _specieDataAccess.Get(id);
                return View(_mapper.Map<SpecieViewModel>(specie));
            }
            return RedirectToAction("Index");
        }
    }
}
