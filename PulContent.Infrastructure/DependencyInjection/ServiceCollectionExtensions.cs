using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PulContent.Application.Interfaces;
using PulContent.Infrastructure.Consumers;
using PulContent.Infrastructure.Data;
using PulContent.Infrastructure.Services;
using StackExchange.Redis;
using System;

namespace PulContent.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        //var apiKey = config["OpenAi:ApiKey"] ?? throw new ArgumentNullException("ApiKey NotFound!");

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        //services.AddScoped<IAiService>(provider => new OpenAiTranscriptionService(apiKey));

        // Local Whsper
        //services.AddScoped<IAiService, LocalWhisperService>();
        // Mock For test
        services.AddScoped<IAiService, MockAiService>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = new ConfigurationOptions
            {
                EndPoints = { config.GetValue<string>("Redis:Connection") ?? "localhost:6379" },
                AbortOnConnectFail = false,
                ConnectTimeout = 3000
            };
            return ConnectionMultiplexer.Connect(options);
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<MediaProcessingConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => { });
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
