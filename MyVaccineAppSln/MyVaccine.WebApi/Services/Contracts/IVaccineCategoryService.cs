using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IVaccineCategoryService
{
    Task<IEnumerable<VaccineCategoryResponseDTO>> GetAll();
    Task<VaccineCategoryResponseDTO> GetById(int id);
    Task<IEnumerable<VaccineCategory_VaccineResponseDTO>> GetVaccinesByCategoryId(int vaccineCategoryId);
    Task<VaccineCategoryResponseDTO> Add(VaccineCategoryRequestDTO request);
    Task<VaccineCategoryResponseDTO> Update(int id, VaccineCategoryRequestDTO request);
    Task<VaccineCategoryResponseDTO> Delete(int id);
    Task RegisterVaccineAsync(VaccineCategory_VaccineRequestDTO request);
}
