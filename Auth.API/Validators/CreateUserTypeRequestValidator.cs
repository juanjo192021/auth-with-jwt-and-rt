using Auth.API.Contracts.Requests.UserType;
using FluentValidation;
using System.Data;

namespace Auth.API.Validators
{
    public class CreateUserTypeRequestValidator: AbstractValidator<CreateUserTypeRequest>
    {
        public CreateUserTypeRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre no puede estar vacío.")
                .NotNull().WithMessage("El nombre no puede ser null")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");


            RuleFor(x => x.Description)
                .MinimumLength(5).WithMessage("La descripción debe tener al menos 5 caracteres.")
                .MaximumLength(200).WithMessage("La descripción no puede superar los 200 caracteres.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("El estado no puede estar vacío.");
        }
    }
}
