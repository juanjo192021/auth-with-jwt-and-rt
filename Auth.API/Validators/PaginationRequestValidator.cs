using Auth.API.Common.Dtos;
using FluentValidation;

namespace Auth.API.Validators
{
    public class PaginationRequestValidator: AbstractValidator<PaginationRequest>
    {

        public PaginationRequestValidator()
        {
            RuleFor(x => x.Page)
                .Cascade(CascadeMode.Stop)
                .Must(v => int.TryParse(v, out _))
                .WithMessage("El page debe contener solo números.")
                .Must(v => int.TryParse(v, out var page) && page > 0)
                .WithMessage("El page debe ser mayor que 0.")
                .When(x => !string.IsNullOrEmpty(x.Page)); ;

            RuleFor(x => x.PageSize)
                .Cascade(CascadeMode.Stop)
                .Must(v => int.TryParse(v, out _))
                .WithMessage("El pageSize debe contener solo números.")
                .Must(v => int.TryParse(v, out var size) && size > 0 && size <= 100)
                .WithMessage("El pageSize de página no debe ser mayor que 100.")
                .When(x => !string.IsNullOrEmpty(x.PageSize));

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .WithMessage("El término de búsqueda no debe exceder los 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Search));

        }
    }
}
