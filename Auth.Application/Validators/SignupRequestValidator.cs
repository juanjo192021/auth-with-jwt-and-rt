using Auth.Application.DTOs.Auth;
using FluentValidation;

namespace Auth.Application.Validators
{
    public class SignupRequestValidator : AbstractValidator<SignupDto>
    {
        public SignupRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("Debe ser un correo válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(4).WithMessage("La contraseña debe tener al menos 4 caracteres.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MinimumLength(2).WithMessage("El apellido debe tener al menos 2 caracteres.");

            // TODO: Validar la imagen cuando se implemente la carga de archivos

            RuleFor(x => x.DocumentType)
                .NotEmpty().WithMessage("El tipo de documento es obligatorio.")
                .Must(dt => dt == "DNI" || dt == "CE" || dt == "Pasaporte");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("El número de documento es obligatorio.")
                .MinimumLength(8).WithMessage("El número de documento debe tener al menos 8 caracteres.")
                .MaximumLength(15).WithMessage("El número de documento no puede tener más de 15 caracteres.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria.")
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("La fecha de nacimiento debe ser una fecha pasada.");

            RuleFor(x => x.Phone)
                .MinimumLength(8).WithMessage("El número de teléfono debe tener al menos 8 caracteres.")
                .MaximumLength(15).WithMessage("El número de teléfono no puede tener más de 15 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Mobile)
                .NotEmpty().WithMessage("El número de móvil es obligatorio.")
                .MinimumLength(8).WithMessage("El número de móvil debe tener al menos 8 caracteres.")
                .MaximumLength(15).WithMessage("El número de móvil no puede tener más de 15 caracteres.");

            RuleFor(x => x.Gender)
                .Must(g => g == null || g == "Masculino" || g == "Femenino")
                .WithMessage("El género debe ser 'Masculino' o 'Femenino'.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("La dirección es obligatoria.");

            RuleFor(x => x.UserTypeId)
                .NotEmpty().WithMessage("El tipo de usuario es obligatorio.")
                .GreaterThan(0).WithMessage("El tipo de usuario debe ser mayor que 0.");

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("Debe asignar al menos un rol al usuario.");
                

        }
    }
}
