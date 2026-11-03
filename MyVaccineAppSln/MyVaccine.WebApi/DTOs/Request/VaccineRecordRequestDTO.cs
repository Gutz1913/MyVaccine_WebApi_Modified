namespace MyVaccine.WebApi.DTOs.Request;

public class VaccineRecordRequestDTO
{
    public int UserId { get; set; }
    public int DependentId { get; set; }
    public int VaccineId { get; set; }
    public DateTime AdministeredDate { get; set; }
    public string AdministeredLocation { get; set; } = string.Empty;
    public string AdministeredBy { get; set; } = string.Empty;
}