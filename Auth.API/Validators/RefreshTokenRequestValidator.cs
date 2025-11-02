using Auth.API.Contracts.Requests;
using FluentValidation;

namespace Auth.API.Validators
{
    public class RefreshTokenRequestValidator: AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("El token es obligatorio.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("El refresh token es obligatorio.");
        }
    }
}
