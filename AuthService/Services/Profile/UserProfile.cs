using AuthService.Domain;
using AuthService.Services.Dto;

namespace AuthService.Services.Profile;

public class UserProfile : AutoMapper.Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserSimpleDto>();
    }
}