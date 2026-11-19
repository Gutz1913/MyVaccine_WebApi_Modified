using MyVaccine.WebApi.DTOs.Response;

namespace MyVaccine.WebApi.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? Birthdate { get; set; }
    public string? Photo { get; set; }
    public ICollection<DependentResponseDTO> Dependents { get; set; } = new List<DependentResponseDTO>();
    public ICollection<FamilyGroupResponseDTO> FamilyGroups { get; set; } = new List<FamilyGroupResponseDTO>();
    public ICollection<VaccineRecordResponseDTO> VaccineRecords { get; set; } = new List<VaccineRecordResponseDTO>();
    public ICollection<UserAllergyResponseDTO> UsersAllergies { get; set; } = new List<UserAllergyResponseDTO>();
}
