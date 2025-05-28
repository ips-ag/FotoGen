using FotoGen.Application.Interfaces;
using FotoGen.Application.UseCases.GetUserAvailableModel;
using FotoGen.Domain.Entities.Response;
using MediatR;

namespace FotoGen.Application.UseCases.CheckUserModelAvailable
{
    public class CheckUserModelAvailableQueryHandler : IRequestHandler<CheckUserModelAvailableQuery, BaseResponse<bool>>
    {
        private readonly IReplicateService _replicateService;
        public CheckUserModelAvailableQueryHandler(IReplicateService replicateService)
        {
            _replicateService = replicateService;
        }
        public async Task<BaseResponse<bool>> Handle(CheckUserModelAvailableQuery request, CancellationToken cancellationToken)
        {
            var response = await _replicateService.GetModelAsync(request.ModelName);
            if (response.IsSuccess) 
            {
                return BaseResponse<bool>.Success(true);
            }
            return BaseResponse<bool>.Fail(ErrorCode.GetReplicateModelFail);
        }
    }
}
