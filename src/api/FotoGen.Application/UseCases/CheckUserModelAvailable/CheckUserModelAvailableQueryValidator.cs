using FluentValidation;
using FotoGen.Application.UseCases.GetUserAvailableModel;

namespace FotoGen.Application.UseCases.CheckUserModelAvailable
{
    public class CheckUserModelAvailableQueryValidator : AbstractValidator<CheckUserModelAvailableQuery>
    {
        public CheckUserModelAvailableQueryValidator()
        {
            RuleFor(x => x.ModelName)
                .NotEmpty().WithMessage("Model name is required");
        }
    }
}
