using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeleDoc.API.Area.Patients.Models;
using TeleDoc.API.Context;
using TeleDoc.API.Dtos.PatientsDto;
using TeleDoc.API.Models;
using TeleDoc.API.Static;

namespace TeleDoc.API.Services;

public class PatientRepository : IPatientRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;

    public PatientRepository(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<List<PatientDetailsDto>?> GetPatientListAsync()
    {
        var result = await _userManager.Users
            .Where(r => r.Role == UserRoles.Patient)
            .ToListAsync();
        
        var data = _mapper.Map<List<Patient>>(result);
        var dataToReturn = _mapper.Map<List<PatientDetailsDto>>(data);

        return dataToReturn;

    }

    public async Task<Patient> GetPatientByEmail(string email)
    {
        var result = await _userManager.FindByEmailAsync(email);
        
        var data = _mapper.Map<Patient>(result);
        var dataToReturn = _mapper.Map<PatientDetailsDto>(data);

        return data;
    }

    public async Task<Patient> UpdatePatientByEmail(Patient patient)
    {
        // var doc = _dbContext.Users.FirstOrDefault(d => d.Email == doctor.Email);
        var pat = _dbContext.Users.FirstOrDefault(p => p.Email == patient.Email);
        // doc = _mapper.Map<Doctor>(doc);
        // doc = doctor;

        if (pat is not null)
        {
            pat.Name = patient.Name;
            pat.Gender = patient.Gender;
            pat.Address = patient.Address;
            pat.Disease = patient.Disease;
            pat.PhoneNumber = patient.PhoneNumber;
            pat.DateOfBirth = patient.DateOfBirth;

        }

        await _dbContext.SaveChangesAsync();

        return patient;
    }
}