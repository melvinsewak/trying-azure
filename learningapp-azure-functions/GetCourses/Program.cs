using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Azure.Identity;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
            {
                builder
                .AddEnvironmentVariables()
                .AddAzureAppConfiguration(options =>{
                    options.Connect(
                        new Uri(Environment.GetEnvironmentVariable("AppConfigEndpoint")!),
                        new ManagedIdentityCredential());
                    options.UseFeatureFlags();
                    }
                );
            }
    )
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
