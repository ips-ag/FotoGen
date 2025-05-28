using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Response;
using MediatR;

namespace FotoGen.Application.UseCases.GeneratePhoto
{
    public class GeneratePhotoCommandHandler : IRequestHandler<GeneratePhotoCommand, BaseResponse<GeneratePhotoResponse>>
    {
        private readonly IReplicateService _replicateService;
        private readonly IDownloadClient _downloadClient;
        public GeneratePhotoCommandHandler(IReplicateService replicateService, IDownloadClient downloadClient) 
        {
            _replicateService = replicateService;
            _downloadClient = downloadClient;
        }
        public async Task<BaseResponse<GeneratePhotoResponse>> Handle(GeneratePhotoCommand request, CancellationToken cancellationToken)
        {
            var replicateReponse = await _replicateService.GeneratePhotoAsync(request.Prompt, request.ModelName);
            if (replicateReponse.IsSuccess)
            {
                var bytesImage = await _downloadClient.GetByteArrayAsync(replicateReponse.Data.StreamUrl);
                if (bytesImage.Length == 0)
                {
                    return BaseResponse<GeneratePhotoResponse>.Fail(ErrorCode.ImageGenerationResponseEmpty);
                }
                var base64Image = Convert.ToBase64String(bytesImage);
                var result = new GeneratePhotoResponse { Base64Image = base64Image, OutputFormat = replicateReponse.Data.OutputFormat };
                return BaseResponse<GeneratePhotoResponse>.Success(result);
            }
            return BaseResponse<GeneratePhotoResponse>.Fail(ErrorCode.GeneratePhotoFail);
        }
    }
}
