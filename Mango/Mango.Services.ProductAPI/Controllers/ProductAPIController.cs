using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers;

public class ProductAPIController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}