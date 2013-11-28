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
    /// Interaction logic for LRooster.xaml
    /// </summary>
    public partial class LRooster : UserControl
    {
        public LRooster()
        {
            InitializeComponent();
        }

        private void EnableDisableControls()
        {
            //Buttons enkel als overal een item is geselecteerd in de comboboxen
            btnOpslaan.IsEnabled = false;
            btnVerwijderen.IsEnabled = false;

            if (cboBand.SelectedIndex != -1 && cboDag.SelectedIndex != -1 && cboStage.SelectedIndex != -1)
            {
                btnOpslaan.IsEnabled = true;
                btnVerwijderen.IsEnabled = true;
            }
        }

    }
}
