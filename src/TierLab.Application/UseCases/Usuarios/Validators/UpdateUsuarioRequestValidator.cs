using FluentValidation;
using TierLab.Application.UseCases.Usuarios.Requests;

namespace TierLab.Application.UseCases.Usuarios.Validators;

public sealed class UpdateUsuarioRequestValidator : AbstractValidator<UpdateUsuarioRequest>
{
    public UpdateUsuarioRequestValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(3)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.Username));

        RuleFor(x => x.ImageUrl)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("ImageUrl deve ser um URI válido.");

        RuleFor(x => x.Bio)
            .MaximumLength(240)
            .When(x => !string.IsNullOrWhiteSpace(x.Bio));
    }
}
