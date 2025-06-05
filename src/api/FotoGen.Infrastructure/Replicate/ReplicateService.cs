using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Images;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Entities.Response;
using FotoGen.Infrastructure.Replicate.Configuration;
using FotoGen.Infrastructure.Replicate.CreateModel.Converters;
using FotoGen.Infrastructure.Replicate.GetTrainedModel.Converters;
using FotoGen.Infrastructure.Replicate.GetTrainedModel.Models;
using FotoGen.Infrastructure.Replicate.GetTrainedModelStatus;
using FotoGen.Infrastructure.Replicate.TrainModel;
using FotoGen.Infrastructure.Replicate.UseModel.Converters;
using FotoGen.Infrastructure.Replicate.UseModel.Models;
using Microsoft.Extensions.Options;
using TrainModelResponse = FotoGen.Domain.Entities.Models.TrainModelResponse;

namespace FotoGen.Infrastructure.Replicate;

public class ReplicateService : IReplicateService
{
    private readonly ReplicateSetting _replicateSetting;
    private readonly HttpClient _httpClient;

    public ReplicateService(IOptions<ReplicateSetting> replicateSetting, HttpClient httpClient)
    {
        _replicateSetting = replicateSetting.Value;
        _httpClient = httpClient;
    }

    public async Task<BaseResponse<bool>> CreateTrainedModelAsync(CreateTrainedModelRequest dto)
    {
        var requestModel = CreateModelMapper.ToRequest(dto, _replicateSetting);
        string json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("models", content);
        return !response.IsSuccessStatusCode
            ? BaseResponse<bool>.Fail(ErrorCode.CreateReplicateModelFail)
            : BaseResponse<bool>.Success(true);
    }

    public async Task<BaseResponse<GenerateImageResponse>> GeneratePhotoAsync(
        string prompt,
        TrainedModel model,
        CancellationToken cancel)
    {
        if (model.LatestVersion is null)
        {
            return BaseResponse<GenerateImageResponse>.Fail(ErrorCode.ReplicateModelNotFound);
        }
        var input = UseModelMapper.ToModel(prompt, model.LatestVersion);
        var content = JsonContent.Create(input);
        var response = await _httpClient.PostAsync("predictions", content, cancel);
        if (!response.IsSuccessStatusCode)
        {
            return BaseResponse<GenerateImageResponse>.Fail(ErrorCode.GeneratePhotoFail);
        }
        string contentResponse = await response.Content.ReadAsStringAsync(cancel);
        var responseModel = JsonSerializer.Deserialize<UseModelResponseModel>(contentResponse);
        return BaseResponse<GenerateImageResponse>.Success(UseModelMapper.ToDomain(responseModel));
    }

    public async ValueTask<TrainedModel?> GetTrainedModelByNameAsync(string name, CancellationToken cancel)
    {
        var url = $"models/{_replicateSetting.Owner}/{name}";
        var response = await _httpClient.GetAsync(url, cancel);
        if (!response.IsSuccessStatusCode) return null;
        var responseModel = await response.Content.ReadFromJsonAsync<GetTrainedModelResponseModel>(cancel);
        if (responseModel is null) return null;
        var trainedModel = TrainedModelMapper.ToDomain(responseModel);
        return trainedModel;
    }

    public async Task<BaseResponse<QueryModelTrainingStatus>> GetModelTrainingStatusAsync(string trainModelId)
    {
        var getUrl = $"trainings/{trainModelId}";
        var response = await _httpClient.GetAsync(getUrl);
        if (!response.IsSuccessStatusCode)
        {
            return BaseResponse<QueryModelTrainingStatus>.Fail(ErrorCode.GetReplicateTrainModelFail);
        }
        string contentResponse = await response.Content.ReadAsStringAsync();
        var getTrainModelResponse = JsonSerializer.Deserialize<GetTrainedModelStatusResponse>(contentResponse);
        return BaseResponse<QueryModelTrainingStatus>.Success(
            GetTrainedModelStatusMapper.ToResponseDto(getTrainModelResponse));
    }

    public async Task<BaseResponse<TrainModelResponse>> CreateModelTrainingAsync(TrainModelRequest request)
    {
        var trainingSettings = _replicateSetting.Training;
        var postUrl = $"models/{trainingSettings.Model}/versions/{trainingSettings.Version}/trainings";
        var requestModel = TrainModelMapper.ToRequest(request, _replicateSetting);
        string json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(postUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
        }
        string contentResponse = await response.Content.ReadAsStringAsync();
        var trainModelResponse = JsonSerializer.Deserialize<TrainModelResponseModel>(contentResponse);
        return BaseResponse<TrainModelResponse>.Success(TrainModelMapper.ToResponseDto(trainModelResponse));
    }
}
