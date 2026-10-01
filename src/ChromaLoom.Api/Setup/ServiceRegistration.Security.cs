using Microsoft.Extensions.Options;
using ChromaLoom.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChromaLoom.Infrastructure.Configuration.Validation;

namespace ChromaLoom.Api.Setup;

internal static partial class ServiceRegistration
{
    extension(WebApplicationBuilder builder)
    {
        private WebApplicationBuilder AddSecurity()
        {
            builder.Services.AddValidatedOptions<AuthenticationOptions, AuthenticationOptionsValidator>(
                builder.Configuration,
                AuthenticationOptions.SectionName);

            var isDevelopment = builder.Environment.IsDevelopment();

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<AuthenticationOptions>>((jwt, authentication) =>
                {
                    var options = authentication.Value;

                    jwt.MapInboundClaims = false;
                    jwt.Authority = options.Authority;
                    jwt.Audience = options.Audience;
                    jwt.RequireHttpsMetadata = !isDevelopment;
                    jwt.TokenValidationParameters.ValidateAudience = true;
                    jwt.TokenValidationParameters.NameClaimType = "sub";
                    jwt.TokenValidationParameters.RoleClaimType = "role";
                });

            builder.Services.AddAuthorization();

            return builder;
        }
    }
}
