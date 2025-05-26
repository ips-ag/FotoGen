namespace FotoGen.Common.Contracts.Replicated.GetTrainedModelStatus
{
    public class GetTrainedModelStatusResponseDto
    {
        public string Id { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string Model { get; set; } = default!;
        public string Version {  get; set; } = default!;
    }
}
