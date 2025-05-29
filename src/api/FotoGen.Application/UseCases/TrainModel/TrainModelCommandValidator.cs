using FluentValidation;

namespace FotoGen.Application.UseCases.TrainModel
{
    public class TrainModelCommandValidator : AbstractValidator<TrainModelCommand>
    {
        public TrainModelCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.UserEmail)
                .NotEmpty().EmailAddress().WithMessage("Email is required");
            RuleFor(x => x.ModelName)
                .NotEmpty().WithMessage("Model name is required");
        }
    }
}
