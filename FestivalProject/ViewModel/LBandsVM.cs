using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalProject.ViewModel
{
    class LBandsVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Bands"; }
        }

        //Property om de geselecteerde band te bewerken
        private Band _selectedBand;

        public Band SelectedBand
        {
            get { return _selectedBand; }
            set { _selectedBand = value; OnPropertyChanged("SelectedBand"); }
        }
        
        //Property om alle bands in te lezen
        private ObservableCollection<Band> _bands;

        public ObservableCollection<Band> Bands
        {
            get { return _bands; }
            set { _bands = value; OnPropertyChanged("Bands"); }
        }

        //Constructor
        public LBandsVM()
        {
            Bands = Band.GetBands();
            SelectedBand = Bands[0];
        }

    }
}
