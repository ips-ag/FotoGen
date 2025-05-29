namespace FotoGen.Application.Interfaces
{
    public interface IDownloadClient
    {
        public Task<byte[]> GetByteArrayAsync(string outputUri);
    }
}
