namespace FotoGen.Application.UseCases.GeneratePhoto
{
    public class GeneratePhotoResponse
    {
        public BinaryData Data { get; set; } = default!;
        public string OutputFormat { get; set; } = default!;
    }
}
