using Auth.API.Common.Dtos;
using FluentValidation;

namespace Auth.API.Validators
{
    public class PaginationRequestValidator: AbstractValidator<PaginationRequest>
    {

        public PaginationRequestValidator()
        {
            //ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Page)
                .InclusiveBetween(1, 10000).WithMessage("El número de página debe estar entre 1 y 10,000.")
                .GreaterThan(0)
                .When(x => x.Page != default)
                .WithMessage("La página debe ser mayor que 0.");

            RuleFor(x => x.PageSize).Cascade(CascadeMode.Stop)
                .LessThanOrEqualTo(100).When(x => x.Page != default).WithMessage("El tamaño de página no debe ser mayor que 100.");

            RuleFor(x => x.Search)
                .MaximumLength(100).WithMessage("El término de búsqueda no debe exceder los 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Search));

        }
    }
}
