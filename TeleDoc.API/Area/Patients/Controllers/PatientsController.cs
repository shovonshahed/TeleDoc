using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleDoc.API.Area.Patients.Models;
using TeleDoc.API.Dtos.PatientsDto;
using TeleDoc.API.Enums;
using TeleDoc.API.Exceptions;
using TeleDoc.API.Models;
using TeleDoc.API.Models.Account;
using TeleDoc.API.Services;
using TeleDoc.API.Static;

namespace TeleDoc.API.Area.Patients.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CustomeRoles.PatientAdmin)]
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

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _authRepo.Register(model, UserRoles.Patient);

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
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _authRepo.Login(model, UserRoles.Patient);
        
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
        Console.WriteLine(CustomeRoles.PatientAdmin);
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

    [AllowAnonymous]
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ForGotPassword([FromQuery] string email)
    {
        var model = new ForgotPasswordViewModel()
        {
            Email = email
        };
        var result = await _authRepo.ForgotPassword(model);

        if (result.Status == ResponseStatus.NotFound)
        {
            throw new NotFoundException(email);
        }
        else if (result.Status == ResponseStatus.Succeeded)
        {
            var url = Url.RouteUrl("url", new { code = result.Data }, protocol: HttpContext.Request.Scheme);

        }

        return Ok();
    }

    [HttpGet("appoinment")]
    public async Task<IActionResult> Appoinment()
    {
        var email = User.FindFirst(ClaimTypes.Email)!.Value;

        var schedule = await _patientRepo.GetAppoinment(email);

        return Ok(schedule);
    }




}