using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mango.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;

    public AuthController(IAuthService authService, ITokenProvider tokenProvider)
    {
        _authService = authService;
        _tokenProvider = tokenProvider;
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDto loginRequestDto = new LoginRequestDto();
        
        return View(loginRequestDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
    {
        ResponseDto? responseDto = await _authService.LoginAsync(loginRequestDto);

        if (responseDto != null && responseDto.Success)
        {
            LoginResponseDto? loginResponseDto = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseDto>(responseDto.Result?.ToString() ?? string.Empty);
            await SignInUser(loginResponseDto);
            _tokenProvider.SetToken(loginResponseDto?.Token);
            
            TempData["success"] = "Login Successful";
            return Redirect($"/Home/Index");
        }
        else
        {
            TempData["error"] = responseDto.Message;
            return View(loginRequestDto);
        }
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        var roleList = new List<SelectListItem>()
        {
            new SelectListItem {Text = SD.RoleAdmin, Value = SD.RoleAdmin},
            new SelectListItem {Text = SD.RoleCustomer, Value = SD.RoleCustomer},
        };
        ViewBag.RoleList = roleList;
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
    {
        ResponseDto? result = await _authService.RegisterAsync(registerRequestDto);
        ResponseDto? assignRole;

        if (result != null && result.Success)
        {
            if (string.IsNullOrEmpty(registerRequestDto.Role))
            {
                registerRequestDto.Role = SD.RoleCustomer;
            }

            assignRole = await _authService.AssignRoleAsync(registerRequestDto);
            if (assignRole != null && assignRole.Success)
            {
                TempData["success"] = "Registration Successful";
                return Redirect(nameof(Login));
            }
        }
        else
        {
            TempData["error"] = result.Message;
        }
        
        var roleList = new List<SelectListItem>()
        {
            new SelectListItem {Text = SD.RoleAdmin, Value = SD.RoleAdmin},
            new SelectListItem {Text = SD.RoleCustomer, Value = SD.RoleCustomer},
        };
        ViewBag.RoleList = roleList;
        return View(registerRequestDto);
    }
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        _tokenProvider.ClearToken();
        return Redirect("/Home/Index");
    }

    private async Task SignInUser(LoginResponseDto model)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(model.Token);
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        
        var valueEmail = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;
        var valueId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var valueUserName = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name)?.Value;
        var valueUserRole = jwt.Claims.FirstOrDefault(u => u.Type == "role")?.Value;

        if (valueEmail != null && valueId != null && valueUserName != null && valueUserRole != null)
        {
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, valueEmail));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, valueId));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, valueUserName));
            identity.AddClaim(new Claim(ClaimTypes.Name, valueEmail));
            identity.AddClaim(new Claim(ClaimTypes.Role, valueUserRole));
        }

        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}