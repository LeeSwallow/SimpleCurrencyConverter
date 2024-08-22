namespace SimpleCurrecyApp.Data
{
    public class CurrencyResponse
    {
        public string Result { get; set; }
        public string TimeLastUpdateUtc { get; set; }
        public string BaseCode { get; set; }
        public Dictionary<string, double> ConversionRates { get; set; }

    }
}