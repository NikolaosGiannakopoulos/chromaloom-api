using Microsoft.AspNetCore.Builder;

namespace ChromaLoom.Infrastructure.Setup;

public static partial class ServiceRegistration
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddIdentity();

        return builder;
    }
}
