using Newtonsoft.Json.Schema;

namespace EnergyAnnualCostCalculation.Model
{
    public class ValidateResponse
    {
        public bool Valid { get; set; }
        public IList<ValidationError> Errors { get; set; }
    }
}
