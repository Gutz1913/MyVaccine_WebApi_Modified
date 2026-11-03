using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IVaccineService
{
    Task<IEnumerable<VaccineResponseDTO>> GetAll();
    Task<VaccineResponseDTO> GetById(int id);
    Task<VaccineResponseDTO> Add(VaccineRequestDTO request);
    Task<VaccineResponseDTO> Update(int id, VaccineRequestDTO request);
    Task<VaccineResponseDTO> Delete(int id);
}