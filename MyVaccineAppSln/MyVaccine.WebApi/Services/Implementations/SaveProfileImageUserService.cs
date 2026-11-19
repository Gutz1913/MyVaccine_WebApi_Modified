using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;

namespace MyVaccine.WebApi.Services.Implementations;

public class SaveProfileImageUserService
{
    private readonly string _imagesDirectory = "Images";
    private readonly IUserRepository _userRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

    public SaveProfileImageUserService(IUserRepository userRepository, UserManager<IdentityUser> userManager)
    {
        if (!Directory.Exists(_imagesDirectory))
        {
            Directory.CreateDirectory(_imagesDirectory);
        }
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<string> SaveImageAsync(IFormFile file, string userId)
    {
        // Validar archivo
        ValidateFile(file);

        // Buscar usuario por nombre
        var appNetUser = await _userManager.FindByNameAsync(userId);
        if (appNetUser == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado.");
        }

        // Buscar registro de User por AspNetUserId
        var user = await _userRepository
            .FindBy(u => u.AspNetUserId == appNetUser.Id)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException("Registro de usuario no encontrado.");
        }

        // Eliminar foto anterior si existe
        if (!string.IsNullOrWhiteSpace(user.Photo))
        {
            var existingImagePath = Path.Combine(Directory.GetCurrentDirectory(), user.Photo);
            if (File.Exists(existingImagePath))
            {
                File.Delete(existingImagePath);
            }
        }

        // Guardar nueva foto
        var relativePath = Path.Combine(_imagesDirectory, Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
        var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        using (var stream = new FileStream(absolutePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Actualizar registro de User
        user.Photo = relativePath;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.Update(user);

        return relativePath;
    }

    public async Task<(bool, byte[]?)> GetImageAsync(string userId)
    {
        // Buscar usuario por nombre
        var appNetUser = await _userManager.FindByNameAsync(userId);
        if (appNetUser == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado.");
        }

        // Buscar registro de User por AspNetUserId
        var user = await _userRepository
            .FindByAsNoTracking(u => u.AspNetUserId == appNetUser.Id)
            .FirstOrDefaultAsync();

        if (user == null || string.IsNullOrWhiteSpace(user.Photo))
        {
            return (false, null);
        }

        var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), user.Photo);

        if (File.Exists(absolutePath))
        {
            var bytes = await File.ReadAllBytesAsync(absolutePath);
            return (true, bytes);
        }

        return (false, null);
    }

    /// <summary>
    /// Valida que el archivo sea una imagen válida
    /// </summary>
    private void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("El archivo no puede estar vacío.", nameof(file));
        }

        if (file.Length > _maxFileSize)
        {
            throw new InvalidOperationException($"El archivo no puede exceder {_maxFileSize / (1024 * 1024)}MB.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException($"Las extensiones permitidas son: {string.Join(", ", _allowedExtensions)}");
        }

        var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedContentTypes.Contains(file.ContentType?.ToLowerInvariant()))
        {
            throw new InvalidOperationException("El tipo de archivo no es una imagen válida.");
        }
    }
}
