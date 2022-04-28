using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeleDoc.API.Area.Patients.Models;
using TeleDoc.API.Dtos.PatientsDto;
using TeleDoc.API.Models.Account;
using TeleDoc.API.Services;
using TeleDoc.DAL.Entities;
using TeleDoc.DAL.Enums;
using TeleDoc.DAL.Exceptions;

namespace TeleDoc.API.Area.Patients.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : Controller
{
    private readonly IAuthRepository<ApplicationUser> _authRepo;
    private readonly IPatientRepository _patientRepo;
    private readonly IMapper _mapper;

    public PatientsController(IAuthRepository<ApplicationUser> authRepo, IMapper mapper, IPatientRepository patientRepo)
    {
        _authRepo = authRepo;
        _mapper = mapper;
        _patientRepo = patientRepo;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _authRepo.Register(model);

        var data = _mapper.Map<Patient>(result.Data);
        var dataToReturn = _mapper.Map<PatientDetailsDto>(data);

        return result.Status switch
        {
            ResponseStatus.Succeeded => Ok(dataToReturn),
            ResponseStatus.Duplicate => throw new DuplicateException(model.Email),
            ResponseStatus.Failed => throw new FailedException("register", model.Email),
            _ => BadRequest()
        };

    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _authRepo.Login(model);
        
        var data = _mapper.Map<Patient>(result.Data);
        var dataToReturn = _mapper.Map<PatientDetailsDto>(data);

        return result.Status switch
        {
            ResponseStatus.Succeeded => Ok(new {result.Token, dataToReturn}),
            ResponseStatus.NotFound => throw new NotFoundException("user with " + model.Email),
            _ => Unauthorized()
        };
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPatientListAsync()
    {
        var result = await _patientRepo.GetPatientListAsync();

        return Ok(result);
    }


    [HttpGet("p")]
    public async Task<IActionResult> GetPatient([FromQuery] string email)
    {
        var result = await _patientRepo.GetPatientByEmail(email);

        return Ok(result);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdatePatient([FromQuery] string email, Patient patient)
    {
        if (email != patient.Email) return BadRequest();
        
        var result = await _patientRepo.UpdatePatientByEmail(patient);
    
        return Ok(result);
    }



}