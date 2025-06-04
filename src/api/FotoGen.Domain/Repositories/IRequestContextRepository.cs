using FotoGen.Domain.Entities.Requests;

namespace FotoGen.Domain.Repositories;

public interface IRequestContextRepository
{
    ValueTask<RequestContext> GetAsync();
}
