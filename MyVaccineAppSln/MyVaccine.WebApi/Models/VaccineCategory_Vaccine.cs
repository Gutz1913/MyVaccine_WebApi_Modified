namespace MyVaccine.WebApi.Models;

public class VaccineCategory_Vaccine : BaseModel
{
    public int VaccineId { get; set; }
    public int VaccineCategoryId { get; set; }
    public DateTime RegisteredAt { get; set; }

    public Vaccine Vaccine { get; set; } = null!;
    public VaccineCategory VaccineCategory { get; set; } = null!;
}
