using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeleDoc.API.Area.Doctors.Models;
using TeleDoc.API.Context;
using TeleDoc.API.Dtos.DoctorsDto;
using TeleDoc.API.Models;
using TeleDoc.API.Static;
using DayOfWeek = TeleDoc.API.Area.Admins.Enums.DayOfWeek;

namespace TeleDoc.API.Services;

public class DoctorRepository : IDoctorRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public DoctorRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IMapper mapper, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<List<DoctorDetailsDto>?> GetDoctorListAsync()
    {
        var result = await _userManager.Users
            .Include(d => d.Schedules)!.
            ThenInclude(p => p.Patients)
            .Where(r => r.Role == UserRoles.Doctor)
            .ToListAsync();

        // var role = _roleManager.Roles.Where(r => r.Name == UserRoles.Doctor);
        
        var data = _mapper.Map<List<Doctor>>(result);
        var dataToReturn = _mapper.Map<List<DoctorDetailsDto>>(data);

        return dataToReturn;
    }

    public async Task<DoctorDetailsDto> GetDoctorByEmail(string email)
    {
        var result = await _userManager.Users
            .Include(m => m.MapLocation)
            .Include(d => d.Schedules)!
            .ThenInclude(p => p.Patients)
            .FirstOrDefaultAsync(d => d.Email == email && d.Role == UserRoles.Doctor);
        
        var data = _mapper.Map<Doctor>(result);
        var dataToReturn = _mapper.Map<DoctorDetailsDto>(data);

        return dataToReturn;
    }

    private async Task<ApplicationUser> GetDoctorById(string id)
    {
        var user = await _userManager.Users
            .Include(s => s.Schedules)!
            .ThenInclude(p => p.Patients)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        return user!;
    }

    public async Task<List<DoctorDetailsDto>?> GetDoctorByName(string name)
    {
        // var result = await _userManager.FindByNameAsync(name);
        var result = await _userManager.Users
            .Where(u => u.Role == UserRoles.Doctor && u.Name!.Contains(name))
            .Include(s => s.Schedules)!
            .ThenInclude(p => p.Patients)
            .ToListAsync();

        var data = _mapper.Map<List<Doctor>>(result);
        var dataToReturn = _mapper.Map<List<DoctorDetailsDto>>(data);
        
        return dataToReturn;
    }

    public Task<Doctor> GetDoctorBySpeciality(string speciality)
    {
        throw new NotImplementedException();
    }

    public async Task<DoctorDetailsDto> UpdateDoctorByEmail(Doctor doctor)
    {
        var doc = await _dbContext.Users
            .Include(m => m.MapLocation)
            .FirstOrDefaultAsync(d => d.Email == doctor.Email);
        // doc = _mapper.Map<Doctor>(doc);
        // doc = doctor;

        if (doc is not null)
        {
            // _dbContext.Update(doc);
            doc.Name = doctor.Name;
            // doc.Email = doctor.Email;
            doc.Gender = doctor.Gender;
            doc.DateOfBirth = doctor.DateOfBirth;
            doc.Address = doctor.Address;
            doc.PhoneNumber = doctor.PhoneNumber;
            doc.CertificateUrl = doctor.CertificateUrl;
            // doc.MapLocation = doctor.MapLocation;

            if (doctor.MapLocation is not null)
            {
                if (doc.MapLocation is null)
                {
                    doc.MapLocation = new MapLocation()
                    {
                        Latitude = doctor.MapLocation.Latitude,
                        Longitude = doctor.MapLocation.Longitude
                    };
                }
                else
                {
                    doc.MapLocation.Latitude = doctor.MapLocation.Latitude;
                    doc.MapLocation.Longitude = doctor.MapLocation.Longitude;
                }
                
            }
            
            doc.College = doctor.College;
            doc.Speciality = doctor.Speciality;
            doc.CertificateUrl = doctor.CertificateUrl;

            // doc = doctor;
        }
        
        await _dbContext.SaveChangesAsync();

        var data = _mapper.Map<Doctor>(doc);
        var dataToReturn = _mapper.Map<DoctorDetailsDto>(data);
        
        return dataToReturn;
    }

    public async Task<DoctorDetailsDto> ApplyForCertified(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        
        var data = _mapper.Map<Doctor>(user);
        var userToReturn = _mapper.Map<DoctorDetailsDto>(data);

        return userToReturn;
    }

    public async Task<DoctorDetailsDto> AddDoctorSchedule(string id, Schedule schedule)
    {
        var user = await GetDoctorById(id);
        
        var existsSchedule =  user.Schedules!
            .FirstOrDefault(d => d.DayOfWeek == schedule.DayOfWeek);

        if (existsSchedule is not null)
        {
            var dataT = _mapper.Map<Doctor>(user);
            var userToReturnT = _mapper.Map<DoctorDetailsDto>(dataT);

            return userToReturnT;
        }
        

        user.Schedules?.Add(schedule);
        await _dbContext.SaveChangesAsync();
        
        var data = _mapper.Map<Doctor>(user);
        var userToReturn = _mapper.Map<DoctorDetailsDto>(data);

        return userToReturn;
    }

    public async Task<List<Schedule>> GetScheduleAsync(string email)
    {
        var doctor = await _userManager.Users
            .Include(s => s.Schedules)!
            .ThenInclude(p => p.Patients)
            .FirstOrDefaultAsync(u => u.Email == email);

        var schedules = doctor!.Schedules!.ToList();
        
        return  schedules;
    }

    public async Task<Schedule> AddBooking(string pEmail, string email, int dayOfWeek)
    // public async Task<Schedule> AddBooking(string pEmail, int sId)
    {
        var doctor = await GetDoctorByEmail(email);
        var schedule =  doctor.Schedules!
            .FirstOrDefault(d => d.DayOfWeek == (DayOfWeek)dayOfWeek);

        var patientEmail = new BookingSchedule()
        {
            PatientEmail = pEmail
        };
        
        if (schedule is null) return schedule!;
        
        schedule.Patients?.Add(patientEmail);
        await _dbContext.SaveChangesAsync();

        // var schedule = await _dbContext.Schedules!.FirstOrDefaultAsync(s => s.ScheduleId == sId);
        // schedule!.Patients?.Add(patientEmail);

        return schedule;
    }

    public async Task UpdateImageUrl(string uId, string? url)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Email ==uId);

        if (url is not null)
        {
            user.CertificateUrl = url;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async void Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}