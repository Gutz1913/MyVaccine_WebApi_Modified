using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IVaccineRecordService
{
    Task<IEnumerable<VaccineRecordResponseDTO>> GetAll();
    Task<VaccineRecordResponseDTO> GetById(int id);
    Task<IEnumerable<VaccineRecordResponseDTO>> GetRecordsByUserId(int userId);
    Task<VaccineRecordResponseDTO> Add(VaccineRecordRequestDTO request);
    Task<VaccineRecordResponseDTO> Update(int id, VaccineRecordRequestDTO request);
    Task<VaccineRecordResponseDTO> Delete(int id);
}