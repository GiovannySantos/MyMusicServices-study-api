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
    public async Task<IActionResult> Login([FromBody] User loginUser)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginUser.Username);
        if (user == null)
            return Unauthorized("Usuário não encontrado.");

        var hashedPassword = _hasher.HashPassword(loginUser.PasswordHash);
        if (user.PasswordHash != hashedPassword)
            return Unauthorized("Senha inválida.");

        var token = _jwtService.GenerateToken(user);

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            return BadRequest("Usuário já existe.");

        // Aqui você pode fazer hash da senha, mas para testar agora, salve simples
        user.PasswordHash = user.PasswordHash; // Troque depois pelo hash!

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Usuário criado com sucesso.");
    }
}
