using TeleDoc.API.Area.Doctors.Models;
using TeleDoc.API.Area.Patients.Models;
using TeleDoc.API.Dtos.PatientsDto;

namespace TeleDoc.API.Services;

public interface IPatientRepository
{
    Task<List<PatientDetailsDto>?> GetPatientListAsync();
    Task<PatientDetailsDto> GetPatientByEmail(string email);
    Task<Patient> UpdatePatientByEmail(Patient patient);
    Task<List<Schedule>?> GetAppoinment(string email);
    Task UpdateImageUrl(string uId, string url);

    Task Save();
}