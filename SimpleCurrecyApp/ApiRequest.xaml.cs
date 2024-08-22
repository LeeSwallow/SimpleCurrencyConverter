using SimpleCurrecyApp.Data;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace SimpleCurrecyApp
{
    /// <summary>
    /// ApiRequest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ApiRequest : Window
    {
        public MainWindow? ParentWindow { get; set; }

        public ApiRequest()
        {
            InitializeComponent();
        }


        public static void Show(WarningType type, Window owner)
        {
            ApiRequest apiRequest = new ApiRequest()
            {
                Owner = owner
            };
            apiRequest.ParentWindow = owner as MainWindow;
            apiRequest.ShowDialog();
        }




        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Data.WarningType warn = Data.WarningType.Success;
            string apiKey = pwAPIKey.Password;

            warn = await ApiRequester.SetApiKey(apiKey);

            if (apiKey.Equals(""))
            {
                txtWarning.Text = "Please enter a valid API Key";
                pwAPIKey.Clear();
                pwAPIKey.Focus();
            } else if (warn == Data.WarningType.NotValidAPIKey)
            {
                txtWarning.Text = "Please enter a valid API Key";
                pwAPIKey.Clear();
                pwAPIKey.Focus();
            } else
            {
                DataTable dataTable = ApiRequester.GetRequestedData();
                Data.CsvController.SaveDataTableToCsv(dataTable, $"Resources/db_{DateTime.Now.ToString("yyyyMMdd")}.csv");
                ParentWindow.SetComboBoxes(dataTable);
                this.DialogResult = true;
            }
            
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void Hyperlink_skip(object sender, RequestNavigateEventArgs e)
        {
            if (ParentWindow != null) { 
                ParentWindow.UserSkipped = true;
                this.DialogResult = true;
            }
        }
    }
}
