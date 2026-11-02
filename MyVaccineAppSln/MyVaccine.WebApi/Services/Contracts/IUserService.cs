using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IUserService
{
    Task<AuthResponseDTO> AddUserAsync(RegisterRequestDTO request);
    Task<AuthResponseDTO> Login(LoginRequestDTO request);
    Task<AuthResponseDTO> RefreshToken(string email);
    Task<IEnumerable<UserResponseDTO>> GetAll();
    Task<UserResponseDTO> GetById(int id);
    Task<UserResponseDTO> Update(int id, UpdateUserRequestDTO request);
    Task<UserResponseDTO> Delete(int id);
    Task RegisterAllergyAsync(UserAllergyRequestDTO request); 
}
