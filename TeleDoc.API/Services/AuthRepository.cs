using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using TeleDoc.API.Enums;
using TeleDoc.API.Extensions;
using TeleDoc.API.Models;
using TeleDoc.API.Models.Account;

namespace TeleDoc.API.Services;

public class AuthRepository<T> : IAuthRepository<T> where T : ApplicationUser, new()
{
    private readonly UserManager<T> _userManager;
    private readonly SignInManager<T> _signInManager;
    private readonly IConfiguration _config;
    private readonly IEmailSender _emailSender;

    public AuthRepository(UserManager<T> userManager, SignInManager<T> signInManager, IConfiguration config, IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _emailSender = emailSender;
    }

    public async Task<CustomResponse> Register(RegisterViewModel model, string role)
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
            Email = model.Email,
            Role = role
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        await _userManager.AddToRoleAsync(user, role);

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

    public async Task<LoginResponse> Login(LoginViewModel model, string role)
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
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, role)
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

                // await _emailSender.SendEmailAsync(user.Email, "new login", "<p>new login detected</p>");
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

    public async Task<CustomResponse> ForgotPassword(ForgotPasswordViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        var response = new CustomResponse();
        if (user is null)
        {
            response.Status = ResponseStatus.NotFound;
            response.Data = null;
        }
        
        var code = await _userManager.GeneratePasswordResetTokenAsync(user!);
        // var callbackUrl = $"{_config["applicationUrl"]}/resetPassword?email={model.Email}&code={code}";
        //
        // await _emailSender.SendEmailAsync(model.Email, "Reset Password - TeleDoc",
        //     "reset your password by clicking " + callbackUrl);
        

        response.Status = ResponseStatus.Succeeded;
        response.Data = code;
        
        
        return response;
    }
}