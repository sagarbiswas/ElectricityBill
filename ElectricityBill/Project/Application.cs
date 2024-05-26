using EnergyAnnualCostCalculation.BusinessLayer;
using EnergyAnnualCostCalculation.Common;
using EnergyAnnualCostCalculation.Model;
using EnergyAnnualCostCalculation.Model.JsonInputModel;
using EnergyAnnualCostCalculation.Model.JsonValidationModel;
using EnergyAnnualCostCalculation.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyAnnualCostCalculation
{

    public class Application
    {
        private readonly IEnergyConsumptionHandler _iEnergyConsumptionHandler;
        public Application(IEnergyConsumptionHandler iEnergyConsumptionHandler)
        {
            _iEnergyConsumptionHandler = iEnergyConsumptionHandler; 
        }
        public void StartApp()
        {
            StartPoint: // TODO - Goto is not recommended to use. Need to remove for Production.
            //Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                Console.Write(Constants.FileInputMsg);
                // If user enter input keyword then it will ignore.
                JsonFiles.JsonInputFilePath = (Console.ReadLine() ?? "INPUT").ToUpper().Replace("INPUT", string.Empty).Trim();

                // Validate JSON file with Schema.
                var validateJsonWithSchema = new ValidateRequest()
                {
                    Schema = JsonFiles.SchemaJsonFileContent,
                    Json = JsonFiles.JsonInputFileContent
                };
                List<EnergyConsumptionCalculator>? lstEnergyConsumptionCalculatorData;
                if (!_iEnergyConsumptionHandler.ValidateJsonWithSchema(validateJsonWithSchema, out lstEnergyConsumptionCalculatorData))
                {
                    Console.WriteLine(Constants.InvalidFile);
                    goto StartPoint;
                }

                // Get Total bill
                Console.Write(Constants.AnnualConsumptionMsg);
                int totalConsumption = Convert.ToInt32((Console.ReadLine() ?? "annual_unit ").ToUpper().Replace("annual_unit ", string.Empty).Trim()); // if your enter annual_unit keyword then it will ignore.

                Task<double>[] tskEngeryConsumptions = new Task<double>[lstEnergyConsumptionCalculatorData == null ? 0 : lstEnergyConsumptionCalculatorData.Count];

                for (int i = 0; i < tskEngeryConsumptions.Length; i++)
                {
                    var energyConsumptionModel = new EnergyConsumptionModel()
                    {
                        TotalConsumption = totalConsumption,
                        CurrentConsumerEnergyDetail = (lstEnergyConsumptionCalculatorData != null && lstEnergyConsumptionCalculatorData[i] != null) ? lstEnergyConsumptionCalculatorData[i] : new EnergyConsumptionCalculator()
                    };
                    tskEngeryConsumptions[i] = CalculateEnergyBill(energyConsumptionModel);
                }

                var results = Task.WhenAll(tskEngeryConsumptions);

                // Iterate over the results
                for (int i = 0; i < results.Result.Length; i++)
                {
                    _iEnergyConsumptionHandler.PrintOutPut(results.Result[i], (lstEnergyConsumptionCalculatorData != null && lstEnergyConsumptionCalculatorData[i] != null) ?
                        lstEnergyConsumptionCalculatorData[i] : new EnergyConsumptionCalculator(),
                        i == 0,
                        (i == (lstEnergyConsumptionCalculatorData == null ? 0 : lstEnergyConsumptionCalculatorData.Count - 1)));
                }

                // Exit the flow or continue with other consumer
                Console.Write(Constants.ExitMsg);
                if ((Console.ReadLine() ?? string.Empty).ToUpper().Contains("EXIT"))
                    return;

                Console.WriteLine();
                goto StartPoint;
            }
            catch (Exception)
            {
                Console.WriteLine(Constants.InvalidFile);
                goto StartPoint;
            }
        }


        /// <summary>
        /// This method is used to calculate energy bill.
        /// </summary>
        /// <param name="energyConsumptionModel"></param>
        /// <returns></returns>
        public async Task<double> CalculateEnergyBill(EnergyConsumptionModel energyConsumptionModel)
        {
            if (_iEnergyConsumptionHandler != null)
            {
                return await _iEnergyConsumptionHandler.GetTotalCost(energyConsumptionModel);
            }
            return 0;
        }
    }
}
