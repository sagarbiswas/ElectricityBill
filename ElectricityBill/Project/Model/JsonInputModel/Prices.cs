using Newtonsoft.Json;
namespace EnergyAnnualCostCalculation.Model.JsonInputModel
{
    public partial class Price
    {
        [JsonProperty("rate")]
        public double Rate { get; set; }

        [JsonProperty("threshold", NullValueHandling = NullValueHandling.Ignore)]
        public int? Threshold { get; set; }
    }
}