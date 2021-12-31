using AutoMapper;
using DataAccess.Access;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinicWeb.Models;

namespace VetClinicWeb.Controllers
{
    public class EmployeeController : BaseController<EmployeeViewModel>
    {
        private readonly IDataAccess<Employee> _employeeDataAccess;
        private readonly IDataAccess<Position> _positionDataAccess;
        private readonly IDataAccess<Facility> _facilityDataAccess;

        public EmployeeController(IDataAccess<Employee> employeeDataAccess,
            IDataAccess<Position> positionDataAccess,
            IDataAccess<Facility> facilityDataAcces,
            IMapper mapper) : base(mapper)
        {
            _employeeDataAccess = employeeDataAccess;
            _positionDataAccess = positionDataAccess;
            _facilityDataAccess = facilityDataAcces;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;

            var dbEmployees = await _employeeDataAccess.Get();
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            List<Position> positions = (List<Position>)await _positionDataAccess.Get();
            List<Facility> facilities= (List<Facility>)await _facilityDataAccess.Get();

            IDictionary<int, Position> positionsDic = positions.ToDictionary(p => p.PositionId);
            IDictionary<int, Facility> facilitiesDic = facilities.ToDictionary(p => p.FacilityId);

            foreach (var dbEmployee in dbEmployees)
            {
                employees.Add(_mapper.Map<EmployeeViewModel>(dbEmployee));
                employees.Last().PositionName = positionsDic[employees.Last().Position].Name;
                employees.Last().FacilityAddress = facilitiesDic[employees.Last().Facility].Address;
            }

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                search = search.ToLower().Trim();
                option = option.ToLower();
                var searched = new List<EmployeeViewModel>();

                foreach (var val in _propertiesNames)
                {
                    if (option == val.Key.ToLower() || option == "any")
                    {
                        if (val.Value.Item2 == typeof(string))
                            searched.AddRange(employees.Where(entity => entity.GetType().GetProperty(val.Value.Item1).GetValue(entity, null).ToString().ToLower().Contains(search)));
                        else
                            searched.AddRange(employees.Where(entity => entity.GetType().GetProperty(val.Value.Item1).GetValue(entity, null).ToString().ToLower() == search));
                    }
                }

                return View(searched);
            }

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Position> positions = (List<Position>)await _positionDataAccess.Get();
            List<Facility> facilities = (List<Facility>)await _facilityDataAccess.Get();
            ViewBag.positions = new SelectList(positions, "PositionId", "Name");
            ViewBag.facilities = new SelectList(facilities, "FacilityId", "Address");
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var dbEmployee = await _employeeDataAccess.Get(id);
            EmployeeViewModel employee = _mapper.Map<EmployeeViewModel>(dbEmployee);
            Position position = await _positionDataAccess.Get(employee.Position);
            employee.PositionName = position.Name;
            Facility facility = await _facilityDataAccess.Get(employee.Facility);
            employee.FacilityAddress= facility.Address;
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeDataAccess.Insert(_mapper.Map<Employee>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var employee = await GetFullEmployee(id);

            List<Position> positions = (List<Position>)await _positionDataAccess.Get();
            List<Facility> facilities = (List<Facility>)await _facilityDataAccess.Get();
            ViewBag.positions = new SelectList(positions, "PositionId", "Name");
            ViewBag.facilities = new SelectList(facilities, "FacilityId", "Address");

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeDataAccess.Update(_mapper.Map<Employee>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                } 
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ShowDelete(int id)
        {
            return RedirectToAction("Delete", new { id = id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await GetFullEmployee(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, EmployeeViewModel collection)
        {
            try
            {
                await _employeeDataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = $"Employee {GetExceptionMessage(ex.Number)}";
                return View(GetFullEmployee(id));
            }

            return RedirectToAction("Index");
        }

        public async Task<EmployeeViewModel> GetFullEmployee(int id)
        {
            var dbEmployee = await _employeeDataAccess.Get(id);
            EmployeeViewModel employee = _mapper.Map<EmployeeViewModel>(dbEmployee);
            
            Position position = await _positionDataAccess.Get(employee.Position);
            employee.PositionName = position.Name;
            
            Facility facility = await _facilityDataAccess.Get(employee.Facility);
            employee.FacilityAddress = facility.Address;
            
            return employee;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsSalaryInRangeAsync(EmployeeViewModel model)
        {
            Position position = await _positionDataAccess.Get(model.Position);

            if (model.Salary > position.SalaryMax)
                return Json($"Salary is greater than the maximum salary for this position: {position.SalaryMax}");
            else if (model.Salary < position.SalaryMin)
                return Json($"Salary is less than the minimum salary for this position: {position.SalaryMin}");
            else
                return Json(true);
        }
    }
}
