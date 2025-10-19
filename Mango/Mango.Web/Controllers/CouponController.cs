using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers;

public class CouponController : Controller
{
    
    private readonly ICouponService _couponService;

    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }
    
    public async Task<IActionResult> CouponIndex()
    {
        List<CouponDto>? list = new();
        ResponseDto? response = await _couponService.GetAllCouponAsync();
        if (response != null && response.Success)
        {
            list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CouponDto>>(
                Newtonsoft.Json.JsonConvert.SerializeObject(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(list);
    }
    
    public async Task<IActionResult> CouponCreate()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CouponCreate(CouponDto model)
    {
        if (ModelState.IsValid)
        {
            ResponseDto? response = await _couponService.CreateCouponAsync(model);
            if (response != null && response.Success)
            {
                TempData["success"] = "Coupon created successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
        }
        return View(model);
    }
    
    public async Task<IActionResult> CouponDelete(int couponId)
    {
        ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);
        if (response != null && response.Success)
        {
            CouponDto? model = Newtonsoft.Json.JsonConvert.DeserializeObject<CouponDto>(
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
    public async Task<IActionResult> CouponDelete(CouponDto model)
    {
        ResponseDto? response = await _couponService.DeleteCouponAsync(model.CoupunId);
        if (response != null && response.Success)
        {
            TempData["success"] = "Coupon deleted successfully";
            return RedirectToAction(nameof(CouponIndex));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(model);
    }
}