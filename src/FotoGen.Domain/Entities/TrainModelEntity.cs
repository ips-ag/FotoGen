using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Entities
{
    public class TrainModelEntity
    {
        public string Id { get; private set; } = default!;
        public string ModelName { get; private set; } = default!;
        public string UserId { get; private set; } = default!;
        public string ImageUrl { get; private set; } = default!;
        public string TriggerWords { get; private set; } = default!;
        public TrainModelStatus Status {  get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? SuccessedAt {  get; set; }
        public string? CanceledUrl {  get; private set; }
        public TrainModelEntity(string id, string replicateModelModelId, string imageUrl, string triggerWords, TrainModelStatus status, string? canceledUrl = null, DateTime? SuccessedAt = null)
        {
            Id = id;
            ModelName = replicateModelModelId;
            ImageUrl = imageUrl;
            TriggerWords = triggerWords;
            Status = status;
            CreatedAt = DateTime.UtcNow;
            this.SuccessedAt = SuccessedAt;
            CanceledUrl = canceledUrl;
        }
    }
}
