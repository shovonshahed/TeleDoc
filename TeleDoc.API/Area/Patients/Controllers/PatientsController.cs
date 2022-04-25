using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeleDoc.API.Area.Patients.Models;
using TeleDoc.API.Dtos.Patients;
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
    private readonly IMapper _mapper;

    public PatientsController(IAuthRepository<ApplicationUser> authRepo, IMapper mapper)
    {
        _authRepo = authRepo;
        _mapper = mapper;
    }

    // GET
    public string Index()
    {
        return "patient is working";
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
}