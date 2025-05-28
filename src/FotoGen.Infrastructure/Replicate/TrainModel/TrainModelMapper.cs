using FotoGen.Common.Contracts.Replicated.CreateModel;
using FotoGen.Common.Contracts.Replicated.TrainModel;
using FotoGen.Infrastructure.Replicate.CreateModel;
using FotoGen.Infrastructure.Settings;

namespace FotoGen.Infrastructure.Replicate.TrainModel
{
    public static class TrainModelMapper
    {
        public static TrainModelInput ToRequest(TrainModelRequestDto request, ReplicateSetting settings)
        {
            return new TrainModelInput
            {
                Input = new ReplicateModelInput
                {
                    Steps = 1000,
                    LoraRank = 16,
                    Optimizer = "adamw8bit",
                    BatchSize = 1,
                    Resolution = "512,768,1024",
                    AutoCaption = true,
                    InputImages = request.ImageUrl,
                    TriggerWord = request.TriggerWord,
                    LearningRate = 0.0004,
                    WandbProject = "flux_train_replicate",
                    WandbSaveInterval = 100,
                    CaptionDropoutRate = 0.05,
                    CacheLatentsToDisk = false,
                    WandbSampleInterval = 100,
                    GradientCheckpointing = false
                },
                Destination = $"{settings.Owner}/{request.Name}"
            };
        }
        public static TrainModelResponseDto ToResponseDto(TrainModelResponse response)
        {
            return new TrainModelResponseDto
            {
                Id = response.Id,
                CanceledUrl = response.Urls.Cancel,
                Status = response.Status
            };
        }
    }
}
