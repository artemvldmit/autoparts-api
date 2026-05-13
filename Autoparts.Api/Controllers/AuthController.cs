using Autoparts.DataAccess;
using Autoparts.Domains.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autoparts.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db) => _db = db;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user is null || user.PasswordHash != dto.Password)
            return Unauthorized(new { message = "Неверный email или пароль" });

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserRole", user.Role);
        HttpContext.Session.SetString("UserName", user.Name);

        return Ok(new { id = user.Id, name = user.Name, role = user.Role, email = user.Email });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { message = "Выход выполнен" });
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId is null)
            return Ok(new { role = "Visitor" });

        return Ok(new
        {
            id = userId,
            name = HttpContext.Session.GetString("UserName"),
            role = HttpContext.Session.GetString("UserRole")
        });
    }
}
