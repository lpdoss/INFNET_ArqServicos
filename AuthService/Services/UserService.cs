using AuthService.Domain;
using AuthService.Repositories;
using AuthService.Services.Dto;
using AutoMapper;

namespace AuthService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<List<UserDto>> GetAll()
    {
        var result = await _userRepository.GetAll();
        return _mapper.Map<List<UserDto>>(result);
    }
    public async Task<UserDto> Get(int id)
    {
        var result = await _userRepository.Get(id);
        return _mapper.Map<UserDto>(result);
    }
    public async Task Save(UserDto userDto)
    {
        if (userDto.Id > 0)
            throw new ArgumentException("User already has id.");
        var user = _mapper.Map<User>(userDto);
        await _userRepository.Save(user);
    }

    public async Task Update(UserDto userDto)
    {
        if (userDto.Id <= 0)
            throw new ArgumentException("User has no id.");
        var user = _mapper.Map<User>(userDto);
        await _userRepository.Update(user);
    }
    public async Task Delete(UserDto userDto)
    {
        if (userDto.Id <= 0)
            throw new ArgumentException("User is invalid.");
        var user = _mapper.Map<User>(userDto);
        await _userRepository.Delete(user);
    }

    public async Task<UserSimpleDto> Login(string loginOrEmail, string password)
    {
        if (string.IsNullOrEmpty(loginOrEmail) || string.IsNullOrEmpty(password))
            throw new ArgumentException("Login/Email and Password cannot be empty.");

        var user = await _userRepository.GetOneByCriteria(a => (a.Login == loginOrEmail || a.Email == loginOrEmail) && a.Password == password);
        if (user == null)
            throw new ArgumentException("Incorret Login/Email or Password. Please try again...");
        return _mapper.Map<UserSimpleDto>(user);
    }
}
