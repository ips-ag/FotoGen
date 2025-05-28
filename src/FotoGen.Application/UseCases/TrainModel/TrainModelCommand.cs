using FotoGen.Common;
using MediatR;

namespace FotoGen.Application.UseCases.TrainModel
{
    public class TrainModelCommand : IRequest<BaseResponse<TrainModelResponse>>
    {
        public string Name { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string UserEmail {  get; set; } = default!;
        public string InputImageUrl { get; set; } = default!;
    }
}
