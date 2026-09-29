using ChromaLoom.Infrastructure.Setup;

namespace ChromaLoom.Api.Setup;

internal static partial class ServiceRegistration
{
    extension(WebApplicationBuilder builder)
    {
        internal void AddServices()
        {
            builder
                .AddStartupBanner()
                .AddErrorHandling()
                .AddObservability()
                .AddDocumentation()
                .AddInfrastructure();
        }
    }
}
