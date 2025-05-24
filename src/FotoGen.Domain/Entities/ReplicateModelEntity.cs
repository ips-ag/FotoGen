using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Entities
{
    public class ReplicateModelEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private ReplicateModelEntity() { }

        public ReplicateModelEntity(string name, string? description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
