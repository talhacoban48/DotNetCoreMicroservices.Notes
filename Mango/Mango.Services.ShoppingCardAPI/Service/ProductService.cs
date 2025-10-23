using Mango.Services.ShoppingCardAPI.Models.Dto;
using Mango.Services.ShoppingCardAPI.Service.IService;

namespace Mango.Services.ShoppingCardAPI.Service;

public class ProductService: IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public Task<IEnumerable<ProductDto>> GetProducts()
    {
        throw new NotImplementedException();
    }
}