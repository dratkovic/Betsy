using System.Text;
using Betsy.Application.Common.Cache;
using Betsy.Application.Common.Interfaces;
using Betsy.Application.Common.Interfaces.Repositories;
using Betsy.Domain.Common;
using Betsy.Infrastructure.Authentication.PasswordHasher;
using Betsy.Infrastructure.Authentication.TokenGenerator;
using Betsy.Infrastructure.Common;
using Betsy.Infrastructure.Common.Cache;
using Betsy.Infrastructure.Common.Persistence;
using Betsy.Infrastructure.Persistance.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Betsy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddAuthentication(configuration)
            .AddPersistence(configuration);
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<BetsyDbContext>());
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IUserSession, UserSession>();
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.Section, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            });

        services.AddAuthorization();
        return services;
    }

    public static IHostApplicationBuilder AddDb(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<BetsyDbContext>("betsy-db");

        return builder;
    }
}