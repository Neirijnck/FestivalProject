using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FestivalProject.ViewModel
{
    class LInstellingenVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Instellingen"; }
        }
        //Property voor festival
        private ObservableCollection<Festival> _festivals;

        public ObservableCollection<Festival> Festivals
        {
            get { return _festivals; }
            set { _festivals = value; OnPropertyChanged("Festivals"); }
        }
        

        //Property om alle stages in te lezen
        private ObservableCollection<Stage> _stages;

        public ObservableCollection<Stage> Stages
        {
            get { return _stages; }
            set { _stages = value; OnPropertyChanged("Stages"); }
        }

        //Property om alle genres in te lezen
        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; OnPropertyChanged("Genres"); }
        }

        //Property om een nieuw festival toe te voegen
        private Festival _festivalData;

        public Festival FestivalData
        {
            get { return _festivalData; }
            set { _festivalData = value; OnPropertyChanged("FestivalData"); }
        }        

        //Property voor de geselecteerd startdatum
        private DateTime? _selectedStartDate;

        public DateTime? SelectedStartDate
        {
            get { return _selectedStartDate; }
            set { _selectedStartDate = value; OnPropertyChanged("SelectedStartDate"); }
        }
        
        //Property voor de geselecteerde einddatum
        private DateTime? _selectedEndDate;

        public DateTime? SelectedEndDate
        {
            get { return _selectedEndDate; }
            set { _selectedEndDate = value; OnPropertyChanged("SelectedEndDate"); }
        }
        //Property om de uiteindelijk datum weer te geven in textbox
        private String _fullDate;

        public String FullDate
        {
            get { return _fullDate; }
            set { _fullDate = value; OnPropertyChanged("FullDate"); }
        }
        

        //Constructor
        public LInstellingenVM()
        {
            Festivals = Festival.GetFestivals();
            Stages = Stage.GetStages();
            Genres = Genre.GetGenres();
            FestivalData = new Festival();
        }

        //Command om data toe te voegen in database
        public ICommand OpslaanCommand 
        {
            get 
            {
                return new RelayCommand(AddDates, FestivalData.IsValid);
            }
        } 

        //Method om de data toe te voegen in de database
        private void AddDates() 
        {
            int affected = Festival.AddFestival(FestivalData);
            if (affected == 1)
            {
                Festivals.Add(FestivalData);
                ModernDialog.ShowMessage("De data is toegevoegd.", "Data", MessageBoxButton.OK);
            }
        }

        //Command om de data te controleren
        public ICommand ControlDates 
        {
            get 
            {
                return new RelayCommand(ControleerData);
            }
        }

        //Method om de data te controleren
        private void ControleerData()
        {
            String Date1 = "";
            String Date2 = "";

            //Eerste datum opvragen en tonen in textbox
            if (SelectedStartDate == null) { FullDate = Date1; }
            else 
            {
                Date1 = SelectedStartDate.Value.ToShortDateString();
                FullDate = Date1;
            }

            //Tweede datum opvragen en tonen in textbox
            if (SelectedEndDate == null) { FullDate = Date1 + "    -    " + Date2; }
            else 
            {
                Date2 = SelectedEndDate.Value.ToShortDateString();
                FullDate = Date1 + "    -    " + Date2;
            }

            //Controle dat eerste datum niet groter is dan tweede
            DateTime EersteDatum = Convert.ToDateTime(SelectedStartDate);
            DateTime TweedeDatum = Convert.ToDateTime(SelectedEndDate);

            TimeSpan Difference = TweedeDatum.Subtract(EersteDatum);
            int resultCompare = DateTime.Compare(EersteDatum, TweedeDatum);

            if (Date1 != "" && Date2 != "" && resultCompare <= 0)
            {
                FestivalData.StartDate = EersteDatum;
                FestivalData.EndDate = TweedeDatum;
                Console.WriteLine("Data is OK!");
            }
            else 
            {
                Console.WriteLine("Data verkeerd!");
                //Data is niet ok, button moet gedisabled worden!!!
                //FestivalData.IsValid = false;
            }
        }
    }
}
