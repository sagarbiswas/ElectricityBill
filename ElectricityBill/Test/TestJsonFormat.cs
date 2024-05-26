using EnergyAnnualCostCalculation.Common;
using EnergyAnnualCostCalculation.Model.JsonValidationModel;
using EnergyAnnualCostCalculation.Validation;

namespace TestEnergyConsumption
{
    public class TestJsonFormat
    {
        [Fact]
        public void IsValidJson()
        {
            var jsonSchemaValidator = new JsonSchemaValidator();
            var response = jsonSchemaValidator.Validate(new ValidateRequest()
            {
                Schema = JsonFiles.SchemaJsonFileContent,
                Json = JsonFiles.JsonInputFileContent
            });
            Assert.True(response.Valid);

        }

        [Fact]
        public void IsInValidJson()
        {
            var jsonSchemaValidator = new JsonSchemaValidator();
            var validationReq = new ValidateRequest()
            {
                Schema = JsonFiles.SchemaJsonFileContent,
                Json = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), "\\SampleJson\\InvalidSample.json"))

            };
            var response = jsonSchemaValidator.Validate(validationReq);
            Assert.False(response.Valid);

        }
    }
}