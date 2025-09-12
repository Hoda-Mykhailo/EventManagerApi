using EventManagerApi.Models.DTO;
using EventManagerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;

    public AuthController(IUserService service) => _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var token = await _service.RegisterAsync(dto);
        if (token == null) return BadRequest("User already exists");
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _service.LoginAsync(dto);
        if (token == null) return Unauthorized();
        return Ok(new { token });
    }
}
