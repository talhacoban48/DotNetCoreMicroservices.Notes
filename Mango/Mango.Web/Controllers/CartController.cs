using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Mango.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        return View(await LoadCartDtoBasedOnLoggedInUser());
    }
    
    [Authorize]
    public async Task<IActionResult> Remove(int cartDetailsId)
    {
        var userId = User.Claims.Where(u=> u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        ResponseDto? response = await _cartService.RemoveFromCartAsync(cartDetailsId);
        if (response != null && response.Success && response.Result != null)
        {
            TempData["success"] = "Item has been removed to shopping cart.";
            return RedirectToAction(nameof(CartIndex));
        }

        return View(nameof(CartIndex));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
    {
        ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);
        if (response != null && response.Success && response.Result != null)
        {
            TempData["success"] = "Coupon has been applied.";
            return RedirectToAction(nameof(CartIndex));
        }

        return View(nameof(CartIndex));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
    {
        cartDto.CartHeader.CouponCode = string.Empty;
        ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);
        if (response != null && response.Success && response.Result != null)
        {
            TempData["success"] = "Coupon has been applied.";
            return RedirectToAction(nameof(CartIndex));
        }

        return View(nameof(CartIndex));
    }
    
    private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
    {
        var userId = User.Claims.Where(u=> u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);
        if (response != null && response.Success && response.Result != null)
        {
            CartDto cartDto = Newtonsoft.Json.JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            return cartDto;
        }
        return new CartDto();
    }
}