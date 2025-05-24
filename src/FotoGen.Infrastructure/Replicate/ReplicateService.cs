using System.Text;
using System.Text.Json;
using FotoGen.Application.Interfaces;
using FotoGen.Common;
using FotoGen.Common.Contracts.Replicated.CreateModel;
using FotoGen.Common.Contracts.Replicated.TrainModel;
using FotoGen.Infrastructure.Replicate.CreateModel;
using FotoGen.Infrastructure.Replicate.TrainModel;
using FotoGen.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace FotoGen.Infrastructure.Replicate
{
    public class ReplicateService : IReplicateService
    {
        private readonly ReplicateSetting _replicateSetting;
        private readonly HttpClient _httpClient;
        public ReplicateService(IOptions<ReplicateSetting> replicateSetting, HttpClient httpClient)
        {
            _replicateSetting = replicateSetting.Value;
            _httpClient = httpClient;
        }
        public async Task<BaseResponse<bool>> CreateReplicateModelAsync(CreateReplicateModelRequestDto dto)
        {
            var requestModel = CreateModelMapper.ToRequest(dto, _replicateSetting);
            var json = JsonSerializer.Serialize(requestModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/models", content);
            if (!response.IsSuccessStatusCode)
            {
                return BaseResponse<bool>.Fail(ErrorCode.CreateReplicateModelFail);
            }
            return BaseResponse<bool>.Success(true);
        }

        public async Task<BaseResponse<TrainModelResponseDto>> TrainModelAsync(TrainModelRequestDto request)
        {
            var postUrl = $"/models/{_replicateSetting.Model}/versions/{_replicateSetting.Version}";
            var requestModel = TrainModelMapper.ToRequest(request, _replicateSetting);
            var json = JsonSerializer.Serialize(requestModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(postUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                return BaseResponse<TrainModelResponseDto>.Fail(ErrorCode.CreateReplicateModelFail);
            }
            var contentResponse = await response.Content.ReadAsStringAsync();
            var trainModelResponse = JsonSerializer.Deserialize<TrainModelResponse>(contentResponse);
            return BaseResponse<TrainModelResponseDto>.Success(TrainModelMapper.ToResponseDto(trainModelResponse));
        }
    }
}
