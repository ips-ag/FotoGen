using System.Net;
using System.Net.Http.Json;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TrainModelResponse = FotoGen.Domain.Entities.Models.TrainModelResponse;

namespace FotoGen.Infrastructure.Replicate;

public class ReplicateService : IReplicateService
{
    private readonly ReplicateSetting _replicateSetting;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReplicateService> _logger;

    public ReplicateService(
        IOptions<ReplicateSetting> replicateSetting,
        HttpClient httpClient,
        ILogger<ReplicateService> logger)
    {
        _replicateSetting = replicateSetting.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> CreateTrainedModelAsync(
        CreateTrainedModelRequest request,
        CancellationToken cancel)
    {
        var requestModel = CreateModelMapper.ToRequest(request, _replicateSetting);
        var content = JsonContent.Create(requestModel);
        var responseModel = await _httpClient.PostAsync("models", content, cancel);
        if (responseModel.IsSuccessStatusCode) return BaseResponse<bool>.Success(true);
        var problem = await responseModel.Content.ReadFromJsonAsync<ProblemDetails>(cancel);
        _logger.LogWarning(
            "Failed to create trained model {ModelName}, Status Code: {StatusCode}, Detail: {ProblemDetail}",
            request.Name,
            responseModel.StatusCode,
            problem?.Detail);
        return BaseResponse<bool>.Fail(ErrorCode.CreateReplicateModelFail);
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
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancel);
            _logger.LogWarning(
                "Failed to generate photo using model {ModelName}, Status Code: {StatusCode}, Detail: {ProblemDetail}",
                model.Name,
                response.StatusCode,
                problem?.Detail);
            return BaseResponse<GenerateImageResponse>.Fail(ErrorCode.GeneratePhotoFail);
        }
        var responseModel = await response.Content.ReadFromJsonAsync<UseModelResponseModel>(cancel);
        return responseModel is null
            ? BaseResponse<GenerateImageResponse>.Fail(ErrorCode.GeneratePhotoFail)
            : BaseResponse<GenerateImageResponse>.Success(UseModelMapper.ToDomain(responseModel));
    }

    public async ValueTask<TrainedModel?> GetTrainedModelByNameAsync(string name, CancellationToken cancel)
    {
        var url = $"models/{_replicateSetting.Owner}/{name}";
        var response = await _httpClient.GetAsync(url, cancel);
        if (response is { IsSuccessStatusCode: false, StatusCode: HttpStatusCode.NotFound }) return null;
        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancel);
            _logger.LogWarning(
                "Failed to get trained model by name {ModelName}, Status Code: {StatusCode}, Detail: {ProblemDetail}",
                name,
                response.StatusCode,
                problem?.Detail);
            return null;
        }
        var responseModel = await response.Content.ReadFromJsonAsync<GetTrainedModelResponseModel>(cancel);
        if (responseModel is null) return null;
        var trainedModel = TrainedModelMapper.ToDomain(responseModel);
        return trainedModel;
    }

    public async Task<BaseResponse<QueryModelTrainingStatus>> GetModelTrainingStatusAsync(
        string trainModelId,
        CancellationToken cancel)
    {
        var getUrl = $"trainings/{trainModelId}";
        var response = await _httpClient.GetAsync(getUrl, cancel);
        if (!response.IsSuccessStatusCode)
        {
            return BaseResponse<QueryModelTrainingStatus>.Fail(ErrorCode.GetReplicateTrainModelFail);
        }
        var getTrainModelResponse = await response.Content.ReadFromJsonAsync<GetTrainedModelStatusResponse>(cancel);
        return getTrainModelResponse is null
            ? BaseResponse<QueryModelTrainingStatus>.Fail(ErrorCode.GetReplicateTrainModelFail)
            : BaseResponse<QueryModelTrainingStatus>.Success(
                GetTrainedModelStatusMapper.ToResponseDto(getTrainModelResponse));
    }

    public async Task<BaseResponse<TrainModelResponse>> CreateModelTrainingAsync(
        TrainModelRequest request,
        CancellationToken cancel)
    {
        var trainingSettings = _replicateSetting.Training;
        var postUrl = $"models/{trainingSettings.Model}/versions/{trainingSettings.Version}/trainings";
        var requestModel = TrainModelMapper.ToRequest(request, _replicateSetting);
        var content = JsonContent.Create(requestModel);
        var response = await _httpClient.PostAsync(postUrl, content, cancel);
        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancel);
            _logger.LogWarning(
                "Failed to create model training for {ModelName}, Status Code: {StatusCode}, Detail: {ProblemDetail}",
                request.Name,
                response.StatusCode,
                problem?.Detail);
            return BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail);
        }
        var trainModelResponse = await response.Content.ReadFromJsonAsync<TrainModelResponseModel>(cancel);
        return trainModelResponse is null
            ? BaseResponse<TrainModelResponse>.Fail(ErrorCode.CreateReplicateModelFail)
            : BaseResponse<TrainModelResponse>.Success(TrainModelMapper.ToResponseDto(trainModelResponse));
    }
}
