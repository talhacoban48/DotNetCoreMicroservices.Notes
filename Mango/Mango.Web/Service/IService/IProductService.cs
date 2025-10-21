using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface IProductService
{
    Task<ResponseDto?> GetProductAsync(int productId);
    Task<ResponseDto?> GetAllProductAsync();
    Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
    Task<ResponseDto?> UpdateProductAsync(ProductDto productDto);
    Task<ResponseDto?> DeleteProductAsync(int productId);
}