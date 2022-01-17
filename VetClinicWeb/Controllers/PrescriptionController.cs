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
    public class PrescriptionController : BaseController<PrescriptionViewModel>
    {
        private readonly IDataAccess<Drug> _drugDataAccess;
        private readonly IPrescriptionDataAccess _prescriptionDataAccess;

        public PrescriptionController(IPrescriptionDataAccess prescriptionDataAccess,
            IDataAccess<Drug> drugDataAccess,
           IMapper mapper) : base(mapper)
        {
            _drugDataAccess = drugDataAccess;
            _prescriptionDataAccess = prescriptionDataAccess;
            _restrictedInDropdown = new List<string> { "drugid", "appointmentid", "prescriptionid" };
            AddPropertiesNamesToDropdown();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int appointmentId, string option, string search)
        {
            ViewBag.Options = _options;
            ViewBag.AppId = appointmentId;
            await UpdateDropdownLists(appointmentId);
            var dbPrescriptions = await _prescriptionDataAccess.Get(appointmentId);
            var prescriptions = new List<PrescriptionViewModel>();

            foreach (var p in dbPrescriptions)
            {
                var prescription = _mapper.Map<PrescriptionViewModel>(p);
                var drug = await _drugDataAccess.Get(prescription.DrugId);
                prescription.DrugName = drug.Name;
                prescriptions.Add(prescription);
            }

            List<PrescriptionViewModel> searched;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                searched = Search(search, option, prescriptions, "PrescriptionId");
            }
            else
            {
                searched = prescriptions;
            }

            ViewData["prescriptions"] = searched;
            var viewModel = new PrescriptionViewModel();
            viewModel.AppointmentId = appointmentId;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _prescriptionDataAccess.Insert(_mapper.Map<Prescription>(model));
                    ModelState.Clear();
                    return RedirectToAction("Index", new { appointmentId=model.AppointmentId});
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

            if(TempData["Error"].ToString() == "The value '' is invalid.")
            {
                TempData["ErrorDropdown"] = "You need to choose a drug.";
                TempData["Error"] = "";
            }

            return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int appointmentId, int drugId)
        {
            var prescription = await _prescriptionDataAccess.Get(appointmentId, drugId);
            await UpdateDropdownLists(appointmentId, prescription.DrugId);
            var view = _mapper.Map<PrescriptionViewModel>(prescription);
            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PrescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _prescriptionDataAccess.Update(_mapper.Map<Prescription>(model));
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

            await UpdateDropdownLists(model.PrescriptionId, model.DrugId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int appointmentId, int drugId)
        {
            try
            {
                await _prescriptionDataAccess.Delete(appointmentId, drugId);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                //
            }

            return RedirectToAction("Index", new { appointmentId = appointmentId });
        }


        private async Task UpdateDropdownLists(int appointmentId, int excludeDrug = -1)
        {
            var drugs = (List<Drug>)await _drugDataAccess.Get();
            var dbPrescriptions = (List<Prescription>)await _prescriptionDataAccess.Get(appointmentId);
            var uniqueDrugs = new List<Drug>();

            foreach (Drug drug in drugs)
            {
                if(!dbPrescriptions.Exists(p => p.DrugId == drug.DrugId))
                {
                    uniqueDrugs.Add(drug);
                }
                else if(excludeDrug != -1 && excludeDrug == drug.DrugId)
                {
                    uniqueDrugs.Add(drug);
                }
            }

            ViewBag.drugs = new SelectList(uniqueDrugs, "DrugId", "Name");
        }
    }
}
