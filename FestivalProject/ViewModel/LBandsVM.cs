using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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

        private Band _newBand;

        public Band NewBand
        {
            get { return _newBand; }
            set { _newBand = value; OnPropertyChanged("NewBand"); }
        }
        

        //Constructor
        public LBandsVM()
        {
            Bands = Band.GetBands();
            SelectedBand = Bands[0];
            NewBand = new Band();
        }

        public ICommand AddCommand 
        {
            get 
            {
                return new RelayCommand(AddBand);
            }
        }

        private void AddBand() 
        {
            //bij de save command van band:
            //vraag pad op van image

            
            //Stream stream = ImageSource.StreamSource;
            //Byte[] buffer = null;
            //if (stream != null && stream.Length > 0)
            //{
            //    using (BinaryReader br = new BinaryReader(stream))
            //    {
            //        buffer = br.ReadBytes((Int32)stream.Length);
            //    }
            //}

            //NewBand.PictureByte = buffer;

            int affected = Band.AddBand(NewBand);
            if (affected == 1)
            {
                Bands.Add(NewBand);
                int LastIndex = Bands.Count - 1;
                SelectedBand = Bands[LastIndex];
            }
        }
    }
}
