using FotoGen.Domain.Entities.Requests;

namespace FotoGen.Infrastructure.Repositories.Requests;

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

internal class RequestContextAccessor
{
    private static readonly AsyncLocal<RequestContextHolder> CurrentContext = new();

    public RequestContext? Context
    {
        get => CurrentContext.Value?.Context;
        internal set
        {
            var holder = CurrentContext.Value;
            if (holder != null)
            {
                holder.Context = null;
            }
            if (value != null)
            {
                CurrentContext.Value = new RequestContextHolder { Context = value };
            }
        }
    }

    private class RequestContextHolder
    {
        public RequestContext? Context { get; set; }
    }
}
