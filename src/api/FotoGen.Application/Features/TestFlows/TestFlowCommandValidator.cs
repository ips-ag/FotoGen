using FluentValidation;

namespace FotoGen.Application.Features.TestFlows
{
    public sealed class TestFlowCommandValidator : AbstractValidator<TestFlowCommand>
    {
        public TestFlowCommandValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");
        } 
    }
}
