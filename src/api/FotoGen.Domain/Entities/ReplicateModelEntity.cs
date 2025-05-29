using FotoGen.Domain.ValueObjects;

namespace FotoGen.Domain.Entities
{
    public class ReplicateModelEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        private ReplicateModelEntity() { }

        public ReplicateModelEntity(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
