namespace MyVaccine.WebApi.DTOs.Response;

public class VaccineCategory_VaccineResponseDTO
{
    public int Id { get; set; }
    public int VaccineId { get; set; }
    public string VaccineName { get; set; } = string.Empty;
    public int VaccineCategoryId { get; set; }
    public string VaccineCategoryName { get; set; } = string.Empty;
    public string VaccineCategoryDescription { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
}