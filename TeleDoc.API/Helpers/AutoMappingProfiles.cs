using AutoMapper;
using TeleDoc.API.Area.Patients.Models;
using TeleDoc.API.Dtos.Patients;
using TeleDoc.DAL.Entities;

namespace TeleDoc.API.Helpers;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<Patient, ApplicationUser>().ReverseMap();
        CreateMap<Patient, PatientDetailsDto>();
    }
    
}