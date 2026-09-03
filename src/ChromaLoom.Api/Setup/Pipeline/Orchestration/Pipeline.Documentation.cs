using Scalar.AspNetCore;

namespace ChromaLoom.Api.Setup.Pipeline.Orchestration;

internal static partial class Pipeline
{
    extension(WebApplication app)
    {
        private WebApplication UseDocumentation()
        {
            if (!app.Environment.IsDevelopment())
            {
                return app;
            }

            app.MapOpenApi()
                .AllowAnonymous()
                .WithDocumentPerVersion();

            app.MapScalarApiReference(options =>
            {
                options
                    .WithTitle("ChromaLoom API")
                    .WithTheme(ScalarTheme.DeepSpace);
            }).AllowAnonymous();

            app.MapGet("/", () => Results.Redirect("/scalar"))
                .ExcludeFromDescription()
                .AllowAnonymous();

            return app;
        }
    }
}
