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

namespace EDRouter.View.Dialog
{
    /// <summary>
    /// Interaktionslogik für DialogSettings.xaml
    /// </summary>
    public partial class DialogSettings : Window
    {
        public Model.Settings UserSettings { get; set; } = new Model.Settings();

        public DialogSettings()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           this.DialogResult = true;
        }
    }
}
