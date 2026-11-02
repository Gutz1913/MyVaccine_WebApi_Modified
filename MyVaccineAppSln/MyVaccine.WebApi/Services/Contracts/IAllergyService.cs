using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IAllergyService
{
    Task<IEnumerable<AllergyResponseDTO>> GetAll();
    Task<AllergyResponseDTO> GetById(int id);
    Task<AllergyResponseDTO> Add(AllergyRequestDTO request);
    Task<AllergyResponseDTO> Update(int id, AllergyRequestDTO request);
    Task<AllergyResponseDTO> Delete(int id);
}
