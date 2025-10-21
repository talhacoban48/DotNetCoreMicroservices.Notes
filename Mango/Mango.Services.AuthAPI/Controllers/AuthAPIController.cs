using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers;

[Route("/api/auth")]
[ApiController]
public class AuthAPIController : ControllerBase
{
    private readonly IAuthService _authService;
    protected ResponseDto _response;

    public AuthAPIController(IAuthService authService)
    {
        _authService = authService;
        _response = new ResponseDto();
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
    {
        var errorMessage = await _authService.Register(registerRequest);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.Success = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }
        return Ok(_response);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var loginResponse = await _authService.Login(loginRequest);
        if (loginResponse.User == null)
        {
            _response.Success = false;
            _response.Message = "Username or password is incorrect.";
            return BadRequest(_response);
        }
        _response.Success = true;
        _response.Message = "";
        _response.Result = loginResponse;
        return Ok(_response);
    }
    
    [HttpPost("AssignRole")]
    public async Task<IActionResult> AssignRole([FromBody] RegisterRequestDto model)
    {
        var assignRoleSuccessfull = await _authService.AssignRole(model.Email, model.Role.ToUpper());
        if (!assignRoleSuccessfull)
        {
            _response.Success = false;
            _response.Message = "Error encountered. Please try again.";
            return BadRequest(_response);
        }
        return Ok(_response);
    }
}