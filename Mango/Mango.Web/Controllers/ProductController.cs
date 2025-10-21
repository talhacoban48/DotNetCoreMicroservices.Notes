using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    public async Task<IActionResult> ProductIndex()
    {
        List<ProductDto>? list = new();
        ResponseDto? response = await _productService.GetAllProductAsync();
        if (response != null && response.Success)
        {
            list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductDto>>(
                Newtonsoft.Json.JsonConvert.SerializeObject(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(list);
    }
    
    public async Task<IActionResult> ProductCreate()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductDto model)
    {
        if (ModelState.IsValid)
        {
            ResponseDto? response = await _productService.CreateProductAsync(model);
            if (response != null && response.Success)
            {
                TempData["success"] = "Product created successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
        }
        return View(model);
    }
    
    public async Task<IActionResult> ProductUpdate(int productId)
    {
        ResponseDto? response = await _productService.GetProductAsync(productId);
        if (response != null && response.Success)
        {
            ProductDto? model = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductDto>(
                Newtonsoft.Json.JsonConvert.SerializeObject(response.Result));
            return View(model);
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> ProductUpdate(ProductDto model)
    {
        if (ModelState.IsValid)
        {
            ResponseDto? response = await _productService.UpdateProductAsync(model);
            if (response != null && response.Success)
            {
                TempData["success"] = "Product updated successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
        }
        return View(model);
    }
    
    public async Task<IActionResult> ProductDelete(int productId)
    {
        ResponseDto? response = await _productService.GetProductAsync(productId);
        if (response != null && response.Success)
        {
            ProductDto? model = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductDto>(
                Newtonsoft.Json.JsonConvert.SerializeObject(response.Result));
            return View(model);
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> ProductDelete(ProductDto model)
    {
        ResponseDto? response = await _productService.DeleteProductAsync(model.ProductId);
        if (response != null && response.Success)
        {
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction(nameof(ProductIndex));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(model);
    }
}