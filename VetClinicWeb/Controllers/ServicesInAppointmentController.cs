using AutoMapper;
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
    public class ServicesInAppointmentController : BaseController<ServicesInAppointmentViewModel>
    {
        private readonly IDataAccess<Service> _serviceDataAccess;
        private readonly IDataAccess<ServicesInAppointment> _servicesInAppointmentDataAccess;

        public ServicesInAppointmentController(IDataAccess<ServicesInAppointment> servicesInAppointmentDataAccess,
           IDataAccess<Service> serviceDataAccess,
            IMapper mapper) : base(mapper)
        {
            _servicesInAppointmentDataAccess = servicesInAppointmentDataAccess;
            _serviceDataAccess = serviceDataAccess;
            _restrictedInDropdown = new List<string> { "servicesinappointmentid", "appointmentid", "service" };
            AddPropertiesNamesToDropdown();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int appointmentId, string option, string search)
        {
            ViewBag.Options = _options;
            ViewBag.AppId = appointmentId;
            await UpdateDropdownLists(appointmentId);
            var dbServicesInApp = await _servicesInAppointmentDataAccess.Get();
            dbServicesInApp = dbServicesInApp.Where(servInApp => servInApp.AppointmentId == appointmentId);
            var servicesInAppointments = new List<ServicesInAppointmentViewModel>();

            foreach (var p in dbServicesInApp)
            {
                var servicesInAppointment = _mapper.Map<ServicesInAppointmentViewModel>(p);
                var service = await _serviceDataAccess.Get(servicesInAppointment.Service);
                servicesInAppointment.ServiceName = service.Name;
                servicesInAppointments.Add(servicesInAppointment);
            }

            List<ServicesInAppointmentViewModel> searched;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                searched = Search(search, option, servicesInAppointments, "ServicesInAppointmentId");
            }
            else
            {
                searched = servicesInAppointments;
            }

            ViewData["servicesInAppointments"] = searched;
            var viewModel = new ServicesInAppointmentViewModel();
            viewModel.AppointmentId = appointmentId;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServicesInAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _servicesInAppointmentDataAccess.Insert(_mapper.Map<ServicesInAppointment>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    TempData["ErrorDb"] = GetExceptionMessage(ex.Number);
                    return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
                }
            }

            TempData["Error"] = ModelState.Values
                .SelectMany(v => v.Errors)
                .First().ErrorMessage;

            if (TempData["Error"].ToString() == "The value '' is invalid.")
            {
                TempData["ErrorDropdown"] = "You need to choose a drug.";
                TempData["Error"] = "";
            }

            return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var servicesInAppointment = await _servicesInAppointmentDataAccess.Get(id);
            await UpdateDropdownLists(servicesInAppointment.AppointmentId, servicesInAppointment.Service);
            var view = _mapper.Map<ServicesInAppointmentViewModel>(servicesInAppointment);
            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ServicesInAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _servicesInAppointmentDataAccess.Update(_mapper.Map<ServicesInAppointment>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    await UpdateDropdownLists(model.AppointmentId);
                    ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                    return View(model);
                }
            }

            TempData["Error"] = ModelState.Values
                .SelectMany(v => v.Errors)
                .First().ErrorMessage;

            if (TempData["Error"].ToString() == "The value '' is invalid.")
            {
                TempData["ErrorDropdown"] = "You need to choose a drug.";
                TempData["Error"] = "";
            }

            await UpdateDropdownLists(model.ServicesInAppointmentId, model.Service);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id, int appointmentId)
        {
            try
            {
                await _servicesInAppointmentDataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                //
            }

            return RedirectToAction("Index", new { appointmentId = appointmentId });
        }


        private async Task UpdateDropdownLists(int appointmentId, int excludeService = -1)
        {
            var services = (List<Service>)await _serviceDataAccess.Get();
            var dbServicesInAppAll = (List<ServicesInAppointment>)await _servicesInAppointmentDataAccess.Get();
            var dbServicesInApp = dbServicesInAppAll.Where(servInApp => servInApp.AppointmentId == appointmentId).ToList();
            var uniqueServices = new List<Service>();

            foreach (var service in services)
            {
                if (!dbServicesInApp.Exists(x => x.Service == service.ServiceId))
                {
                    uniqueServices.Add(service);
                }
                else if (excludeService != -1 && excludeService == service.ServiceId)
                {
                    uniqueServices.Add(service);
                }
            }

            ViewBag.services = new SelectList(uniqueServices, "ServiceId", "Name");
        }
    }
}
