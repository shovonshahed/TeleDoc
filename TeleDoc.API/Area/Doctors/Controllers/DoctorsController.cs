using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleDoc.API.Area.Doctors.Models;
using TeleDoc.API.Dtos.DoctorsDto;
using TeleDoc.API.Enums;
using TeleDoc.API.Exceptions;
using TeleDoc.API.Models;
using TeleDoc.API.Models.Account;
using TeleDoc.API.Services;
using TeleDoc.API.Static;

namespace TeleDoc.API.Area.Doctors.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CustomeRoles.DoctorAdmin)]
[ApiController]
[Route("api/[controller]")]
public class DoctorsController : Controller
{
    private readonly IAuthRepository<ApplicationUser> _authRepo;
    private readonly IMapper _mapper;
    private readonly IDoctorRepository _doctorRepo;
    private readonly IWebHostEnvironment _hostEnvironment;
    
    public DoctorsController(IAuthRepository<ApplicationUser> authRepo, IMapper mapper, IDoctorRepository doctorRepo, IWebHostEnvironment hostEnvironment)
    {
        _authRepo = authRepo;
        _mapper = mapper;
        _doctorRepo = doctorRepo;
        _hostEnvironment = hostEnvironment;
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
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetDoctorListAsync()
    {
        var result = await _doctorRepo.GetDoctorListAsync();

        return Ok(result);
    }
    

    [AllowAnonymous]
    [HttpGet("de")]
    public async Task<IActionResult> GetDoctorByEmail([FromQuery] string email)
    {
        var result = await _doctorRepo.GetDoctorByEmail(email);

        return Ok(result);
    }
    
    [AllowAnonymous]
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


    [HttpGet("ApplyForCertified")]
    public async Task<IActionResult> ApplyForCertified()
    {
        var uId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var user = await _doctorRepo.ApplyForCertified(uId);
        
        return Ok(user);
    }

    [HttpPost("addSchedule")]
    public async Task<IActionResult> AddDoctorSchedule([FromBody] Schedule schedule)
    {
        var uId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        // if (id != uId) return Unauthorized();

        // var schedules = _doctorRepo.GetScheduleAsync(uId);
        // if (schedules.Result.Count > 5)
        //     throw new FailedException("schedule for 7 days already added");

        var result = await _doctorRepo.AddDoctorSchedule(uId, schedule);
        
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("schedule")]
    public async Task<IActionResult> GetDoctorScheduleList([FromQuery] string email)
    {
        var result = await _doctorRepo.GetScheduleAsync(email);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("booking")]
    public async Task<IActionResult> AddBookingSchedule([FromQuery] string email,[FromQuery]  int dayOfWeek)
    {
        var pEmail = User.FindFirst(ClaimTypes.Email)!.Value;

        var result = await _doctorRepo.AddBooking(pEmail, email, dayOfWeek);

        return Ok(result);
    }

    [HttpPost("documents")]
    public async Task<IActionResult> AddDocuments([FromForm] IFormFile? file)
    {
        var dId =  User.FindFirst(ClaimTypes.Email)!.Value;
        var doctor = await _doctorRepo.GetDoctorByEmail(dId);

        var wwwRootPath = _hostEnvironment.WebRootPath;
        if (file is not null)
        {
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath, @"images/");
            var extension = Path.GetExtension(file.FileName);

            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                file.CopyTo(fileStreams);
            }

            var url = @"/images/" + fileName + extension;
            await _doctorRepo.UpdateImageUrl(dId, url);
            _doctorRepo.Save();

            return Ok("image updated");
        }
        
        return BadRequest("something wrong");
    }

}