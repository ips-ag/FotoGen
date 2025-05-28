namespace FotoGen.Common.Contracts.Replicated.TrainModel
{
    public class TrainModelRequestDto
    {
        public string Name { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string TriggerWord { get; set; } = default!;
    }
}
