namespace ChromaLoom.Api.Setup;

internal static partial class PipelineOrchestration
{
    extension(WebApplication app)
    {
        internal void UsePipeline()
        {
            app
                .UseDocumentation();
        }
    }
}
