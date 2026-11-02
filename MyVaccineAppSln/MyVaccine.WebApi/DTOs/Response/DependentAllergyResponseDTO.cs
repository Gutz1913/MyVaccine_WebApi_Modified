namespace MyVaccine.WebApi.DTOs.Response;

public class DependentAllergyResponseDTO
{
    public int Id { get; set; }
    public int DependentId { get; set; }
    public string DependentName { get; set; } = string.Empty;
    public int AllergyId { get; set; }
    public string AllergyName { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
}