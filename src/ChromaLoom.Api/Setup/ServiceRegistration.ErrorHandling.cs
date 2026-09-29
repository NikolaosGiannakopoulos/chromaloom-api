using System.Diagnostics;
using ChromaLoom.Api.ErrorHandling;

namespace ChromaLoom.Api.Setup;

internal static partial class ServiceRegistration
{
    extension(WebApplicationBuilder builder)
    {
        private WebApplicationBuilder AddErrorHandling()
        {
            builder.Services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance ??=
                        $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                    context.ProblemDetails.Extensions["traceId"] =
                        Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
                };
            });

            builder.Services.AddExceptionHandler<OperationCanceledExceptionHandler>();
            builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
            builder.Services.AddExceptionHandler<DomainExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            return builder;
        }
    }
}
