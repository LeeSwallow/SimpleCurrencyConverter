﻿using System.Data;
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
    public MainWindow()
        {
            InitializeComponent();
            txtResult.Text = "test result";

        }


        private void BindCurrency()
        {
            DataTable dtCurrency = new DataTable();
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = string.Empty;
        }
    }
}