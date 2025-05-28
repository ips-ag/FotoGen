using System.Text;
using System.Text.Json;
using FotoGen.Application.Interfaces;
using FotoGen.Common;
using FotoGen.Common.Contracts.Replicated.CreateModel;
using FotoGen.Common.Contracts.Replicated.GetTrainedModelStatus;
using FotoGen.Common.Contracts.Replicated.TrainModel;
using FotoGen.Common.Contracts.Replicated.UseModel;
using FotoGen.Infrastructure.Replicate.CreateModel;
using FotoGen.Infrastructure.Replicate.GetTrainModelStatus;
using FotoGen.Infrastructure.Replicate.TrainModel;
using FotoGen.Infrastructure.Replicate.UseModel;
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
            var response = await _httpClient.PostAsync("models", content);
            if (!response.IsSuccessStatusCode)
            {
                return BaseResponse<bool>.Fail(ErrorCode.CreateReplicateModelFail);
            }
            return BaseResponse<bool>.Success(true);
        }

        public async Task<BaseResponse<UseModelResponseDto>> GeneratePhotoAsync(string prompt, string modelName)
        {
            var input = UseModelMapper.ToInput(prompt, modelName, _replicateSetting);
            var json = JsonSerializer.Serialize(input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("predictions", content);
            if (!response.IsSuccessStatusCode)
            {
                return BaseResponse<UseModelResponseDto>.Fail(ErrorCode.GeneratePhotoFail);
            }
            var contentResponse = await response.Content.ReadAsStringAsync();
            var useModelResponse = JsonSerializer.Deserialize<UseModelResponse>(contentResponse);
            return BaseResponse<UseModelResponseDto>.Success(UseModelMapper.ToResponseDto(useModelResponse));
        }

        public async Task<BaseResponse<bool>> GetModelAsync(string name)
        {
            var url = $"models/{_replicateSetting.Owner}/{name}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return BaseResponse<bool>.Fail(ErrorCode.GetReplicateModelFail);
            }
            return BaseResponse<bool>.Success(true);
        }

        public async Task<BaseResponse<GetTrainedModelStatusResponseDto>> GetTrainModelStatusAsync(string trainModelId)
        {
            var getUrl = $"trainings/{trainModelId}";
            var response = await _httpClient.GetAsync(getUrl);
            if (!response.IsSuccessStatusCode)
            {
                return BaseResponse<GetTrainedModelStatusResponseDto>.Fail(ErrorCode.GetReplicateTrainModelFail);
            }
            var contentResponse = await response.Content.ReadAsStringAsync();
            var getTrainModelResponse = JsonSerializer.Deserialize<GetTrainModelStatusResponse>(contentResponse);
            return BaseResponse<GetTrainedModelStatusResponseDto>.Success(GetTrainModelStatusMapper.ToResponseDto(getTrainModelResponse));
        }

        public async Task<BaseResponse<TrainModelResponseDto>> TrainModelAsync(TrainModelRequestDto request)
        {
            var postUrl = $"models/{_replicateSetting.Model}/versions/{_replicateSetting.Version}/trainings";
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
