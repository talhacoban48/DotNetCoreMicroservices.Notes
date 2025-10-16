using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CouponDto, Coupon>();
            cfg.CreateMap<Coupon, CouponDto>();
        }, LoggerFactory.Create(builder => 
        {
            builder.AddConsole();
        }));
    }
}