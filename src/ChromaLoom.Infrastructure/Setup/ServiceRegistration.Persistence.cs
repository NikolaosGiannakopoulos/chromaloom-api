using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ChromaLoom.Infrastructure.Persistence;
using ChromaLoom.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChromaLoom.Infrastructure.Configuration.Validation;
using ChromaLoom.Infrastructure.Persistence.Interceptors;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChromaLoom.Infrastructure.Setup;

public static partial class ServiceRegistration
{
    public static WebApplicationBuilder AddModuleDbContext<TContext>(
        this WebApplicationBuilder builder,
        Action<IServiceProvider, DbContextOptionsBuilder>? configure = null)
        where TContext : BaseDbContext
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddValidatedOptions<DatabaseOptions, DatabaseOptionsValidator>(
            builder.Configuration,
            DatabaseOptions.SectionName);

        builder.Services.TryAddScoped<SoftDeleteInterceptor>();
        builder.Services.TryAddScoped<AuditInterceptor>();

        builder.Services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var database = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            options
                .UseNpgsql(database.ConnectionString)
                .AddInterceptors(
                    serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
                    serviceProvider.GetRequiredService<AuditInterceptor>());

            configure?.Invoke(serviceProvider, options);
        });

        return builder;
    }
}
