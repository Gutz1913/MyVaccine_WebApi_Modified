namespace MyVaccine.WebApi.Models;

public class VaccineCategory : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<VaccineCategory_Vaccine> VaccineCategory_Vaccines { get; set; } = new List<VaccineCategory_Vaccine>();
}
