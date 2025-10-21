using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace Mango.Services.AuthAPI.Service;

public class AuthService: IAuthService
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    public AuthService(
        AppDbContext db,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<string> Register(RegisterRequestDto registerRequest)
    {
        ApplicationUser user = new()
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            NormalizedEmail = registerRequest.Email.ToUpper(),
            Name = registerRequest.Name,
            PhoneNumber = registerRequest.PhoneNumber,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (result.Succeeded)
            {
                var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registerRequest.Email);
                UserDto userDto = new()
                {
                    Email = userToReturn.Email,
                    Name = userToReturn.Name,
                    PhoneNumber = userToReturn.PhoneNumber,
                    ID = userToReturn.Id
                };
                return "";
            }
            else
            {
                return result.Errors.First().Description;
            }
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequest.Username.ToLower());
        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

        if (user == null || isValid == false)
        {
            return new LoginResponseDto() { User = null, Token = "" };
        }
        
        // if user found, generate jwt token
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenGenerator.GenerateToken(user, roles);
        UserDto userDto = new UserDto()
        {
            Email = user.Email,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            ID = user.Id
        };
        
        LoginResponseDto loginResponseDto = new LoginResponseDto() { User = userDto, Token = token };
        return loginResponseDto;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == email.ToLower());
        if (user != null)
        {
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                // create role if it does not exits
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }
        return false;
    }
}