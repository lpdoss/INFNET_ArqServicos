using System.Text.Json;
using PaymentService.Services;
using PaymentService.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public OrderController(ILogger<OrderController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Retrieving all orders.");
        var result = await _orderService.GetAll();
        if (result == null || result.Count == 0)
            return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromQuery]int id)
    {
        _logger.LogInformation($"Retrieving order with id = {id}.");
        var result = await _orderService.Get(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetByUserId([FromQuery]int userId)
    {
        _logger.LogInformation($"Retrieving order for userId = {userId}.");
        var result = await _orderService.GetByUser(userId);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] OrderDto order)
    {
        _logger.LogInformation($"Saving new order with DTO: {order.ToString()}.");
        await _orderService.Save(order);
        return Ok();
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] OrderDto order)
    {
        _logger.LogInformation($"Updating order with DTO: {order.ToString()}.");
        await _orderService.Update(order);
        return Ok();
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] OrderDto order)
    {
        _logger.LogInformation($"Deleting order with DTO: {order.ToString()}.");
        await _orderService.Delete(order);
        return Ok();
    }
}
