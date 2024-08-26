using SimpleCurrecyApp.Data;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using System.IO;
namespace SimpleCurrecyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataTable dtCurrency;
        public bool UserSkipped = false;

        public MainWindow()
        {
            InitializeComponent();
            SetComboBoxes(Data.CsvController.LoadCsvToDataTable("Resources/ConcurrencyData.csv"));

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string apiKey = ApiRequester.GetApiKey();
            DataTable temp;


            if (!UserSkipped && apiKey.Equals(""))
            {
                ApiRequest.Show(Data.WarningType.NotValidAPIKey, this);
                return;
            }

            if (!File.Exists($"Resources/db_{DateTime.Now.ToString("yyyyMMdd")}.csv") == false)
            {
                await ApiRequester.SetApiKey(ApiRequester.GetApiKey());
                temp = ApiRequester.GetRequestedData();

                if (temp.Rows.Count > 0)
                {
                    Data.CsvController.SaveDataTableToCsv(temp, $"Resources/db_{DateTime.Now.ToString("yyyyMMdd")}.csv");
                } else
                {
                    ApiRequest.Show(Data.WarningType.NotValidAPIKey, this);
                    return;
                }
            }

            SetComboBoxes(Data.CsvController.LoadCsvToDataTable($"Resources/db_{DateTime.Now.ToString("yyyyMMdd")}.csv"));
        }




        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            decimal fromval;
            if (!decimal.TryParse(txtUserInput.Text, out fromval))
            {

                WarningWindow.Show(Data.WarningType.NoUserInput, this);

            } else if (cmbFromUnit.SelectedValue == null || cmbFromUnit.SelectedIndex == 0)
            {
                WarningWindow.Show(Data.WarningType.NoFromCurrency, this);
            } else if (cmbToUnit.SelectedValue == null || cmbToUnit.SelectedIndex == 0)
            {
                WarningWindow.Show(Data.WarningType.NoToCurrency, this);
            } else
            {
                decimal toval = Math.Round(fromval / decimal.Parse(cmbFromUnit.SelectedValue.ToString()) * decimal.Parse(cmbToUnit.SelectedValue.ToString()), 2);
                txtResult.Text = toval.ToString();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtUserInput.Text = "";
            txtResult.Text = "";
            cmbFromUnit.SelectedIndex = 0;
            cmbToUnit.SelectedIndex = 0;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        public void SetComboBoxes(DataTable dataTable)
        {
            dtCurrency = dataTable;
            cmbFromUnit.ItemsSource = dtCurrency.DefaultView;
            cmbFromUnit.DisplayMemberPath = "Name";
            cmbFromUnit.SelectedValuePath = "Rate";
            cmbFromUnit.SelectedIndex = 0;
            cmbToUnit.ItemsSource = dtCurrency.DefaultView;
            cmbToUnit.DisplayMemberPath = "Name";
            cmbToUnit.SelectedValuePath = "Rate";
            cmbToUnit.SelectedIndex = 0;
        }
    }
}