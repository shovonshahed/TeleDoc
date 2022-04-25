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

public class AuthRepository<T> : IAuthRepository<T> where T : ApplicationUser
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _config;

    public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
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
            response.User = null;

            return response;
        }

        user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            response.Status = ResponseStatus.Failed;
            response.User = null;
            return response;
        }

        response.Status = ResponseStatus.Succeeded;
        response.User = user;

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
            response.User = null;
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
                response.User = user;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Token = null;
                response.User = null;
            }
        }

        return response;

    }
}