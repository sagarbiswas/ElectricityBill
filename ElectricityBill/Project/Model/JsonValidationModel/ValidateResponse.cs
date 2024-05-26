using Newtonsoft.Json.Schema;

namespace EnergyAnnualCostCalculation.Model.JsonValidationModel
{
    public class ValidateResponse
    {
        public bool Valid { get; set; }
        public IList<ValidationError> Errors { get; set; }
    }
}
