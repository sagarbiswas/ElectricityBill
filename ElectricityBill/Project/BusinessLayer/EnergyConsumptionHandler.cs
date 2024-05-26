using EnergyAnnualCostCalculation.Common;
using EnergyAnnualCostCalculation.Model;
using EnergyAnnualCostCalculation.Model.JsonInputModel;
using EnergyAnnualCostCalculation.Model.JsonValidationModel;
using EnergyAnnualCostCalculation.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EnergyAnnualCostCalculation.BusinessLayer
{
    public class EnergyConsumptionHandler : IEnergyConsumptionHandler
    {
        private readonly ILogger<EnergyConsumptionHandler> _logger;
        private readonly IJsonSchemaValidator _iJsonSchemaValidator;
        
        public EnergyConsumptionHandler(IJsonSchemaValidator iJsonSchemaValidator, ILogger<EnergyConsumptionHandler> logger)
        {
            _iJsonSchemaValidator = iJsonSchemaValidator;
            _logger = logger;
        }
        /// <summary>
        /// Validate Json with Schema and Check records entry
        /// </summary>
        /// <param name="validateJsonWithSchema"></param>
        /// <returns></returns>
        public bool ValidateJsonWithSchema(ValidateRequest validateJsonWithSchema, out List<EnergyConsumptionCalculator> lstEnergyConsumptionCalculatorData)
        {
            lstEnergyConsumptionCalculatorData = new List<EnergyConsumptionCalculator>();
            var response = _iJsonSchemaValidator.Validate(validateJsonWithSchema);
            if (!response.Valid)
                return response.Valid;

            lstEnergyConsumptionCalculatorData = JsonConvert.DeserializeObject<List<EnergyConsumptionCalculator>>(JsonFiles.JsonInputFileContent) ?? new List<EnergyConsumptionCalculator>();
            return lstEnergyConsumptionCalculatorData.Count > 0;
        }

        /// <summary>
        /// This method used to get total cost .Should be abstract, so that someone can create new as per requirement.
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetTotalCost(EnergyConsumptionModel energyConsumptionModel)
        {
            if (energyConsumptionModel == null || 
                energyConsumptionModel.CurrentConsumerEnergyDetail == null ||
                energyConsumptionModel.TotalConsumption < 0)
            {
                return 0;
            }
            IEnumerable<Price> Prices = energyConsumptionModel.CurrentConsumerEnergyDetail.Prices;
            double totalCost = 0;
            foreach (var price in Prices)
            {
                price.Threshold ??= int.MaxValue; // last/remaining unit is assuming max unit. 
                if (energyConsumptionModel.TotalConsumption > price.Threshold)
                {
                    totalCost += price.Rate * (price.Threshold ?? 0);
                    energyConsumptionModel.TotalConsumption -= (price.Threshold ?? 0);
                }
                else
                {
                    // This should get the default cost that has no Threshold / range
                    if (Prices.Any(t => t.Threshold == int.MaxValue)) //. Handling null
                    {
                        var defaultRate = Prices.First(t => t.Threshold == null || t.Threshold == int.MaxValue);
                        totalCost += defaultRate.Rate * energyConsumptionModel.TotalConsumption;
                    }
                    break;
                }
            }

            //Adding standing charge
            if (totalCost <= 0) // If no Threshold  the cost will be 0
            {
                _logger.LogInformation("Standing charge applied");
                totalCost += (energyConsumptionModel.CurrentConsumerEnergyDetail.StandingCharge ?? Constants.DefaultStandingCharge) * Constants.TotalBillDays; // default 
            }

            return await Task.FromResult(Math.Round(totalCost, 2));
        }

        /// <summary>
        /// This method is used to print output.
        /// </summary>
        /// <param name="totalBillAmount"></param>
        public void PrintOutPut(Double totalBillAmount, EnergyConsumptionCalculator energyConsumptionCalculator, bool shouldPrintHeader, bool shouldPrintFooter)
        {
            // Print output.
            Action<string, string, string, ConsoleColor> insertLine = (supplierName, planName, cost, color) => Console.WriteLine($"|{supplierName.PadRight(60, ' '),5}|{planName.PadRight(30, ' '),5}|{cost.PadRight(10, ' '),5}|", Console.ForegroundColor = color);

            if (shouldPrintHeader)
            {
                Console.WriteLine();
                Console.WriteLine(new string('*', 100));
                // Adding header
                insertLine("Supplier name", "Plan Name", "Cost", ConsoleColor.DarkYellow);
            }

            // Adding consumer  details row
            insertLine(energyConsumptionCalculator.SupplierName, energyConsumptionCalculator.PlanName, Convert.ToString(totalBillAmount), ConsoleColor.White);

            if (shouldPrintFooter)
            {
                Console.WriteLine(new string('*', 110));
                Console.WriteLine();
            }
        }
    }
}