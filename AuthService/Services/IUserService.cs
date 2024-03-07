using AuthService.Services.Dto;

namespace AuthService.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAll();
    Task<UserDto> Get(int id);
    Task Save(UserDto user);
    Task Update(UserDto user);
    Task Delete(UserDto user);
    Task<UserSimpleDto> Login(string loginOrEmail, string password);
}