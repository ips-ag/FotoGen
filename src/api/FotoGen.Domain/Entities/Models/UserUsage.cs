using FotoGen.Domain.Entities.Requests;

namespace FotoGen.Domain.Entities.Models
{
    public class UserUsage
    {
        public UserUsage() { }
        public UserUsage(User user) 
        {
            UserId = user.Id;
            Date = DateOnly.FromDateTime(DateTime.UtcNow);
            TrainingCount = 0;
            PhotoGenerationCount = 0;
            LastUpdate = DateTime.UtcNow;
        }
        public string UserId { get; init; }
        public DateOnly Date { get; init; }
        public int TrainingCount { get; set; }
        public int PhotoGenerationCount { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
