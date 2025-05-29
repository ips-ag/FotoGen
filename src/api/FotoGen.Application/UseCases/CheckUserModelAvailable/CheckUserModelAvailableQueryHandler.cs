using FluentValidation;
using FotoGen.Application.Interfaces;
using FotoGen.Application.UseCases.GetUserAvailableModel;
using FotoGen.Domain.Entities.Response;
using MediatR;

namespace FotoGen.Application.UseCases.CheckUserModelAvailable
{
    public class CheckUserModelAvailableQueryHandler : IRequestHandler<CheckUserModelAvailableQuery, BaseResponse<bool>>
    {
        private readonly IReplicateService _replicateService;
        private readonly IValidator<CheckUserModelAvailableQuery> _validator;
        public CheckUserModelAvailableQueryHandler(IReplicateService replicateService, 
            IValidator<CheckUserModelAvailableQuery> validator)
        {
            _replicateService = replicateService;
            _validator = validator;
        }
        public async Task<BaseResponse<bool>> Handle(CheckUserModelAvailableQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BaseResponse<bool>.Fail(validationResult.ToDictionary());
            }
            var response = await _replicateService.GetModelAsync(request.ModelName);
            if (response.IsSuccess) 
            {
                return BaseResponse<bool>.Success(true);
            }
            return BaseResponse<bool>.Fail(ErrorCode.GetReplicateModelFail);
        }
    }
}
