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
            CreateMap<Specie, SpecieViewModel>().ReverseMap();
        }
    }
}   
