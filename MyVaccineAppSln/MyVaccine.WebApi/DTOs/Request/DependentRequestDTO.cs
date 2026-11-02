namespace MyVaccine.WebApi.DTOs.Request;

public class DependentRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public int UserId { get; set; }
    public int FamilyGroupId { get; set; }
}