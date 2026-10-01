using Microsoft.AspNetCore.Builder;
using ChromaLoom.Infrastructure.Identity;
using ChromaLoom.Kernel.Abstractions.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChromaLoom.Infrastructure.Setup;

public static partial class ServiceRegistration
{
    private static WebApplicationBuilder AddCurrentUser(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.TryAddScoped<ICurrentUser, CurrentUser>();

        return builder;
    }
}
