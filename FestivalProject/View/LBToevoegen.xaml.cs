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
    /// Interaction logic for LBToevoegen.xaml
    /// </summary>
    public partial class LBToevoegen : UserControl
    {
        public LBToevoegen()
        {
            InitializeComponent();
        }

        private void EnableDisableControls()
        {
            //button enkel enablen als de verplichte velden ingevuld zijn
            btnOpslaan.IsEnabled = false;
            if (txbName.Text != "" && txbBeschrijving.Text != "" && txbFB.Text != "")
            {
                btnOpslaan.IsEnabled = true;
            }
        }

    }
}
