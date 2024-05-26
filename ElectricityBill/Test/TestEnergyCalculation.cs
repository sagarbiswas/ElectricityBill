using EnergyAnnualCostCalculation;
using EnergyAnnualCostCalculation.BusinessLayer;
using EnergyAnnualCostCalculation.Model;
using EnergyAnnualCostCalculation.Validation;
using Microsoft.Extensions.Logging;
using Moq;


namespace TestEnergyConsumption
{
    public class TestEnergyCalculation
    {
        #region Property  
        public Mock<IEnergyConsumptionHandler> mockHandler = new Mock<IEnergyConsumptionHandler>();
        private Mock<ILogger<EnergyConsumptionHandler>> _mockLogger = new Mock<ILogger<EnergyConsumptionHandler>>(); 
        private Mock<IJsonSchemaValidator> _mockJsonSchemaValidator = new Mock<IJsonSchemaValidator>();

        #endregion
        [Fact]
        public async void IsValidCalculation()
        {
            double expectedResult = 6000;
            var energyConsumptionModel = new EnergyConsumptionModel()
            {
                TotalConsumption = 500,
                CurrentConsumerEnergyDetail = new EnergyConsumptionCalculator()
                {
                    PlanName = "Test Plan",
                    SupplierName = "Test supplier",
                    StandingCharge = 5,
                    Prices = new Price[]
                    {
                        new Price()
                        {
                            Rate = 20,
                            Threshold = 100
                        },
                        new Price()
                        {
                            Rate = 10,
                        }
                    }
                }
            };
            Application app = new Application(new EnergyConsumptionHandler(_mockJsonSchemaValidator.Object,_mockLogger.Object));
            var actualCalculateEnergyBill = await app.CalculateEnergyBill(energyConsumptionModel);
            Assert.True(actualCalculateEnergyBill.Equals(expectedResult));

        }
        [Fact]
        public async void IsStandingChargeAppliedForNonThreshold()
        {
            double expectedResult = 1825;
            var energyConsumptionModel = new EnergyConsumptionModel()
            {
                TotalConsumption = 500,
                CurrentConsumerEnergyDetail = new EnergyConsumptionCalculator()
                {
                    PlanName = "Test Plan",
                    SupplierName = "Test supplier",
                    StandingCharge = 5,
                    Prices = new Price[] { }
                }
            };
            Application app = new Application(new EnergyConsumptionHandler(_mockJsonSchemaValidator.Object, _mockLogger.Object));
            var actualCalculateEnergyBill = await app.CalculateEnergyBill(energyConsumptionModel);
            Assert.True(actualCalculateEnergyBill.Equals(expectedResult));

        }
    }

}
