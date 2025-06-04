using FotoGen.Application.Interfaces;

namespace FotoGen.Infrastructure;

public class DownloadClient : IDownloadClient
{
    private readonly HttpClient _httpClient;

    public DownloadClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<byte[]> GetByteArrayAsync(string outputUri)
    {
        return await _httpClient.GetByteArrayAsync(outputUri);
    }
}
