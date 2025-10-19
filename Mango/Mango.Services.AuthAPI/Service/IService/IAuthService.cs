using Mango.Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Identity.Data;

namespace Mango.Services.AuthAPI.Service.IService;

public interface IAuthService
{
    Task<string> Register(RegisterRequestDto registerRequest);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
    Task<bool> AssignRole(string email, string roleName);
}