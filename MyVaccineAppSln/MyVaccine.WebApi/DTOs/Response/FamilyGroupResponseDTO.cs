namespace MyVaccine.WebApi.DTOs.Response;

public class FamilyGroupResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
