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
    public class PatientController : BaseController<PatientViewModel>
    {
        private readonly IDataAccess<Patient> _patientDataAccess;
        private readonly IDataAccess<Species> _speciesDataAccess;
        private readonly IDataAccess<Organization> _organizationDataAccess;
        private readonly IDataAccess<Owner> _ownerDataAccess;

        public PatientController(IDataAccess<Patient> patientDataAccess,
            IDataAccess<Species> speciesDataAccess,
            IDataAccess<Organization> organizationDataAccess,
            IDataAccess <Owner> ownerDataAccess,
            IMapper mapper) : base(mapper)
        {
            _patientDataAccess = patientDataAccess;
            _speciesDataAccess = speciesDataAccess;
            _organizationDataAccess = organizationDataAccess;
            _ownerDataAccess = ownerDataAccess;

            _restrictedInDropdown = new List<string> { "patientid", "owner", "organization", "species" };
            AddPropertiesNamesToDropdown();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;

            var dbPatients = await _patientDataAccess.Get();
            var patients = new List<PatientViewModel>();
            var species = (List<Species>)await _speciesDataAccess.Get();
            var organizations = (List<Organization>)await _organizationDataAccess.Get();
            var owners = (List<Owner>)await _ownerDataAccess.Get();

            IDictionary<int, Species> speciesDic = species.ToDictionary(p => p.SpeciesId);
            IDictionary<int, Organization> orgzanizationsDic = organizations.ToDictionary(p => p.OrganizationId);
            IDictionary<int, Owner> ownersDic = owners.ToDictionary(p => p.OwnerId);

            foreach (var dbPatinet in dbPatients)
            {
                patients.Add(_mapper.Map<PatientViewModel>(dbPatinet));
                patients.Last().SpeciesName = speciesDic[patients.Last().Species].Name;

                patients.Last().OrganizationName = null;
                if (patients.Last().Organization != null)
                {
                    int id = (int)patients.Last().Organization;
                    patients.Last().OrganizationName = orgzanizationsDic[id].NameNIP;
                }

                patients.Last().OwnerName = null;
                if(patients.Last().Owner != null)
                {
                    int id = (int) patients.Last().Owner;
                    patients.Last().OwnerName = ownersDic[id].NameSurnamePESEL;
                }
            }

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                var searched = Search(search, option, patients);
                return View(searched);
            }

            return View(patients);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await UpdateDropdownLists();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _patientDataAccess.Insert(_mapper.Map<Patient>(model));
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
            var patient = await GetFullPatient(id);
            await UpdateDropdownLists();
            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _patientDataAccess.Update(_mapper.Map<Patient>(model));
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
            return View(await GetFullPatient(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, PatientViewModel collection)
        {
            try
            {
                await _patientDataAccess.Delete(id);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                ViewBag.ErrorMessage = GetExceptionMessage(ex.Number);
                return View(await GetFullPatient(id));
            }

            return RedirectToAction("Index");
        }

        public async Task<PatientViewModel> GetFullPatient(int id)
        {
            var dbPatient = await _patientDataAccess.Get(id);
            var patient = _mapper.Map<PatientViewModel>(dbPatient);

            var species = await _speciesDataAccess.Get(patient.Species);
            patient.SpeciesName = species.Name;

            patient.OrganizationName = null;
            if(patient.Organization != null)
            {
                int organizationId = (int)patient.Organization;
                var organization = await _organizationDataAccess.Get(organizationId);
                patient.OrganizationName = organization.NameNIP;
            }

            patient.OwnerName = null;
            if (patient.Owner != null)
            {
                int ownerId = (int)patient.Owner;
                var owner = await _ownerDataAccess.Get(ownerId);
                patient.OwnerName = owner.NameSurnamePESEL;
            }

            return patient;
        }

        private async Task UpdateDropdownLists()
        {
            var species = (List<Species>)await _speciesDataAccess.Get();
            var organizations = (List<Organization>)await _organizationDataAccess.Get();
            var owners = (List<Owner>)await _ownerDataAccess.Get();

            ViewBag.species = new SelectList(species, "SpeciesId", "Name");
            ViewBag.organizations = new SelectList(organizations, "OrganizationId", "NameNIP");
            ViewBag.owners = new SelectList(owners, "OwnerId", "NameSurnamePESEL");
        }
    }
}
