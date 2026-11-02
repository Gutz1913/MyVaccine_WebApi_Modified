namespace MyVaccine.WebApi.DTOs.Response;

public class DependentResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public int FamilyGroupId { get; set; }
    public string FamilyGroupName { get; set; } = string.Empty;
    public int AllergyId { get; set; }
    public string AllergyName { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
