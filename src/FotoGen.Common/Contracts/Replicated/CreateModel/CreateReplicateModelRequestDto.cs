namespace FotoGen.Common.Contracts.Replicated.CreateModel
{
    public class CreateReplicateModelRequestDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; } = default!;
    }
}
