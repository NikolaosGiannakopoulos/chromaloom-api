namespace ChromaLoom.Api.Setup;

internal static partial class PipelineOrchestration
{
    extension(WebApplication app)
    {
        private WebApplication UseSecurity()
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
