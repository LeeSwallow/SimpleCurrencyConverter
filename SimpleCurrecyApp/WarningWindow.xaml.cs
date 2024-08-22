using Accessibility;
using SimpleCurrecyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleCurrecyApp
{
    /// <summary>
    /// WarningWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WarningWindow : Window
    {

        public WarningType? Warn { get; set; }
        public MainWindow? ParentWindow { get; set; }
        
        public WarningWindow()
        {
            InitializeComponent();
        }

        public static void Show(WarningType type, Window owner)
        {
            WarningWindow warningWindow = new WarningWindow()
            {
                Owner = owner
        
            };
            warningWindow.Warn = type;
            warningWindow.ParentWindow = owner as MainWindow;

            if (Data.WarningType.NoUserInput == type)
            {
                warningWindow.txtWarning.Text = "Please enter a valid number";
            }
            else if (Data.WarningType.NoFromCurrency == type)
            {
                warningWindow.txtWarning.Text = "Please select a currency to convert from";
            }
            else
            {
                warningWindow.txtWarning.Text = "Please select a currency to convert to";
            }
            warningWindow.ShowDialog();
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (ParentWindow == null)
            {
                return;
            }

            if (Warn == WarningType.NoUserInput)
            {
                ParentWindow?.txtUserInput.Focus();
            }
            else if (Warn == WarningType.NoFromCurrency)
            {
                ParentWindow?.cmbFromUnit.Focus();
            }
            else
            {
                ParentWindow?.cmbToUnit.Focus();
            }
        }
    }
    }

