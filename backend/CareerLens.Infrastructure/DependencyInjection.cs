using CareerLens.Application.Common.Interfaces;
using CareerLens.Domain.Interfaces;
using CareerLens.Infrastructure.Jobs;
using CareerLens.Infrastructure.Persistence;
using CareerLens.Infrastructure.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CareerLens.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, bool isDevelopment = false)
    {
        // PostgreSQL + EF Core
        services.AddDbContext<CareerLensDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Redis
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")!));
        services.AddScoped<ICacheService, CacheService>();

        // Şifreler + JWT
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();

        // Blob Storage — development'ta lokal, production'da Azure
        if (isDevelopment)
            services.AddScoped<IBlobStorageService, LocalBlobStorageService>();
        else
            services.AddScoped<IBlobStorageService, BlobStorageService>();

        // CV Parser
        services.AddScoped<ICvParserService, CvParserService>();

        // AI Service — development'ta mock, production'da gerçek
        if (isDevelopment)
            services.AddScoped<ICareerAiService, MockCareerAiService>();
        else
            services.AddScoped<ICareerAiService, CareerAiService>();

        // TÜFE Enflasyon Endeksi — development'ta mock, production'da TCMB EVDS
        services.AddHttpClient("Tcmb", client =>
        {
            client.BaseAddress = new Uri("https://evds2.tcmb.gov.tr/service/evds/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });
        if (isDevelopment)
            services.AddScoped<ICpiIndexService, MockCpiIndexService>();
        else
            services.AddScoped<ICpiIndexService, TcmbCpiIndexService>();

        // Background Jobs
        services.AddScoped<ICvProcessingJobService, CvProcessingJobService>();
        services.AddScoped<CvTextExtractionJob>();
        services.AddScoped<CvAiParsingJob>();

        // Hangfire
        services.AddHangfire(cfg => cfg
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(config.GetConnectionString("DefaultConnection"))));

        services.AddHangfireServer();

        return services;
    }
}
