namespace ChromaLoom.Api.Setup.Pipeline.Orchestration;

internal static partial class Pipeline
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
