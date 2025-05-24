using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Entities
{
    public class TrainModelEntity
    {
        public string Id { get; private set; } = default!;
        public string ReplicateModelModelId { get; private set; } = default!;
        public string ImageUrl { get; private set; } = default!;
        public string TriggerWords { get; private set; } = default!;
        public TrainModelStatus Status {  get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt {  get; private set; }
        public string? CanceledUrl {  get; private set; }
        public TrainModelEntity(string id, string replicateModelModelId, string imageUrl, string triggerWords, TrainModelStatus status, string? canceledUrl = null, DateTime? completedAt = null)
        {
            Id = id;
            ReplicateModelModelId = replicateModelModelId;
            ImageUrl = imageUrl;
            TriggerWords = triggerWords;
            Status = status;
            CreatedAt = DateTime.UtcNow;
            CompletedAt = completedAt;
            CanceledUrl = canceledUrl;
        }
    }
}
