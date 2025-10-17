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

}
