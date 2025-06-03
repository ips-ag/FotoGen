using System.Text;
using System.Text.Json;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Images;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;
using FotoGen.Infrastructure.Replicate.CreateModel;
using FotoGen.Infrastructure.Replicate.GetTrainedModelStatus;
using FotoGen.Infrastructure.Replicate.TrainModel;
using FotoGen.Infrastructure.Replicate.UseModel;
using FotoGen.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using TrainModelResponse = FotoGen.Domain.Entities.Models.TrainModelResponse;

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
        public async Task<BaseResponse<bool>> CreateReplicateModelAsync(CreateModelRequest dto)
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

        public async Task<BaseResponse<GenerateImageResponse>> GeneratePhotoAsync(string prompt, string modelName)
        {
            var input = UseModelMapper.ToModel(prompt, modelName, _replicateSetting);
            var json = JsonSerializer.Serialize(input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("predictions", content);
            if (!response.IsSuccessStatusCode)
            {
                return BaseResponse<GenerateImageResponse>.Fail(ErrorCode.GeneratePhotoFail);
            }
            var contentResponse = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<UseModelResponseModel>(contentResponse);
            return BaseResponse<GenerateImageResponse>.Success(UseModelMapper.ToDomain(responseModel));
        }

        public async Task<BaseResponse<bool>> GetModelAsync(string name)
        {
            var url = $"models/{_replicateSetting.Owner}/{name}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return BaseResponse<bool>.Fail(ErrorCode.ReplicateModelNotFound);
                return BaseResponse<bool>.Fail(ErrorCode.GetReplicateModelFail);
            }
            return BaseResponse<bool>.Success(true);
        }

        public async Task<BaseResponse<QueryModelTrainingStatus>> GetTrainModelStatusAsync(string trainModelId)
        {
            var getUrl = $"trainings/{trainModelId}";
            var response = await _httpClient.GetAsync(getUrl);
            if (!response.IsSuccessStatusCode)
            {
                return BaseResponse<QueryModelTrainingStatus>.Fail(ErrorCode.GetReplicateTrainModelFail);
            }
            var contentResponse = await response.Content.ReadAsStringAsync();
            var getTrainModelResponse = JsonSerializer.Deserialize<GetTrainedModelStatusResponse>(contentResponse);
            return BaseResponse<QueryModelTrainingStatus>.Success(GetTrainedModelStatusMapper.ToResponseDto(getTrainModelResponse));
        }

        public async Task<BaseResponse<TrainModelResponse>> TrainModelAsync(TrainModelRequest request)
        {
            var postUrl = $"models/{_replicateSetting.Model}/versions/{_replicateSetting.Version}/trainings";
            var requestModel = TrainModelMapper.ToRequest(request, _replicateSetting);
            var json = JsonSerializer.Serialize(requestModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(postUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
            }
            var contentResponse = await response.Content.ReadAsStringAsync();
            var trainModelResponse = JsonSerializer.Deserialize<TrainModel.TrainModelResponseModel>(contentResponse);
            return BaseResponse<TrainModelResponse>.Success(TrainModelMapper.ToResponseDto(trainModelResponse));
        }
    }
}
