namespace MyVaccine.WebApi.DTOs.Response;

public class AuthResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public bool IsSuccess { get; set; }
    public string[] Errors { get; set; } = null!;
}
