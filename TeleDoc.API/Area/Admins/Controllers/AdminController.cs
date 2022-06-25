using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeleDoc.API.Area.Admins.Models;
using TeleDoc.API.Context;

namespace TeleDoc.API.Area.Admins.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AdminController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Index()
    {
        return "Wahhh... Admin controller";
    }

    [HttpPost("ambulance")]
    public async Task<IActionResult> AddAmbulance([FromBody] Ambulance ambulance)
    {
        if (!ModelState.IsValid) return BadRequest(ambulance);
        
         _dbContext.Ambulances!.Add(ambulance);
        await _dbContext.SaveChangesAsync();

        return Ok(ambulance);
    }


}