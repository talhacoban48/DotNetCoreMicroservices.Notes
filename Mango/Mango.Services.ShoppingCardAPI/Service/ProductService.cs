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
    
    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        var client = _httpClientFactory.CreateClient("Product");
        var response = await client.GetAsync("api/product");
        var apiContent = await response.Content.ReadAsStringAsync();
        var resp = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        if (resp.Success)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
        }
        return new List<ProductDto>();
    }
}