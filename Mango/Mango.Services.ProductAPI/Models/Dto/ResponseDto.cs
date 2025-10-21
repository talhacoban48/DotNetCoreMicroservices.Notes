namespace Mango.Services.ProductAPI.Models.Dto;

public class ResponseDto
{
    public object? Result { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}