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
        }

        [HttpGet]
        public async Task<IActionResult> Index(int appointmentId)
        {
            await UpdateDropdownLists(appointmentId);
            var dbPrescriptions = await _prescriptionDataAccess.Get(appointmentId);
            List<PrescriptionViewModel> prescriptions = new List<PrescriptionViewModel>();

            foreach (var p in dbPrescriptions)
            {
                var prescription = _mapper.Map<PrescriptionViewModel>(p);
                var drug = await _drugDataAccess.Get(prescription.DrugId);
                prescription.DrugName = drug.Name;
                prescriptions.Add(prescription);
            }
            ViewData["prescriptions"] = prescriptions;
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
                    ModelState.AddModelError("Custom error", $"Drug {GetExceptionMessage(ex.Number)}");
                    return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
                }

            }
            return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
        }

        public async Task<IActionResult> Delete(int appointmentId, int drugId)
        {
            try
            {
                await _prescriptionDataAccess.Delete(appointmentId, drugId);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                //add db error page
            }
            return RedirectToAction("Index", new { appointmentId = appointmentId });
        }


        private async Task UpdateDropdownLists(int appointmentId)
        {
            var drugs = (List<Drug>)await _drugDataAccess.Get();
            var dbPrescriptions = (List<Prescription>)await _prescriptionDataAccess.Get(appointmentId);
           
            List<Drug> uniqueDrugs = new List<Drug>();

            foreach (Drug drug in drugs)
            {
                if(!dbPrescriptions.Exists(p => p.DrugId == drug.DrugId))
                {
                    uniqueDrugs.Add(drug);
                }
            }

            ViewBag.drugs = new SelectList(uniqueDrugs, "DrugId", "Name");
        }
    }
}