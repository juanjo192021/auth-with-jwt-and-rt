using Auth.Application.DTOs.Auth;
using Auth.Application.DTOs.User;
using Auth.Domain.Entities;
using Mapster;

namespace Auth.Application.Mappings
{
    public class UserMapping: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // De un User a un UserDTO
            config.NewConfig<User, UserDto>();

            config.NewConfig<UserDto, User>();

            // TSource a TDestination
            config.NewConfig<SignupDto,User>();
        }
    }
}
