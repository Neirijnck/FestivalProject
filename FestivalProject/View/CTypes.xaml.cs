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

namespace FestivalProject
{
    /// <summary>
    /// Interaction logic for CTypes.xaml
    /// </summary>
    public partial class CTypes : UserControl
    {
        public CTypes()
        {
            InitializeComponent();
            this.Loaded += CTypes_Loaded;
            txbType.TextChanged += txbType_TextChanged;
        }

        void txbType_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableDisableControls();
        }

        void CTypes_Loaded(object sender, RoutedEventArgs e)
        {
            EnableDisableControls();
        }
        private void EnableDisableControls() 
        {
            if (txbType.Text != "" && txbType.Text.Length>=2 && lstTypes.SelectedIndex != -1) 
            {
                btnBewerken.IsEnabled = true;
            }
            else
            {
                btnBewerken.IsEnabled = false;
            }
        }

    }
}
