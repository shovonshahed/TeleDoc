using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleDoc.API.Area.Doctors.Models;
using TeleDoc.API.Dtos.DoctorsDto;
using TeleDoc.API.Models.Account;
using TeleDoc.API.Services;
using TeleDoc.API.Static;
using TeleDoc.DAL.Entities;
using TeleDoc.DAL.Enums;
using TeleDoc.DAL.Exceptions;

namespace TeleDoc.API.Area.Doctors.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CustomeRoles.DoctorAdmin)]
[ApiController]
[Route("api/[controller]")]
public class DoctorsController : Controller
{
    private readonly IAuthRepository<ApplicationUser> _authRepo;
    private readonly IMapper _mapper;
    private readonly IDoctorRepository _doctorRepo;
    
    public DoctorsController(IAuthRepository<ApplicationUser> authRepo, IMapper mapper, IDoctorRepository doctorRepo)
    {
        _authRepo = authRepo;
        _mapper = mapper;
        _doctorRepo = doctorRepo;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _authRepo.Register(model, UserRoles.Doctor);

        var data = _mapper.Map<Doctor>(result.Data);
        var dataToReturn = _mapper.Map<DoctorDetailsDto>(data);

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

        var result = await _authRepo.Login(model, UserRoles.Doctor);
        
        var data = _mapper.Map<Doctor>(result.Data);
        var dataToReturn = _mapper.Map<DoctorDetailsDto>(data);

        return result.Status switch
        {
            ResponseStatus.Succeeded => Ok(new {result.Token, dataToReturn}),
            ResponseStatus.NotFound => throw new NotFoundException("user with " + model.Email),
            _ => Unauthorized()
        };
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetDoctorListAsync()
    {
        var result = await _doctorRepo.GetDoctorListAsync();

        return Ok(result);
    }
    

    
    [HttpGet("de")]
    public async Task<IActionResult> GetDoctorByEmail([FromQuery] string email)
    {
        var result = await _doctorRepo.GetDoctorByEmail(email);

        return Ok(result);
    }
    
   
    [HttpGet("dn")]
    public async Task<IActionResult> GetDoctorByName([FromQuery] string name)
    {
        var result = await _doctorRepo.GetDoctorByName(name);

        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateDoctor([FromQuery] string email, Doctor doctor)
    {
        if (email != doctor.Email) return BadRequest();
        
        var result = await _doctorRepo.UpdateDoctorByEmail(doctor);

        return Ok(result);
    }





}