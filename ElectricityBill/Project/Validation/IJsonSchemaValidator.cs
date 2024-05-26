using EnergyAnnualCostCalculation.Model.JsonValidationModel;

namespace EnergyAnnualCostCalculation.Validation
{
    // Single responsibility principle
    public interface IJsonSchemaValidator
    {
        ValidateResponse Validate(ValidateRequest request);
    }
}
