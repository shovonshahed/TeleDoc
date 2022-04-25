using TeleDoc.API.Models;
using TeleDoc.API.Models.Account;
using TeleDoc.DAL.Entities;
using TeleDoc.DAL.Extensions;

namespace TeleDoc.API.Services;

public interface IAuthRepository<T> where T: ApplicationUser, new()
{
    Task<CustomResponse> Register(RegisterViewModel model);
    Task<LoginResponse> Login(LoginViewModel model);
}