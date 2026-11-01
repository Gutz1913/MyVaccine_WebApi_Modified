namespace MyVaccine.WebApi.Models;

public class Vaccine : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public bool RequiresBooster { get; set; }
    public ICollection<VaccineCategory_Vaccine> VaccineCategory_Vaccines { get; set; } = new List<VaccineCategory_Vaccine>();
    public ICollection<VaccineRecord> VaccineRecords { get; set; } = new List<VaccineRecord>();
}
