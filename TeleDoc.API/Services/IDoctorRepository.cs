using TeleDoc.API.Area.Doctors.Models;
using TeleDoc.API.Dtos.DoctorsDto;

namespace TeleDoc.API.Services;

public interface IDoctorRepository
{
    Task<List<DoctorDetailsDto>?> GetDoctorListAsync();
    Task<DoctorDetailsDto> GetDoctorByEmail(string email);
    Task<List<DoctorDetailsDto>?> GetDoctorByName(string name);
    Task<Doctor> GetDoctorBySpeciality(string speciality);
    Task<Doctor> UpdateDoctorByEmail(Doctor doctor);
    Task<DoctorDetailsDto> ApplyForCertified(string id);
    Task<DoctorDetailsDto> AddDoctorSchedule(string id, Schedule schedule);
    Task<List<Schedule>> GetScheduleAsync(string docId);
    Task<Schedule> AddBooking(string pEmail, string email, int dayOfWeek);
}