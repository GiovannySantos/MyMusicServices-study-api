using Microsoft.AspNetCore.Mvc;
using MyMusicServicesApi.Models;
using MyMusicServicesApi.Services;
using MyMusicServicesApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MyMusicServicesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher _hasher;
    private readonly JwtTokenService _jwtService;

    public AuthController(AppDbContext context, PasswordHasher hasher, JwtTokenService jwtService)
    {
        _context = context;
        _hasher = hasher;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, [FromServices] AuthService authService)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || user.PasswordHash != request.Password)
            return Unauthorized("Usuário ou senha inválidos.");

        var token = authService.GenerateToken(user);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            return BadRequest("Username já existente.");

        user.PasswordHash = _hasher.HashPassword(user.PasswordHash);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Usuário criado com sucesso.");
    }

    [HttpGet("users")]
    public async Task<IActionResult> ObterUsarios()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }
}
