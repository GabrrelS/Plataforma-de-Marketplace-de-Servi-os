using Microsoft.Extensions.Http.Resilience;

namespace PlataformaServicos.HttpClients;

public static class HttpClientConfiguration
{
    public static IServiceCollection AddMicroserviceClients(this IServiceCollection services)
    {
        services.AddHttpClient("OrderService", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5001");
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddStandardResilienceHandler(options =>
        {
            options.Retry.MaxRetryAttempts = 3;
            options.Retry.Delay = TimeSpan.FromMilliseconds(300);
            options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
            options.CircuitBreaker.FailureRatio = 0.5;
            options.CircuitBreaker.MinimumThroughput = 5;
            options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15);
        });

        services.AddHttpClient("ProductService", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5002");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddStandardResilienceHandler(options =>
        {
            options.Retry.MaxRetryAttempts = 2;
            options.Retry.Delay = TimeSpan.FromMilliseconds(200);
            options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(20);
            options.CircuitBreaker.FailureRatio = 0.6;
            options.CircuitBreaker.MinimumThroughput = 3;
            options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(10);
        });

        services.AddHttpClient("NotificationService", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5003");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddStandardResilienceHandler(options =>
        {
            options.Retry.MaxRetryAttempts = 2;
            options.Retry.Delay = TimeSpan.FromMilliseconds(200);
        });

        return services;
    }
}