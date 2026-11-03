namespace MyVaccine.WebApi.DTOs.Response;

public class VaccineRecordResponseDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public int DependentId { get; set; }
    public string DependentName { get; set; } = string.Empty;
    public int VaccineId { get; set; }
    public string VaccineName { get; set; } = string.Empty;
    public DateTime AdministeredDate { get; set; }
    public string AdministeredLocation { get; set; } = string.Empty;
    public string AdministeredBy { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}