using System.Security.Claims;
using FotoGen.Domain.Entities.Requests;
using Microsoft.AspNetCore.Http;

namespace FotoGen.Infrastructure.Repositories.Requests;

internal class RequestContextFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestContextFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ValueTask<RequestContext> CreateAsync()
    {
        var context = new RequestContext(GetUser());
        return new ValueTask<RequestContext>(context);
    }

    private User GetUser()
    {
        string? id = GetClaimValue("sub");
        string? name = GetClaimValue(ClaimTypes.Name);
        string? email = GetClaimValue(ClaimTypes.Email);
        if (id is null || name is null || email is null) throw new InvalidOperationException("Unknown user");
        return new User(id, name, email);
    }

    private string? GetClaimValue(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }
}
