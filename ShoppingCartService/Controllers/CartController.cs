using System.Text.Json;
using ShoppingCartService.Services;
using ShoppingCartService.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartService.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly ILogger<CartController> _logger;
    private readonly ICartService _cartService;

    public CartController(ILogger<CartController> logger, ICartService cartService)
    {
        _logger = logger;
        _cartService = cartService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Retrieving all carts.");
        var result = await _cartService.GetAll();
        if (result == null || result.Count == 0)
            return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromQuery]int id)
    {
        _logger.LogInformation($"Retrieving cart with id = {id}.");
        var result = await _cartService.Get(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetByUserId([FromQuery]int userId)
    {
        _logger.LogInformation($"Retrieving cart for userId = {userId}.");
        var result = await _cartService.GetByUser(userId);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CartDto cart)
    {
        _logger.LogInformation($"Saving new cart with DTO: {cart.ToString()}.");
        await _cartService.Save(cart);
        return Ok();
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] CartDto cart)
    {
        _logger.LogInformation($"Updating cart with DTO: {cart.ToString()}.");
        await _cartService.Update(cart);
        return Ok();
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] CartDto cart)
    {
        _logger.LogInformation($"Deleting cart with DTO: {cart.ToString()}.");
        await _cartService.Delete(cart);
        return Ok();
    }
}
