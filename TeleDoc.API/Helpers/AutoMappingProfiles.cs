using AutoMapper;
using TeleDoc.API.Area.Doctors.Models;
using TeleDoc.API.Area.Patients.Models;
using TeleDoc.API.Dtos.DoctorsDto;
using TeleDoc.API.Dtos.PatientsDto;
using TeleDoc.DAL.Entities;

namespace TeleDoc.API.Helpers;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<Patient, ApplicationUser>().ReverseMap();
        CreateMap<Patient, PatientDetailsDto>();
        
        CreateMap<Doctor, ApplicationUser>().ReverseMap();
        CreateMap<Doctor, DoctorDetailsDto>();
        
        
    }
    
}