namespace MyVaccine.WebApi.DTOs.Request;

public class DependentAllergyRequestDTO
{
    public int DependentId { get; set; }
    public int AllergyId { get; set; }
    public DateTime RegisteredAt { get; set; }
}
