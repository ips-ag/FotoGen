using FluentValidation;
using FotoGen.Common;
using MediatR;

namespace FotoGen.Application.Features.TestFlows
{
    public class TestFlowCommandHandler : IRequestHandler<TestFlowCommand, BaseResponse<bool>>
    {
        private readonly IValidator<TestFlowCommand> _validator;
        public TestFlowCommandHandler(IValidator<TestFlowCommand> validator)
        {
            _validator = validator;
        }

        public async Task<BaseResponse<bool>> Handle(TestFlowCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return BaseResponse<bool>.Fail(validationResult.ToDictionary());
            }
            return BaseResponse<bool>.Success(true);
        }
    }
}
