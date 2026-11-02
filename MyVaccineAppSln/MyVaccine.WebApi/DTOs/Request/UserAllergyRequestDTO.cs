namespace MyVaccine.WebApi.DTOs.Request;

public class UserAllergyRequestDTO
{
    public int UserId { get; set; }
    public int AllergyId { get; set; }
    public DateTime RegisteredAt { get; set; }
}
