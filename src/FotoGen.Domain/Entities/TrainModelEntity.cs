using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Entities
{
    public class TrainModelEntity
    {
        public string Id { get; set; } = default!;
        public string ReplicateModelModelId { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string TriggerWords { get; set; } = default!;
        public TrainModelStatus Status {  get; set; }
        public DateTime CreatedAt { get; set; }
        public void MarkAsQueued() => Status = TrainModelStatus.Queued;
        public void MarkAsStarted() => Status = TrainModelStatus.Starting;
        public void MarkAsTrained() => Status = TrainModelStatus.Completed;
        public void MarkAsFailed() => Status = TrainModelStatus.Failed;
    }
}
