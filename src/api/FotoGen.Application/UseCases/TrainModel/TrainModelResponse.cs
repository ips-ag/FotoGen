namespace FotoGen.Application.UseCases.TrainModel
{
    public class TrainModelResponse
    {
        public string Id { get; init; } = default!;
        public string Status { get; init; } = default!;
        public string CanceledUrl {  get; init; } = default!;
    }
}
