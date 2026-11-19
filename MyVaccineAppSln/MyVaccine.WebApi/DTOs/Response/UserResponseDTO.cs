namespace MyVaccine.WebApi.DTOs.Response;

public class UserResponseDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public int DependentsCount { get; set; }
    public int AllergyId { get; set; }
    public string AllergyName { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
