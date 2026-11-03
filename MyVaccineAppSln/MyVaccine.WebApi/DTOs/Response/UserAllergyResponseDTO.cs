namespace MyVaccine.WebApi.DTOs.Response;

public class UserAllergyResponseDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; } = null!;
    public int AllergyId { get; set; }
    public string AllergyName { get; set; } = null!;
    public DateTime RegisteredAt { get; set; }
}
