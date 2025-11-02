using Auth.API.Contracts.Requests;
using FluentValidation;

namespace Auth.API.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("Debe ser un correo válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(4).WithMessage("La contraseña debe tener al menos 4 caracteres.");
        }
    }
}
