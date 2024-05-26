using EnergyAnnualCostCalculation.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace EnergyAnnualCostCalculation.Validation
{
    public class JsonSchemaValidator : IJsonSchemaValidator
    {
        /// <summary>
        /// This method is used to validate energy calculator or any json with jsaon schema.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidateResponse Validate(ValidateRequest request)
        {
            // load schema
            JSchema schema = JSchema.Parse(request.Schema);
            JToken json = JToken.Parse(request.Json);

            // validate json
            IList<ValidationError> errors;
            bool valid = json.IsValid(schema, out errors);

            // return error messages and line info to the browser
            return new ValidateResponse
            {
                Valid = valid,
                Errors = errors
            };
        }
    }

}
