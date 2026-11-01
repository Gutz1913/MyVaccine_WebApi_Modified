namespace MyVaccine.WebApi.Models;

public class FamilyGroup : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();
}
