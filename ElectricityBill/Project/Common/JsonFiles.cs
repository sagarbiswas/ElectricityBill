namespace EnergyAnnualCostCalculation.Common
{
    public static class JsonFiles
    {
        static string schemaFilename = Path.Join(Directory.GetCurrentDirectory(),
                                        "\\Asset\\JsonSchema\\EnergyConsumptionCalulator.schema.json");
        public static string SchemaJsonFileContent => File.ReadAllText(schemaFilename ?? "");
        public static string JsonInputFileContent => File.ReadAllText(string.IsNullOrWhiteSpace(JsonInputFilePath) ?
            Path.Join(Directory.GetCurrentDirectory(), "\\Asset\\SampleJson\\Sample.json") :
            JsonInputFilePath);

        public static string? JsonInputFilePath { get; set; }
    }
}
