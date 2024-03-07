using System.Text.Json;
using AuthService.Services;
using AuthService.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IUserService _userService;

    public AuthController(ILogger<AuthController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login(string loginOrEmail, string password)
    {
        _logger.LogInformation($"logging in user: login/email:{loginOrEmail} - password: {password}");
        var result = await _userService.Login(loginOrEmail, password);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Retrieving all users.");
        var result = await _userService.GetAll();
        if (result == null || result.Count == 0)
            return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromQuery]int id)
    {
        _logger.LogInformation($"Retrieving user with id = {id}.");
        var result = await _userService.Get(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] UserDto user)
    {
        _logger.LogInformation($"Saving new user with DTO: {user.ToString()}.");
        await _userService.Save(user);
        return Ok();
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UserDto user)
    {
        _logger.LogInformation($"Updating user with DTO: {user.ToString()}.");
        await _userService.Update(user);
        return Ok();
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] UserDto user)
    {
        _logger.LogInformation($"Deleting user with DTO: {user.ToString()}.");
        await _userService.Delete(user);
        return Ok();
    }
}
