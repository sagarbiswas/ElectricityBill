﻿using EnergyAnnualCostCalculation.Model;
using EnergyAnnualCostCalculation.Model.JsonInputModel;
using EnergyAnnualCostCalculation.Model.JsonValidationModel;

namespace EnergyAnnualCostCalculation.BusinessLayer
{
    public interface IEnergyConsumptionHandler
    {
        bool ValidateJsonWithSchema(ValidateRequest validateJsonWithSchema, out List<EnergyConsumptionCalculator> lstEnergyConsumptionCalculatorData);
        Task<double> GetTotalCost(EnergyConsumptionModel energyConsumptionModel);
        void PrintOutPut(Double totalBillAmount, EnergyConsumptionCalculator energyConsumptionCalculator, bool shouldPrintHeader, bool shouldPrintFooter);
    }
}
