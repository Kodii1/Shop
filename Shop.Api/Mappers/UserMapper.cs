using Shop.Api.Dtos;
using Shop.Api.Models;
namespace Shop.Api.Mappers;

public static class UserMapper
{

    public static UserDto ToDto(this ApplicationUser user)
    {
        return new UserDto(
            user.FirstName,
            user.LastName,
            user.Email!);
    }
    public static ApplicationUser ToModel(this UserRegisterDto userRegisterDto)
    {
        return new ApplicationUser
        {
            UserName = userRegisterDto.FirstName + userRegisterDto.LastName,
            FirstName = userRegisterDto.FirstName,
            LastName = userRegisterDto.LastName,
            Email = userRegisterDto.Email,
            NormalizedEmail = userRegisterDto.Email.ToUpper(),
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,

        };

    }
}
