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
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeDataAccess _employeeDataAccess;
        private readonly IPositionDataAccess _positionDataAccess;
        private readonly IFacilityDataAccess _facilityDataAccess;

        public EmployeeController(IEmployeeDataAccess employeeDataAccess,
            IPositionDataAccess positionDataAccess,
            IFacilityDataAccess facilityDataAcces,
            IMapper mapper) : base(mapper)
        {
            _employeeDataAccess = employeeDataAccess;
            _positionDataAccess = positionDataAccess;
            _facilityDataAccess = facilityDataAcces;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dbEmployees = await _employeeDataAccess.GetEmployees();
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            List<Position> positions = (List<Position>)await _positionDataAccess.GetPositions();
            List<Facility> facilities= (List<Facility>)await _facilityDataAccess.GetFacilities();


            IDictionary<int, Position> positionsDic = positions.ToDictionary(p => p.PositionId);
            IDictionary<int, Facility> facilitiesDic = facilities.ToDictionary(p => p.FacilityId);

            foreach (var dbEmployee in dbEmployees)
            {
                employees.Add(_mapper.Map<EmployeeViewModel>(dbEmployee));
                employees.Last().PositionName = positionsDic[employees.Last().Position].Name;
                employees.Last().FacilityAddress = facilitiesDic[employees.Last().Facility].Address;
            }

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Position> positions = (List<Position>)await _positionDataAccess.GetPositions();
            List<Facility> facilities = (List<Facility>)await _facilityDataAccess.GetFacilities();
            ViewBag.positions = new SelectList(positions, "PositionId", "Name");
            ViewBag.facilities = new SelectList(facilities, "FacilityId", "Address");
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var dbEmployee = await _employeeDataAccess.GetEmployee(id);
            EmployeeViewModel employee = _mapper.Map<EmployeeViewModel>(dbEmployee);
            Position position = await _positionDataAccess.GetPosition(employee.Position);
            employee.PositionName = position.Name;
            Facility facility = await _facilityDataAccess.GetFacility(employee.Facility);
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
                    await _employeeDataAccess.InsertEmployee(_mapper.Map<Employee>(model));
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

            List<Position> positions = (List<Position>)await _positionDataAccess.GetPositions();
            List<Facility> facilities = (List<Facility>)await _facilityDataAccess.GetFacilities();
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
                    await _employeeDataAccess.UpdateEmployee(_mapper.Map<Employee>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch
                {
                    //return View();
                } 

            }
            return View(model);
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
                await _employeeDataAccess.DeleteEmployee(id);
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
            var dbEmployee = await _employeeDataAccess.GetEmployee(id);
            EmployeeViewModel employee = _mapper.Map<EmployeeViewModel>(dbEmployee);
            Position position = await _positionDataAccess.GetPosition(employee.Position);
            employee.PositionName = position.Name;
            Facility facility = await _facilityDataAccess.GetFacility(employee.Facility);
            employee.FacilityAddress = facility.Address;
            return employee;
        }
    }
}
