using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;

namespace Mango.Services.ProductAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductDto>();
            cfg.CreateMap<ProductDto, Product>();
        }, LoggerFactory.Create(builder => 
        {
            builder.AddConsole();
        }));
    }
}