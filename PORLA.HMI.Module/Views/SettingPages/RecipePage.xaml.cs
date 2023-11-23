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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PORLA.HMI.Module.Views.SettingPages
{
    /// <summary>
    /// Interaction logic for RecipePage.xaml
    /// </summary>
    public partial class RecipePage : UserControl
    {
        public RecipePage()
        {
            InitializeComponent();
        }

        private void DataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
