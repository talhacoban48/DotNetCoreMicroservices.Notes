using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers;

[Route("/api/coupon")]
[ApiController]
[Authorize]
public class CouponAPIController : Controller
{
    private readonly AppDbContext _db;
    private ResponseDto _response;
    private IMapper _mapper;

    public CouponAPIController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _response = new ResponseDto();
        _mapper = mapper;
    }

    [HttpGet]
    public ResponseDto Get()
    {
        try
        {
            IEnumerable<Coupon> coupons = _db.Coupons.ToList();
            _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }
        catch (Exception ex)
        {
            _response.Success = false;
            _response.Message = ex.Message;
        }
        return _response;
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public ResponseDto Get(int id)
    {
        try
        {
            Coupon coupon = _db.Coupons.First(u => u.CoupunId == id);
            _response.Result = _mapper.Map<CouponDto>(coupon);
        }
        catch (Exception ex)
        {
            _response.Success = false;
            _response.Message = ex.Message;
        }
        return _response;
    }
    
    [HttpGet]
    [Route("GetByCode/{code}")]
    public ResponseDto GetByCode(string code)
    {
        try
        {
            Coupon coupon = _db.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower());
            _response.Result = _mapper.Map<CouponDto>(coupon);
        }
        catch (Exception ex)
        {
            _response.Success = false;
            _response.Message = ex.Message;
        }
        return _response;
    }
    
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto Post([FromBody] CouponDto couponDto)
    {
        try
        {
            Coupon coupon = _mapper.Map<Coupon>(couponDto);
            _db.Coupons.Add(coupon);
            _db.SaveChanges();
            _response.Result = _mapper.Map<CouponDto>(coupon);
        }
        catch (Exception ex)
        {
            _response.Success = false;
            _response.Message = ex.Message;
        }
        return _response;
    }
    
    [HttpPut]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto Put([FromBody] CouponDto couponDto)
    {
        try
        {
            Coupon coupon = _mapper.Map<Coupon>(couponDto);
            _db.Coupons.Update(coupon);
            _db.SaveChanges();
            _response.Result = _mapper.Map<CouponDto>(coupon);
        }
        catch (Exception ex)
        {
            _response.Success = false;
            _response.Message = ex.Message;
        }
        return _response;
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto Delete(int id)
    {
        try
        {
            Coupon coupon = _db.Coupons.First(u => u.CoupunId == id);
            _db.Coupons.Remove(coupon);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            _response.Success = false;
            _response.Message = ex.Message;
        }
        return _response;
    }
}