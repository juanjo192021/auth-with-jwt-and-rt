using Auth.Domain.Entities;
using Auth.Application.DTOs.UserType;
using Mapster;

namespace Auth.Application.Mappings
{
    public class UserTypeMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // De un UserType a un UserTypeDTO
            config.NewConfig<UserType, UserTypeDto>();

            // De un CreateUserTypeDTO a un UserType
            config.NewConfig<CreateUserTypeDto, UserType>();


            config.NewConfig<UpdateUserTypeDto, UserType>().IgnoreNullValues(true);
        }
    }
}
