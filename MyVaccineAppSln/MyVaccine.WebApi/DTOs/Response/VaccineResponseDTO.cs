namespace MyVaccine.WebApi.DTOs.Response;

public class VaccineResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool RequiresBooster { get; set; }
    public int VaccineCategoryId { get; set; }
    public string VaccineCategoryName { get; set; } = string.Empty;
    public string VaccineCategoryDescription { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
