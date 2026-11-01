namespace MyVaccine.WebApi.Models;

public class Dependent : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int FamilyGroupId { get; set; }
    public FamilyGroup FamilyGroup { get; set; } = null!;
    public ICollection<DependentAllergy> DependentAllergies { get; set; } = new List<DependentAllergy>();
    public ICollection<VaccineRecord> VaccineRecords { get; set; } = new List<VaccineRecord>();
}
