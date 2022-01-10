﻿using AutoMapper;
using DataAccess.Access;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinicWeb.Models;

namespace VetClinicWeb.Controllers
{
    public class AppointmentController : BaseController<AppointmentViewModel>
    {
        private readonly IDataAccess<Appointment> _appointmentDataAccess;
        private readonly IDataAccess<Employee> _employeeDataAccess;
        private readonly IDataAccess<Patient> _patientDataAccess;
        private readonly IDataAccess<Office> _officeDataAccess;
        private readonly IDataAccess<Facility> _facilityDataAccess;

        public AppointmentController(IDataAccess<Appointment> appointmentDataAccess,
            IDataAccess<Employee> employeeDataAccess,
            IDataAccess<Patient> patientDataAccess,
            IDataAccess<Office> officeDataAccess,
            IDataAccess<Facility> facilityDataAccess,
            IMapper mapper) : base(mapper)
        {
            _appointmentDataAccess = appointmentDataAccess;
            _employeeDataAccess = employeeDataAccess;
            _patientDataAccess = patientDataAccess;
            _officeDataAccess = officeDataAccess;
            _facilityDataAccess = facilityDataAccess;

            _restrictedInDropdown = new List<string> { "appointmentid", "date", "time", "cause", "employee", "office", "facility", "patient", "officenumber" };
            AddPropertiesNamesToDropdown();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;

            var dbAppointments = await _appointmentDataAccess.Get();
            var appointments = new List<AppointmentViewModel>();
            var employees = (List<Employee>)await _employeeDataAccess.Get();
            var patients = (List<Patient>)await _patientDataAccess.Get();
            var offices = (List<Office>)await _officeDataAccess.Get();
            var facilities = (List<Facility>)await _facilityDataAccess.Get();

            var employeesDic = employees.ToDictionary(p => p.EmployeeId);
            var patientsDic = patients.ToDictionary(p => p.PatientId);
            var officesDic = offices.ToDictionary(p => p.OfficeId);
            var facilitiesDic = facilities.ToDictionary(p => p.FacilityId);

            foreach (var dbAppointment in dbAppointments)
            {
                appointments.Add(_mapper.Map<AppointmentViewModel>(dbAppointment));
                appointments.Last().EmployeeName = $"{employeesDic[appointments.Last().Employee].Name} {employeesDic[appointments.Last().Employee].Surname}";
                appointments.Last().OfficeNumber = officesDic[appointments.Last().Office].OfficeNumber;
                appointments.Last().PatientName = patientsDic[appointments.Last().Patient].Name;
                appointments.Last().FacilityAddress = facilitiesDic[officesDic[appointments.Last().Office].Facility].Address;
            }

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                var searched = Search(search, option, appointments);
                return View(searched);
            }

            return View(appointments);
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await GetFullAppointment(id));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await UpdateDropdownLists();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dateAndTime = $"{model.Date} {model.Time}";
                model.AppointmentDate = dateAndTime;
                try
                {
                    var facility = await _facilityDataAccess.Get(model.Facility);
                    var _appointmentProcedureAccess = (IProcedureDataAccess<Appointment, Facility>)_appointmentDataAccess;
                    await _appointmentProcedureAccess.Insert(_mapper.Map<Appointment>(model), facility);
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    await UpdateDropdownLists();
                    ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                    return View(model);
                }
            }

            await UpdateDropdownLists();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var patient = await GetFullAppointment(id);
            await UpdateDropdownLists();
            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _appointmentDataAccess.Update(_mapper.Map<Appointment>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    await UpdateDropdownLists();
                    ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                    return View(model);
                }
            }

            await UpdateDropdownLists();
            return View(model);
        }

        [HttpGet]
        public IActionResult ShowDelete(int id)
        {
            return RedirectToAction("Delete", new { id = id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await GetFullAppointment(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, AppointmentViewModel collection)
        {
            try
            {
                await _appointmentDataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                return View(GetFullAppointment(id));
            }

            return RedirectToAction("Index");
        }

        public async Task<AppointmentViewModel> GetFullAppointment(int id)
        {
            var dbAppointment = await _appointmentDataAccess.Get(id);
            var appointment = _mapper.Map<AppointmentViewModel>(dbAppointment);
           
            var employee = await _employeeDataAccess.Get(appointment.Employee);
            appointment.EmployeeName = $"{employee.Name} {employee.Surname}";

            var patient = await _patientDataAccess.Get(appointment.Patient);
            appointment.PatientName = patient.Name;

            var office = await _officeDataAccess.Get(appointment.Office);
            appointment.OfficeNumber = office.OfficeNumber;

            var facility = await _facilityDataAccess.Get(office.Facility);
            appointment.FacilityAddress = facility.Address;

            return appointment;
        }

        private async Task UpdateDropdownLists()
        {
            var facilities = (List<Facility>) await _facilityDataAccess.Get();
            var patients = (List<Patient>) await _patientDataAccess.Get();

            ViewBag.facilities = new SelectList(facilities, "FacilityId", "Address");
            ViewBag.patients = new SelectList(patients, "PatientId", "Name");
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsTimeValid(string time)
        {
            int startHour = 7;
            int endHour = 18;
            var availableHours = new List<string>();
            for(int i = startHour; i <= endHour; i++)
            {
                if(i <= 9)
                {
                    availableHours.Add($"0{i}:00");
                    availableHours.Add($"0{i}:30");
                }
                else
                {
                    availableHours.Add($"{i}:00");
                    availableHours.Add($"{i}:30");
                }
            }

            if (!availableHours.Contains(time))
                return Json($"Time {time} is invalid.");
            else
                return Json(true);
        }
    }
}
