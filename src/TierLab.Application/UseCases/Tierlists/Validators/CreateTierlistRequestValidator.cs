using FluentValidation;
using TierLab.Application.UseCases.Tierlists.Requests;

namespace TierLab.Application.UseCases.Tierlists.Validators;

public sealed class CreateTierlistRequestValidator : AbstractValidator<CreateTierlistRequest>
{
    public CreateTierlistRequestValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Descricao));

        RuleFor(x => x.ImageUrl)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("ImageUrl deve ser um URI válido.");

        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("UsuarioId é obrigatório.");

        RuleFor(x => x.Visibilidade)
            .Must(value => string.IsNullOrWhiteSpace(value) || value == "public" || value == "private")
            .WithMessage("Visibilidade deve ser 'public' ou 'private'.");
    }
}
