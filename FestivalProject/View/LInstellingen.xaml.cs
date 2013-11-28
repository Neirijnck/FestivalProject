using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for LInstellingen.xaml
    /// </summary>
    public partial class LInstellingen : UserControl
    {
        public LInstellingen()
        {
            InitializeComponent();
            this.Loaded += LInstellingen_Loaded;
        }

        void LInstellingen_Loaded(object sender, RoutedEventArgs e)
        {
            EnableDisableControls();
        }

        String Data1 = "";
        DateTime? date1;
        String Data2 = "";
        DateTime? date2;
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //Eerste datum opvragen en tonen in textbox
            Data1 = "";
            var datepicker = sender as DatePicker;
            date1 = datepicker.SelectedDate;
            if (date1 == null)
            {
                txbData.Text = Data1;
            }
            else
            {
                Data1 += date1.Value.ToShortDateString();
                txbData.Text = Data1;
            }
            EnableDisableControls();
        }

        private void DatePicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //Tweede datum opvragen en tonen in textbox
            Data2 = "";
            var datepicker = sender as DatePicker;
            date2 = datepicker.SelectedDate;
            if (date2 == null)
            {
                txbData.Text = Data2;
            }
            else 
            {
                Data2 += date2.Value.ToShortDateString();
                txbData.Text = Data1 +" - "+ Data2;
            }
            EnableDisableControls();
        }
        private void btnBevestigen_Click(object sender, RoutedEventArgs e)
        {
            //wegschrijven naar Database
        }

        private void txbGenre_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableDisableControls();
        }

        private void txbStage_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableDisableControls();
        }

        private void EnableDisableControls()
        {
            //knoppen enkel beschikbaar maken als er content in de textboxen staat en data in orde zijn
            btnBevestigen.IsEnabled = false;
            btnGenre.IsEnabled = false;
            btnStage.IsEnabled = false;

            //Controle Datum 1 niet verder dan Datum 2
            DateTime? dt1 = date1;
            DateTime? dt2 = date2;
            DateTime DtA = Convert.ToDateTime(date1);
            DateTime DtB = Convert.ToDateTime(date2);

            TimeSpan Difference = DtB.Subtract(DtA);

            int resultCompare = DateTime.Compare(DtA, DtB);

            if (Data1 != "" && Data2 != "" && resultCompare <= 0)
            {
                btnBevestigen.IsEnabled = true;
            }

            if (txbStage.Text != "") { btnStage.IsEnabled = true; }
            if (txbGenre.Text != "") { btnGenre.IsEnabled = true; }

            Console.WriteLine(Difference.Days);
        }
    }
}
