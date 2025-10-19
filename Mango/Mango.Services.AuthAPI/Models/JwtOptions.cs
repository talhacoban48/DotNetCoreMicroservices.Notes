namespace Mango.Services.AuthAPI.Models;

public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;  // Token’ı Yayımlayan
    public string Audience { get; set; } = string.Empty;  // Token’ı Kullanacak Olan
    public string SecretKey { get; set; } = string.Empty;  // Gizli Anahtar
}