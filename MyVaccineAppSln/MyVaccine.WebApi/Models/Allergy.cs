namespace MyVaccine.WebApi.Models;

public class Allergy : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public ICollection<UserAllergy> UserAllergies { get; set; } = new List<UserAllergy>();
    public ICollection<DependentAllergy> DependentAllergies { get; set; } = new List<DependentAllergy>();
}
