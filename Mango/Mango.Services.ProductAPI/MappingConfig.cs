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
            cfg.CreateMap<ProductDto, Product>();
            cfg.CreateMap<Product, ProductDto>();
        }, LoggerFactory.Create(builder => 
        {
            builder.AddConsole();
        }));
    }
}