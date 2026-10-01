namespace ChromaLoom.Api.Setup;

internal static partial class PipelineOrchestration
{
    extension(WebApplication app)
    {
        private WebApplication UseHealthChecks()
        {
            app.MapHealthChecks("/health").AllowAnonymous();
            return app;
        }
    }
}
