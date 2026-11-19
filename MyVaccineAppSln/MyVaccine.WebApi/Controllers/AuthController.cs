using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Services.Contracts;
using MyVaccine.WebApi.Services.Implementations;
using System.Security.Claims;

namespace MyVaccine.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly SaveProfileImageUserService _saveProfileImageUserService;
    public AuthController(IUserService userService, SaveProfileImageUserService saveProfileImageUserService)
    {
        _userService = userService;
        _saveProfileImageUserService = saveProfileImageUserService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
    {
        var response = await _userService.AddUserAsync(model);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
    {
        var response = await _userService.Login(model);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        else
        {
            return Unauthorized(response);
        }
    }

    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromQuery] LoginRequestDTO model)
    {
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var response = await _userService.RefreshToken(claimsIdentity.Name);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        else
        {
            return Unauthorized(response);
        }
    }

    [Authorize]
    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity?.Name == null)
            {
                return Unauthorized(new { message = "No hay sesión activa" });
            }

            var response = await _userService.GetUserInfo(claimsIdentity.Name);
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener información de usuario" });
        }
    }

    [Authorize]
    [HttpPost("update-user-photo-profile")]
    public async Task<IActionResult> UpdateUserPhotoImage(IFormFile file)
    {
        // ✅ Validar archivo
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "El archivo no puede estar vacío" });
        }

        // ✅ Obtener identidad del usuario
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        if (claimsIdentity?.Name == null)
        {
            return Unauthorized(new { message = "No se pudo obtener la identidad del usuario" });
        }

        // ✅ Guardar imagen
        var response = await _saveProfileImageUserService.SaveImageAsync(file, claimsIdentity.Name);

        return Ok(new { message = "Foto actualizada exitosamente", photoPath = response });
    }

    [Authorize]
    [HttpGet("get-user-photo-profile")]
    public async Task<IActionResult> GetUserPhotoImage()
    {
        try
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity?.Name == null)
            {
                return Unauthorized(new { message = "No hay sesión activa" });
            }

            var (success, bytes) = await _saveProfileImageUserService.GetImageAsync(claimsIdentity.Name);
            if (!success || bytes == null)
            {
                return NotFound(new { message = "No hay foto de perfil" });
            }

            return File(bytes, "image/jpeg");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener foto" });
        }
    }
}

