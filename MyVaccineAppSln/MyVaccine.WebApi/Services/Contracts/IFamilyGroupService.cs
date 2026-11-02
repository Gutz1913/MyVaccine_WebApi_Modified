using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IFamilyGroupService
{
    Task<IEnumerable<FamilyGroupResponseDTO>> GetAll();
    Task<FamilyGroupResponseDTO> GetById(int id);
    Task<FamilyGroupResponseDTO> Add(FamilyGroupRequestDTO request);
    Task<FamilyGroupResponseDTO> Update(int id, FamilyGroupRequestDTO request);
    Task<FamilyGroupResponseDTO> Delete(int id);
}
