using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleCurrecyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    public static DataTable dtCurrency;
    public static DataTable dtCurrencyList;
    public MainWindow()
        {
            InitializeComponent();
            dtCurrency = Data.CsvReader.LoadCsvToDataTable("Resources/ConcurrencyData.csv");

            cmbFromUnit.ItemsSource = dtCurrency.DefaultView;
            cmbFromUnit.DisplayMemberPath = "Name";
            cmbFromUnit.SelectedValuePath = "Rate";
            cmbFromUnit.SelectedIndex = 0;

            cmbToUnit.ItemsSource = dtCurrency.DefaultView;
            cmbToUnit.DisplayMemberPath = "Name";
            cmbToUnit.SelectedValuePath = "Rate";
            cmbToUnit.SelectedIndex = 0;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            decimal fromval;
            if (!decimal.TryParse(txtUserInput.Text, out fromval))
            {
                
                WarningWindow.Show(Data.WarningType.NoUserInput, this);

            } else if (cmbFromUnit.SelectedValue == null || cmbFromUnit.SelectedValue == "0")
            {
                WarningWindow.Show(Data.WarningType.NoFromCurrency, this);
            } else if (cmbToUnit.SelectedValue == null || cmbToUnit.SelectedValue == "0")
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
    }
}