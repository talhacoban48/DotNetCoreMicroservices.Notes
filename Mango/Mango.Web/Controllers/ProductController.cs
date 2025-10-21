using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers;

public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}