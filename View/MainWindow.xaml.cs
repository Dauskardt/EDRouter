﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EDRouter.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            // Some operations with this row
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.ViewModelMain VMM = (ViewModel.ViewModelMain)this.DataContext;

            if (VMM.SelectedRoutenFile != null)
            {
                VMM.ACLoadRouteFileFunc(VMM.SelectedRoutenFile.FullName);
                VMM.SaveSettings();
            }

        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header)
            {
                case "System":
                case "NS":
                case "Sprünge":
                case "Entfernung":
                case "Rest":
                case "besucht":
                case "TimeStamp":
                case "Info":
                    e.Column.IsReadOnly = true;
                    break;
                default:
                    break;
            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.ViewModelMain vmm = (ViewModel.ViewModelMain)this.DataContext;
            vmm.SaveSettings();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            object currentPos = DGVRoute.SelectedItem;

            if (currentPos != null)
            {
                DGVRoute.ScrollIntoView(DGVRoute.Items[DGVRoute.Items.Count - 1]);
                DGVRoute.UpdateLayout();
                DGVRoute.ScrollIntoView(currentPos);
            }
        }
    }
}