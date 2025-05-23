using System.Text;
using System.Text.Json;
using FotoGen.Application.Interfaces;
using FotoGen.Common;
using FotoGen.Common.Contracts.Replicated.CreateModel;
using FotoGen.Infrastructure.Replicate.CreateModel;
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
        public async Task<BaseResponse<CreateReplicateModelResultDto>> CreateTrainModel(CreateReplicateModelRequestDto dto)
        {
            var requestModel = CreateModelMapper.ToRequest(dto, _replicateSetting);
            var json = JsonSerializer.Serialize(requestModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/models", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return BaseResponse<CreateReplicateModelResultDto>.Fail(ErrorCode.CreateReplicateModelFail);
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<CreateModelResponse>(responseContent);
            var result = CreateModelMapper.ToResultDto(data);
            return BaseResponse<CreateReplicateModelResultDto>.Success(result);
        }
    }
}
