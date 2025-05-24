namespace FotoGen.Common.Contracts.Replicated.TrainModel
{
    public class TrainModelResponseDto
    {
        public string Id { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string CanceledUrl { get; set; } = default!;
    }
}
