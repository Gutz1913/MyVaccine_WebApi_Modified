namespace MyVaccine.WebApi.DTOs.Request;

public class VaccineCategory_VaccineRequestDTO
{
    public int VaccineId { get; set; }
    public int VaccineCategoryId { get; set; }
    public DateTime RegisteredAt { get; set; }
}
