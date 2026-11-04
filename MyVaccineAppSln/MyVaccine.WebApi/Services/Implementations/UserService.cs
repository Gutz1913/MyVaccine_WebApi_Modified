using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyVaccine.WebApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IBaseRepository<Allergy> _allergyRepository;
    private readonly IBaseRepository<UserAllergy> _userAllergyRepository;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<IdentityUser> userManager, 
        IConfiguration configuration, 
        IUserRepository userRepository,
        IBaseRepository<Allergy> allergyRepository,
        IBaseRepository<UserAllergy> userAllergyRepository,
        IMapper mapper)
    {
        _userManager = userManager;
        _configuration = configuration;
        _userRepository = userRepository;
        _allergyRepository = allergyRepository;
        _userAllergyRepository = userAllergyRepository;
        _mapper = mapper;
    }

    public async Task<AuthResponseDTO> AddUserAsync(RegisterRequestDTO request)
    {
        var response = new AuthResponseDTO();
        try
        {
            var result = await _userRepository.AddUser(request);

            if (result != null)
            {
                response.IsSuccess = result.Succeeded;
                response.Errors = result?.Errors?.Select(x => x.Description).ToArray() ?? new string[] { };
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Errors = new string[] { ex.Message }; ;
        }

        return response;
    }

    public async Task<AuthResponseDTO> Login(LoginRequestDTO request)
    {
        var response = new AuthResponseDTO();
        try
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, user.UserName)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    //issuer: _configuration["JwtIssuer"],
                    //audience: _configuration["JwtAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(15), //Tiempo de vida del token
                    signingCredentials: creds
                );

                var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
                response.Token = tokenResult;
                response.Expiration = token.ValidTo;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Errors = new string[] { ex.Message };
        }

        return response;
    }

    public async Task<AuthResponseDTO> RefreshToken(string email)
    {
        var response = new AuthResponseDTO();
        try
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    //issuer: _configuration["JwtIssuer"],
                    //audience: _configuration["JwtAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(15), //Tiempo de vida del token
                    signingCredentials: creds
                );

                var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
                response.Token = tokenResult;
                response.Expiration = token.ValidTo;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Errors = new string[] { ex.Message };
        }

        return response;
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAll()
    {
        var users = await _userRepository
            .GetAll()
            .Include(u => u.Dependents)
            .Include(u => u.UserAllergies)
                .ThenInclude(ua => ua.Allergy)
            .Include(u => u.AspNetUser)
            .AsNoTracking()
            .ToListAsync();

        var response =  _mapper.Map<IEnumerable<UserResponseDTO>>(users);

        return response;
    }

    public async Task<UserResponseDTO> GetById(int id)
    {
        var user = await _userRepository
            .FindByAsNoTracking(u => u.Id == id)
            .Include(u => u.Dependents)
            .Include(u => u.UserAllergies)
                .ThenInclude(ua => ua.Allergy)
            .Include(u => u.AspNetUser)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

        
        var response = _mapper.Map<UserResponseDTO>(user);

        return response;
    }

    public async Task<UserResponseDTO> Update(int id, UpdateUserRequestDTO request)
    {
        var user = await _userRepository
            .FindBy(u => u.Id == id)
            .Include(u => u.Dependents)
            .Include(u => u.UserAllergies)
                .ThenInclude(ua => ua.Allergy)
            .Include(u => u.AspNetUser)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

        // Actualizar los campos del usuario
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        await _userRepository.Update(user);
        var userWithDetails = await _userRepository
            .FindBy(u => u.Id == user.Id)
            .Include(u => u.Dependents)
            .Include(u => u.UserAllergies)
                .ThenInclude(ua => ua.Allergy)
            .Include(u => u.AspNetUser)
            .FirstOrDefaultAsync();

        var response = _mapper.Map<UserResponseDTO>(userWithDetails);

        return response;
    }

    public async Task<UserResponseDTO> Delete(int id)
    {
        var user = await _userRepository
            .FindBy(u => u.Id == id)
            .Include(u => u.Dependents)
            .Include(u => u.UserAllergies)
                .ThenInclude(ua => ua.Allergy)
            .Include(u => u.AspNetUser)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

        await _userRepository.Delete(user);
        var response = _mapper.Map<UserResponseDTO>(user);

        return response;
    }

    public async Task RegisterAllergyAsync(UserAllergyRequestDTO request)
    {
        var user = await _userRepository
            .FindBy(u => u.Id == request.UserId)
            .Include(u => u.Dependents)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new KeyNotFoundException($"Usuario con ID {request.UserId} no encontrado.");

        var allergy = await _allergyRepository.FindBy(a => a.Id == request.AllergyId).FirstOrDefaultAsync();

        if (allergy == null)
            throw new KeyNotFoundException($"Alergia con ID {request.AllergyId} no encontrada.");

        var userAllergy = new UserAllergy()
        {
            UserId = request.UserId,
            AllergyId = request.AllergyId,
            RegisteredAt = DateTime.UtcNow
        };

        await _userAllergyRepository.Add(userAllergy);
        _mapper.Map<UserAllergyResponseDTO>(userAllergy);
    }
}
