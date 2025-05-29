namespace FotoGen.Application.UseCases.GeneratePhoto
{
    public class GeneratePhotoResponse
    {
        public string Base64Image { get; set; } = default!;
        public string OutputFormat { get; set; } = default!;
    }
}
