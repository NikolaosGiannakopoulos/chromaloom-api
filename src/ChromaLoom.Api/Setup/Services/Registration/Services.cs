namespace ChromaLoom.Api.Setup.Services.Registration;

internal static partial class Services
{
    extension(WebApplicationBuilder builder)
    {
        internal void AddServices()
        {
            builder
                .AddDocumentation();
        }
    }
}
