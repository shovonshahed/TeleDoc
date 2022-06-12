using TeleDoc.API.Area.Patients.Models;
using TeleDoc.API.Dtos.PatientsDto;

namespace TeleDoc.API.Services;

public interface IPatientRepository
{
    Task<List<PatientDetailsDto>?> GetPatientListAsync();
    Task<Patient> GetPatientByEmail(string email);
    Task<Patient> UpdatePatientByEmail(Patient patient);

}