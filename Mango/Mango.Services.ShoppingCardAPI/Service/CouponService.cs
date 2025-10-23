using Mango.Services.ShoppingCardAPI.Models.Dto;
using Mango.Services.ShoppingCardAPI.Service.IService;

namespace Mango.Services.ShoppingCardAPI.Service;

public class CouponService: ICouponService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CouponService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    
    public async Task<CouponDto> GetCoupon(string couponCode)
    {
        var client = _httpClientFactory.CreateClient("Coupon");
        var response = await client.GetAsync($"api/coupon/GetByCode/{couponCode}");
        var apiContent = await response.Content.ReadAsStringAsync();
        var resp = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        if (resp.Success)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
        }
        return new CouponDto();
    }
}