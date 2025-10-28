using Auth.Domain.Entities;
using Auth.Application.DTOs.UserType;
using Mapster;

namespace Auth.Application.Mappings
{
    public class UserTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserType, UserTypeDto>();
        }
    }
}
