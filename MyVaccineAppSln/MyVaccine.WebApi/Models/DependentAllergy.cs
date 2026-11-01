namespace MyVaccine.WebApi.Models;

public class DependentAllergy : BaseModel
{
    public int DependentId { get; set; }
    public int AllergyId { get; set; }
    public DateTime RegisteredAt { get; set; }

    public Dependent Dependent { get; set; } = null!;
    public Allergy Allergy { get; set; } = null!;
}
