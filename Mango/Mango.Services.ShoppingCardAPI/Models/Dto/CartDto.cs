namespace Mango.Services.ShoppingCardAPI.Models.Dto;

public class CartDto
{
    public CartHeaderDto Catdheader { get; set; }
    public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
}