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

namespace FestivalProject.View
{
    /// <summary>
    /// Interaction logic for TTypes.xaml
    /// </summary>
    public partial class TTypes : UserControl
    {
        public TTypes()
        {
            InitializeComponent();
            this.Loaded += TTypes_Loaded;
            txbBewerkType.TextChanged += txbBewerkType_TextChanged;
            txbBewerkPrijs.ValueChanged += txbBewerkPrijs_ValueChanged;
            txbBewerkAantal.ValueChanged += txbBewerkAantal_ValueChanged;
        }

        void txbBewerkAantal_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            EnableDisableControls();
        }

        private void EnableDisableControls()
        {
            if (txbBewerkType.Text != "" && txbBewerkType.Text.Length >= 2 && txbBewerkPrijs.Value != null && txbBewerkAantal.Text != "")
            {
                btnBewerken.IsEnabled = true;
            }
            else 
            {
                btnBewerken.IsEnabled = false;
            }
        }

        void txbBewerkPrijs_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            EnableDisableControls();
        }

        void txbBewerkType_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableDisableControls();
        }

        void TTypes_Loaded(object sender, RoutedEventArgs e)
        {
            EnableDisableControls();
        }
    }
}
