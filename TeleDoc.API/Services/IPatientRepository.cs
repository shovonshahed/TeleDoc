using TeleDoc.API.Area.Patients.Models;

namespace TeleDoc.API.Services;

public interface IPatientRepository
{
    Task<List<Patient>?> GetPatientListAsync();
    Task<Patient> GetPatientByEmail(string email);
    Task<Patient> UpdatePatientByEmail(Patient patient);

}