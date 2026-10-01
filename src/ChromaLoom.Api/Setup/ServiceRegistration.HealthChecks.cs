namespace ChromaLoom.Api.Setup;

internal static partial class ServiceRegistration
{
    extension(WebApplicationBuilder builder)
    {
        private WebApplicationBuilder AddHealthChecks()
        {
            builder.Services.AddHealthChecks();
            return builder;
        }
    }
}
