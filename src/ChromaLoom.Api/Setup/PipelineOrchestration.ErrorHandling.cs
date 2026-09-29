namespace ChromaLoom.Api.Setup;

internal static partial class PipelineOrchestration
{
    extension(WebApplication app)
    {
        private WebApplication UseErrorHandling()
        {
            app.UseExceptionHandler();
            app.UseStatusCodePages();

            return app;
        }
    }
}
