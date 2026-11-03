namespace MyVaccine.WebApi.DTOs.Request;

public class VaccineRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public bool RequiresBooster { get; set; }
}
