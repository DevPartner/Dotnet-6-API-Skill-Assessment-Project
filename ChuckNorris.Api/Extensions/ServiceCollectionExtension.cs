using Microsoft.Extensions.Http;
using Polly;
using System.Net;

namespace ChuckNorris.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddTransformation<T, TImplementation>(this IServiceCollection services, IConfiguration config)
        where T : class
        where TImplementation : class, T
    {
        var transformationSection = config.GetRequiredSection(typeof(TImplementation).Name);
        services.AddScoped<T>(sp =>
        {
            var instance = Activator.CreateInstance(typeof(TImplementation), transformationSection) as T;

            return instance;
        });
    }

    public static void AddConnector<T, TImplementation>(this IServiceCollection services, IConfiguration config)
        where T : class
        where TImplementation : class, T
    {
        var connectorSection = config.GetRequiredSection(typeof(TImplementation).Name);
        var apiHost = connectorSection.GetRequiredValue<string>("ApiHost");

        services
            .AddHttpClient<T, TImplementation>(client =>
            {
                // Media can be quite large
                client.Timeout = Timeout.InfiniteTimeSpan;
                client.BaseAddress = new UriBuilder(Uri.UriSchemeHttps, apiHost) { Path = "jokes/" }.Uri;
            })
            .AddPolicyHandler();
    }

    private static void AddPolicyHandler(this IHttpClientBuilder builder)
    {
        //retry for 1 minute (4, 8, 16, 32, 64, 128, 256, 512)
        var policy = Policy<HttpResponseMessage>
            .HandleResult(message => message.StatusCode == HttpStatusCode.BadGateway ||
                                     message.StatusCode == HttpStatusCode.GatewayTimeout ||
                                     (int)message.StatusCode == 524)
            .WaitAndRetryAsync(7, attempt => TimeSpan.FromSeconds(2 << attempt));

        builder.AddHttpMessageHandler(() => new PolicyHttpMessageHandler(policy));
    }
}
