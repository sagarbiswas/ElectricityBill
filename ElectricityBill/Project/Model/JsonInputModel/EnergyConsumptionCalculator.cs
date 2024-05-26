using Newtonsoft.Json;
namespace EnergyAnnualCostCalculation.Model.JsonInputModel
{
    public partial class EnergyConsumptionCalculator
    {
        [JsonProperty("supplier_name")]
        public string SupplierName { get; set; }

        [JsonProperty("plan_name")]
        public string PlanName { get; set; }

        [JsonProperty("prices")]
        public Price[] Prices { get; set; }

        [JsonProperty("standing_charge")]
        public long? StandingCharge { get; set; }
    }
}
