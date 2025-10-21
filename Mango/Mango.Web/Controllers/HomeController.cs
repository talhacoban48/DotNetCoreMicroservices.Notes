using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;

namespace Mango.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
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
    
    [Authorize]
    public async Task<IActionResult> ProductDetails(int productId)
    {
        ProductDto? product = new();
        ResponseDto? response = await _productService.GetProductAsync(productId);
        if (response != null && response.Success)
        {
            product = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductDto>(
                Newtonsoft.Json.JsonConvert.SerializeObject(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(product);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}