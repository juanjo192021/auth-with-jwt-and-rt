using Auth.Application.DTOs.Auth;
using Auth.Application.DTOs.User;
using Auth.Domain.Entities;
using Mapster;

namespace Auth.Application.Mappings
{
    public class UserMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // De un User a un UserDTO
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.UserType, src => src.UserType.Name)
                .Map(dest => dest.Roles, src => src.UserRoles
                .Where(ur => ur.IsActive)
                .Select(ur => ur.Role.Name)
                .ToList()); ;

            config.NewConfig<UserDto, User>();

            // TSource a TDestination
            config.NewConfig<SignupDto, User>().Map(dest => dest.UserTypeId, src => src.UserTypeId);
        }
    }
}
