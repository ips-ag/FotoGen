using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Entities
{
    public class ReplicateModelEntity
    {
        public Guid Id { get; private set; }
        public string Owner { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private ReplicateModelEntity() { }

        public ReplicateModelEntity(string owner, string name, string description)
        {
            Id = Guid.NewGuid();
            Owner = owner;
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
