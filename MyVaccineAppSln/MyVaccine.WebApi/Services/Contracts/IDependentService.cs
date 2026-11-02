using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IDependentService
{
    Task<IEnumerable<DependentResponseDTO>> GetAll();
    Task<DependentResponseDTO> GetById(int id);
    Task<IEnumerable<DependentResponseDTO>> GetDependentsByUserId(int userId);
    Task<DependentResponseDTO> Add(DependentRequestDTO request);
    Task<DependentResponseDTO> Update(int id, DependentRequestDTO request);
    Task<DependentResponseDTO> Delete(int id);
    Task RegisterAllergyAsync(DependentAllergyRequestDTO request);
}