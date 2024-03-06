using System.Text.Json;
using ProductService.Services;
using ProductService.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Retrieving all products.");
        var result = await _productService.GetAll();
        if (result == null || result.Count == 0)
            return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromQuery]int id)
    {
        _logger.LogInformation($"Retrieving product with id = {id}.");
        var result = await _productService.Get(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductDto product)
    {
        _logger.LogInformation($"Saving new product with DTO: {product.ToString()}.");
        await _productService.Save(product);
        return Ok();
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] ProductDto product)
    {
        _logger.LogInformation($"Updating product with DTO: {product.ToString()}.");
        await _productService.Update(product);
        return Ok();
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] ProductDto product)
    {
        _logger.LogInformation($"Deleting product with DTO: {product.ToString()}.");
        await _productService.Delete(product);
        return Ok();
    }
}
