using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


namespace SimpleCurrecyApp.Data
{
    class ApiRequester
    {
        public static string BaseUrl = @"https://v6.exchangerate-api.com/v6/";
        public static string RequestDate = "latest";
        public static string BaseCurrency = "USD";
        private static string _userApiKey = "";
        private static string _jsonData = "";
        private static DataTable _dataTable = new DataTable();
        public static string GetApiKey()
        {
            return _userApiKey;
        }

        public async static Task<Data.WarningType> SetApiKey(string apiKey)
        {
            if (apiKey.Length > 5 && apiKey.Length <= 32)
            {
                await GetJsonData(apiKey);

                if (string.IsNullOrEmpty(_jsonData))
                {
                    return Data.WarningType.NotValidAPIKey;
                }
                else
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["ApiKey"].Value = apiKey;
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    _userApiKey = apiKey;
                    return Data.WarningType.Success;
                }
            }

            return Data.WarningType.NotValidAPIKey;
        }


        private async static Task<string> GetJsonData(string apikey)
        {
            if (_jsonData.Equals(""))
            {

                string url = BaseUrl + apikey + "/" + RequestDate + "/" + BaseCurrency;
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        _jsonData = await client.GetStringAsync(url);
                    }
                    catch (HttpRequestException e)
                    {
                        _jsonData = "";
                    }
                }
            }

            return _jsonData;
        }



        public static DataTable GetRequestedData()
        {

            if (_dataTable.Rows.Count > 0 && (DateTime.Parse(_dataTable.Rows[0]["Date"], ) ))
            {
                return _dataTable;
            }

            _dataTable.Clear();
            if (_dataTable.Columns.Count == 0)
            {
                _dataTable.Columns.Add("Name", typeof(string));
                _dataTable.Columns.Add("Rate", typeof(decimal));
                _dataTable.Columns.Add("Date", typeof(string));
            }

            var jObject = JsonConvert.DeserializeObject<dynamic>(_jsonData);

            try { 
                DateTime.TryParse(jObject.time_last_update_utc.ToString(), out DateTime date);

                string formattedDate = date.ToString("yyyyMMdd");
                _dataTable.Rows.Add(new object[] { "Select", 0.0, formattedDate });


                var conversionRates = jObject["conversion_rates"] as JObject;
                foreach (var property in conversionRates.Properties())
                {
                    var dataRow = _dataTable.NewRow();
                    dataRow.ItemArray = new object[] { property.Name, (double)property.Value, formattedDate };
                    _dataTable.Rows.Add(dataRow);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dataTable.Rows.Clear();
            }

            return _dataTable;
        }
    }
}
