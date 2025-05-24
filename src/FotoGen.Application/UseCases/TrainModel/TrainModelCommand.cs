using FotoGen.Common;
using MediatR;

namespace FotoGen.Application.UseCases.TrainModel
{
    public class TrainModelCommand : IRequest<BaseResponse<TrainModelResponse>>
    {
        public string Owner { get; set; } = default!;
        public string Name { get; set; }
        public string TriggerWords { get; set; } = default!;
        public string InputImageUrl { get; set; } = default!;
    }
}
