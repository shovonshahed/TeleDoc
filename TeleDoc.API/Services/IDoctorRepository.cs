using TeleDoc.API.Area.Doctors.Models;

namespace TeleDoc.API.Services;

public interface IDoctorRepository
{
    Task<List<Doctor>?> GetDoctorListAsync();
    Task<Doctor> GetDoctorByEmail(string email);
    Task<Doctor> GetDoctorByName(string name);
    Task<Doctor> GetDoctorBySpeciality(string speciality);
    Task<Doctor> UpdateDoctorByEmail(Doctor doctor);
}