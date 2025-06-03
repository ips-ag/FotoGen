using FotoGen.Domain.Entities.Requests;
using FotoGen.Domain.Repositories;

namespace FotoGen.Infrastructure.Repositories.Requests;

internal class RequestContextRepository : IRequestContextRepository
{
    private readonly RequestContextAccessor _accessor;
    private readonly RequestContextFactory _factory;

    public RequestContextRepository(RequestContextAccessor accessor, RequestContextFactory factory)
    {
        _accessor = accessor;
        _factory = factory;
    }

    public async ValueTask<RequestContext> GetAsync()
    {
        return _accessor.Context ??= await _factory.CreateAsync();
    }
}
