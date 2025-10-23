using AutoMapper;
using Mango.Services.ShoppingCardAPI.Models;
using Mango.Services.ShoppingCardAPI.Models.Dto;

namespace Mango.Services.ShoppingCardAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            cfg.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        }, LoggerFactory.Create(builder => 
        {
            builder.AddConsole();
        }));
    }
}