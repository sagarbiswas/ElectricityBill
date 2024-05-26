using EnergyAnnualCostCalculation.BusinessLayer;
using EnergyAnnualCostCalculation.Common;
using EnergyAnnualCostCalculation.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EnergyAnnualCostCalculation
{
    public  static class ServicesProvider
    {
        private static ApplicationSettings _applicationSettings;

        public static ApplicationSettings ApplicationSettings
        {
            get { return _applicationSettings; }
            set { _applicationSettings = value; }
        }
        public static ServiceProvider RegisterServices()
        {
             IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(options =>
                {
                    options.ClearProviders();
                    options.AddConsole();
                })
                .AddScoped<Application>()
                .Configure<ApplicationSettings>(config.GetSection("ApplicationSettings"))
                .AddTransient<IJsonSchemaValidator, JsonSchemaValidator>()
                .AddTransient<IEnergyConsumptionHandler, EnergyConsumptionHandler>()
                .BuildServiceProvider();

           _applicationSettings = serviceProvider.GetService<IOptions<ApplicationSettings>>().Value;

            return serviceProvider;
        }
    }
}
