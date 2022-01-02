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

            _restrictedInDropdown = new List<string> { "patientid" };
            AddPropertiesNamesToDropdown();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string option, string search)
        {
            ViewBag.Options = _options;

            var dbPatients = await _patientDataAccess.Get();
            List<PatientViewModel> patients = new List<PatientViewModel>();
            List<Species> species = (List<Species>)await _speciesDataAccess.Get();
            List<Organization> organizations = (List<Organization>)await _organizationDataAccess.Get();
            List<Owner> owners = (List<Owner>)await _ownerDataAccess.Get();

            IDictionary<int, Species> speciesDic = species.ToDictionary(p => p.SpeciesId);
            IDictionary<int, Organization> orgzanizationsDic = organizations.ToDictionary(p => p.OrganizationId);
            IDictionary<int, Owner> ownersDic = owners.ToDictionary(p => p.OwnerId);

            foreach (var dbPatinet in dbPatients)
            {
                patients.Add(_mapper.Map<PatientViewModel>(dbPatinet));
                patients.Last().SpeciesName = speciesDic[patients.Last().Species].Name;

                patients.Last().OrganizationNIP = null;
                if (patients.Last().Organization != null)
                {
                    int id = (int)patients.Last().Organization;
                    patients.Last().OrganizationNIP = orgzanizationsDic[id].NIP;
                }

                patients.Last().OwnerPESEL = null;
                if(patients.Last().Owner != null)
                {
                    int id = (int) patients.Last().Owner;
                    patients.Last().OwnerPESEL = ownersDic[id].PESEL;
                }

            }

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                search = search.ToLower().Trim();
                option = option.ToLower();
                var searched = new List<PatientViewModel>();

                foreach (var val in _propertiesNames)
                {
                    if (option == val.Key.ToLower() || option == "any")
                    {
                        if (val.Value.Item2 == typeof(string))
                            searched.AddRange(patients.Where(entity => entity.GetType().GetProperty(val.Value.Item1).GetValue(entity, null).ToString().ToLower().Contains(search)));
                        else
                            searched.AddRange(patients.Where(entity => entity.GetType().GetProperty(val.Value.Item1).GetValue(entity, null).ToString().ToLower() == search));
                    }
                }

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
                    ModelState.AddModelError("Custom error", $"Patient {GetExceptionMessage(ex.Number)}");
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
                    ModelState.AddModelError("Custom error", $"Patient {GetExceptionMessage(ex.Number)}");
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
                ViewBag.ErrorMessage = $"Patient {GetExceptionMessage(ex.Number)}";
                return View(GetFullPatient(id));
            }

            return RedirectToAction("Index");
        }

        public async Task<PatientViewModel> GetFullPatient(int id)
        {
            var dbPatient = await _patientDataAccess.Get(id);
            PatientViewModel patient = _mapper.Map<PatientViewModel>(dbPatient);

            Species species = await _speciesDataAccess.Get(patient.Species);
            patient.SpeciesName = species.Name;

            patient.OrganizationNIP = null;
            if(patient.Organization != null)
            {
                int organizationId = (int)patient.Organization;
                Organization organization = await _organizationDataAccess.Get(organizationId);
                patient.OrganizationNIP = organization.NIP;
            }

            patient.OwnerPESEL = null;
            if (patient.Owner != null)
            {
                int ownerId = (int)patient.Owner;
                Owner owner = await _ownerDataAccess.Get(ownerId);
                patient.OwnerPESEL = owner.PESEL;
            }

            return patient;
        }

        private async Task UpdateDropdownLists()
        {
            List<Species> species = (List<Species>)await _speciesDataAccess.Get();
            List<Organization> organizations = (List<Organization>)await _organizationDataAccess.Get();
            List<Owner> owners = (List<Owner>)await _ownerDataAccess.Get();

            ViewBag.species = new SelectList(species, "SpeciesId", "Name");
            ViewBag.organizations = new SelectList(organizations, "OrganizationId", "NIP");
            ViewBag.owners = new SelectList(owners, "OwnerId", "PESEL");
        }
    }
}
