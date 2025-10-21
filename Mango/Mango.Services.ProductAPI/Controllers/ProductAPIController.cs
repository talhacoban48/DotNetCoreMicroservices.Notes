using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers;


[Route("/api/product")]
[ApiController]
[Authorize]
public class ProductAPIController : ControllerBase
{
    private readonly AppDbContext _db;
    private ResponseDto _response;
    private IMapper _mapper;
    
    public ProductAPIController(
        AppDbContext db,
        IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
        _response = new ResponseDto();
    }
    
    [HttpGet]
    public ResponseDto Get()
    {
        try
        {
            IEnumerable<Product> products = _db.Products.ToList();
            _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
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
            Product product = _db.Products.First(u => u.ProductId == id);
            _response.Result = _mapper.Map<ProductDto>(product);
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
    public ResponseDto Post([FromBody] ProductDto productDto)
    {
        try
        {
            Product product = _mapper.Map<Product>(productDto);
            _db.Products.Add(product);
            _db.SaveChanges();
            _response.Result = _mapper.Map<ProductDto>(product);
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
    public ResponseDto Put([FromBody] ProductDto productDto)
    {
        try
        {
            Product product = _mapper.Map<Product>(productDto);
            _db.Products.Update(product);
            _db.SaveChanges();
            _response.Result = _mapper.Map<ProductDto>(product);
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
            Product coupon = _db.Products.First(u => u.ProductId == id);
            _db.Products.Remove(coupon);
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