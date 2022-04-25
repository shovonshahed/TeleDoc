using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TeleDoc.API.Models.Account;
using TeleDoc.DAL.Entities;
using TeleDoc.DAL.Enums;
using TeleDoc.DAL.Extensions;

namespace TeleDoc.API.Services;

public class AuthRepository<T> : IAuthRepository<T> where T : ApplicationUser, new()
{
    private readonly UserManager<T> _userManager;
    private readonly SignInManager<T> _signInManager;
    private readonly IConfiguration _config;

    public AuthRepository(UserManager<T> userManager, SignInManager<T> signInManager, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    public async Task<CustomResponse> Register(RegisterViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        var response = new CustomResponse();
        if (user is not null)
        {
            response.Status = ResponseStatus.Duplicate;
            response.Data = null;

            return response;
        }

        user = new T
        {
            Name = model.Name,
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            response.Status = ResponseStatus.Failed;
            response.Data = null;
            return response;
        }

        response.Status = ResponseStatus.Succeeded;
        response.Data = user;

        return response;
    }

    public async Task<LoginResponse> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        var response = new LoginResponse();

        if (user is null)
        {
            response.Status = ResponseStatus.NotFound;
            response.Token = null;
            response.Data = null;
        }
        else
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSetting:Token").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = credentials,
                    Expires = DateTime.Now.AddDays(7)
                };
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                response.Status = ResponseStatus.Succeeded;
                response.Token = tokenHandler.WriteToken(token);
                response.Data = user;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Token = null;
                response.Data = null;
            }
        }

        return response;

    }
}