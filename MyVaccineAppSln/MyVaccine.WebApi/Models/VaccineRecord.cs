namespace MyVaccine.WebApi.Models;

public class VaccineRecord : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int DependentId { get; set; }
    public Dependent Dependent { get; set; } = null!;
    public int VaccineId { get; set; }
    public Vaccine Vaccine { get; set; } = null!;
    public DateTime AdministeredDate { get; set; }
    public string AdministeredLocation { get; set; } = string.Empty;
    public string AdministeredBy { get; set; } = string.Empty;
}
