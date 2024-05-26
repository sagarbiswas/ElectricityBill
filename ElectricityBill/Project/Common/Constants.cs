namespace EnergyAnnualCostCalculation.Common
{
    public class Constants
    {
        public const  string FileInputMsg = "Input <filename> (or Press enter to get default json from Asset folder) : ";
        public const string AnnualConsumptionMsg = "Annual_unit <annual_consumption> : ";
        public const string ExitMsg = "Type X - Exit / R - New Energy Unit / I - Input New Energy Calculation File : ";

        public static ApplicationSettings ApplicationSettings
        {
            get
            {

                if (ServicesProvider.ApplicationSettings == null)
                {
                    ServicesProvider.RegisterServices(); // Test case issue. not mocking
                    return ServicesProvider.ApplicationSettings;
                };
                return ServicesProvider.ApplicationSettings;
            }
        }


        public static string InvalidFile
        {
            get { return ApplicationSettings.InvalidFile;  }
        }

        public static int TotalBillDays
        {
            get { return Convert.ToInt16(ApplicationSettings.TotalBillDays); }
        }
        public static int DefaultStandingCharge
        {
            get { return Convert.ToInt16(ApplicationSettings.DefaultStandingCharge); }
        }

    }
}
