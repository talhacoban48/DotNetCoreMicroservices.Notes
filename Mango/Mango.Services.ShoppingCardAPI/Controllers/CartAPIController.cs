using AutoMapper;
using Mango.Services.ShoppingCardAPI.Data;
using Mango.Services.ShoppingCardAPI.Models;
using Mango.Services.ShoppingCardAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCardAPI.Controllers;

[Route("api/cart")]
[ApiController]
public class CartAPIController : ControllerBase
{
    private ResponseDto _response;
    private IMapper _mapper;
    private readonly AppDbContext _db;

    public CartAPIController(
        IMapper mapper,
        AppDbContext db)
    {
        _response = new ResponseDto();
        _mapper = mapper;
        _db = db;
    }
    
    [HttpGet("GetCard/{userId}")]
    public async Task<ResponseDto> GetCard(string userId)
    {
        try
        {
            CartDto cart = new()
            {
                CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(u => u.UserId == userId)),
            };
            cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails
                .Where(u => u.CartDetailsId == cart.CartHeader.CartHeaderId));

            foreach (var item in cart.CartDetails)
            {
                cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
            }
            
            _response.Result = cart;
        }
        catch (Exception e)
        {
            _response.Message = e.Message;
            _response.Success = false;
        }
        return _response;
    }

    [HttpPost("CartUpsert")]
    public async Task<ResponseDto> CartUpsert([FromBody] CartDto cartDto)
    {
        try
        {
            var cartHeaderFromDb = await _db.CartHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);

            if (cartHeaderFromDb == null)
            {
                // create header and details
                var cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                _db.CartHeaders.Add(cartHeader);
                await _db.SaveChangesAsync();

                cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                await _db.SaveChangesAsync();
            }
            else
            {
                // if header exists, check if same product exists
                var cartDetailsFromDb = await _db.CartDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u =>
                        u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                    cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                    _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
            }

            _response.Result = cartDto;
        }
        catch (Exception e)
        {
            _response.Message = e.Message;
            _response.Success = false;
        }

        return _response;
    }
    
    [HttpPost("RemoveCard")]
    public async Task<ResponseDto> RemoveCard([FromBody] int cartDetailsId)
    {
        try
        {
            CartDetails? cartDetails = _db.CartDetails
                .FirstOrDefault(u => u.CartDetailsId == cartDetailsId);
            
            int totalCountofCartItem = _db.CartDetails.Count(u => u.CartHeaderId == cartDetails.CartHeaderId);

            _db.CartDetails.Remove(cartDetails);

            if (totalCountofCartItem == 1)
            {
                var cartHeaderToRemove = await _db.CartHeaders
                    .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                if (cartHeaderToRemove != null)
                    _db.CartHeaders.Remove(cartHeaderToRemove);
            }

            await _db.SaveChangesAsync();

            await _db.SaveChangesAsync();
            _response.Result = true;
        }
        catch (Exception e)
        {
            _response.Message = e.Message;
            _response.Success = false;
        }

        return _response;
    }
    
}