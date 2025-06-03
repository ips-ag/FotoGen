using FluentValidation;

namespace FotoGen.Application.UseCases.GeneratePhoto;

public class GeneratePhotoCommandValidator : AbstractValidator<GeneratePhotoCommand>
{
    public GeneratePhotoCommandValidator()
    {
        RuleFor(x => x.Prompt)
            .NotEmpty().WithMessage("Prompt is required");
    }
}
