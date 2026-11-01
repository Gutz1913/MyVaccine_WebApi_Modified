namespace MyVaccine.WebApi.Models;

public class UserAllergy : BaseModel
{
    public int UserId { get; set; }
    public int AllergyId { get; set; }
    public DateTime RegisteredAt { get; set; }

    public User User { get; set; } = null!;
    public Allergy Allergy { get; set; } = null!;
}
