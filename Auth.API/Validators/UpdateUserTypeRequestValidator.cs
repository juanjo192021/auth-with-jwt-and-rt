using Auth.API.Contracts.Requests.UserType;
using FluentValidation;

namespace Auth.API.Validators
{
    public class UpdateUserTypeRequestValidator: AbstractValidator<UpdateUserTypeRequest>
    {
        public UpdateUserTypeRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El id debe ser mayor que uno");

            Include(new CreateUserTypeRequestValidator()); // reutiliza las reglas de creación
        }
    }
}
