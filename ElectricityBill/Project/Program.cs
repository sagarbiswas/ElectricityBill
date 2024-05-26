using EnergyAnnualCostCalculation.BusinessLayer;
using EnergyAnnualCostCalculation.Common;
using EnergyAnnualCostCalculation.Model;
using EnergyAnnualCostCalculation.Validation;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
namespace EnergyAnnualCostCalculation
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // DI registration
            var services = ServicesProvider.RegisterServices();

            Application app = services.GetRequiredService<Application>();
            app.StartApp(); // start point of application.
        }

    }
}