using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Entities
{
    public class TrainModelEntity
    {
        public string Id { get; set; } = default!;
        public string ModelName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string UserEmail { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string TriggerWord { get; set; } = default!;
        public TrainModelStatus Status {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SuccessedAt {  get; set; }
        public string? CanceledUrl {  get; set; }
        public TrainModelEntity() { }
        public TrainModelEntity(string id, string modelName, string userEmail, string userName, string imageUrl, string triggerWords, TrainModelStatus status, string? canceledUrl = null, DateTime? successedAt = null)
        {
            Id = id;
            ModelName = modelName;
            UserEmail = userEmail;
            UserName = userName;
            ImageUrl = imageUrl;
            TriggerWord = triggerWords;
            Status = status;
            CreatedAt = DateTime.UtcNow;
            SuccessedAt = successedAt;
            CanceledUrl = canceledUrl;
        }
    }
}
