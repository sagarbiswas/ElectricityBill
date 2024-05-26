using EnergyAnnualCostCalculation.Model;

namespace EnergyAnnualCostCalculation.Validation
{
    // Single responsibility principle
    public interface IJsonSchemaValidator
    {
        ValidateResponse Validate(ValidateRequest request);
    }
}
