using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using Microsoft.Extensions.Options;
using ChromaLoom.Infrastructure.Configuration;
using ChromaLoom.Infrastructure.Configuration.Validation;

namespace ChromaLoom.Api.Setup;

internal static partial class ServiceRegistration
{
    extension(WebApplicationBuilder builder)
    {
        private WebApplicationBuilder AddObservability()
        {
            builder.Services.AddValidatedOptions<OpenTelemetryOptions, OpenTelemetryOptionsValidator>(
                builder.Configuration,
                OpenTelemetryOptions.SectionName);

            builder.Services.AddOptions<OtlpExporterOptions>()
                .Configure<IOptions<OpenTelemetryOptions>>((exporter, otel) =>
                {
                    exporter.Endpoint = new Uri(otel.Value.OtlpEndpoint);
                });

            builder.Services
                .AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(
                    serviceName: builder.Environment.ApplicationName))
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddOtlpExporter())
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter());

            builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

            return builder;
        }
    }
}
