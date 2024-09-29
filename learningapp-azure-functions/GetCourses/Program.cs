using Microsoft.Azure.Functions.Worker;
using Azure.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(builder =>
        {
            builder.AddAzureAppConfiguration(options =>{
    options.Connect(
        new Uri("https://appconfig28092024.azconfig.io"),
        new ManagedIdentityCredential());
    options.UseFeatureFlags();
    }
);
        })
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
