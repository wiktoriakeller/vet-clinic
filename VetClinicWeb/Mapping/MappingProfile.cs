﻿using AutoMapper;
using DataAccess.Models;
using VetClinicWeb.Models;

namespace VetClinic.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
            CreateMap<Facility, FacilityViewModel>().ReverseMap();
            CreateMap<Office, OfficeViewModel>().ReverseMap();
            CreateMap<Position, PositionViewModel>().ReverseMap();
            CreateMap<Species, SpeciesViewModel>().ReverseMap();
            CreateMap<Organization, OrganizationViewModel>().ReverseMap();
            CreateMap<Owner, OwnerViewModel>().ReverseMap();
            CreateMap<Office, OfficeViewModel>().ReverseMap();
            CreateMap<Patient, PatientViewModel>().ReverseMap();

        }
    }
}   
