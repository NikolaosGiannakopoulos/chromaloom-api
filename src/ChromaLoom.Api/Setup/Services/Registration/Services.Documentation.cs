using Asp.Versioning;

namespace ChromaLoom.Api.Setup.Services.Registration;

internal static partial class Services
{
    extension(WebApplicationBuilder builder)
    {
        private WebApplicationBuilder AddDocumentation()
        {
            builder.Services
                .AddApiVersioning(options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddOpenApi();

            return builder;
        }
    }
}
