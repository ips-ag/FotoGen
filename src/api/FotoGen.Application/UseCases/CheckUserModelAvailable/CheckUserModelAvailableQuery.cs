using FotoGen.Domain.Entities.Response;
using MediatR;

namespace FotoGen.Application.UseCases.GetUserAvailableModel
{
    public class CheckUserModelAvailableQuery : IRequest<BaseResponse<bool>>
    {
        public string ModelName {  get; set; }
    }
}
