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
    class LRoosterVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Rooster"; }
        }

        //Alle stages inlezen
        private ObservableCollection<Stage> _stages;

        public ObservableCollection<Stage> Stages
        {
            get { return _stages; }
            set { _stages = value; OnPropertyChanged("Stages"); }
        }

        //Festivaldata inlezen
        private ObservableCollection<Festival> _festivals;

        public ObservableCollection<Festival> Festivals
        {
            get { return _festivals; }
            set { _festivals = value; OnPropertyChanged("Festivals"); }
        }        

        //Property voor alle beschikbare festivaldagen 
        private ObservableCollection<DateTime> _dagen;

        public ObservableCollection<DateTime> Dagen
        {
            get { return _dagen; }
            set { _dagen = value; OnPropertyChanged("Dagen"); }
        }
        
        //Property voor de bands
        private ObservableCollection<Band> _bands;

        public ObservableCollection<Band> Bands
        {
            get { return _bands; }
            set { _bands = value; OnPropertyChanged("Bands"); }
        }

        //Geselecteerde stage
        private Stage _selectedStage;

        public Stage SelectedStage
        {
            get { return _selectedStage; }
            set { _selectedStage = value; OnPropertyChanged("SelectedStage"); }
        }

        //Geselecteerde dag
        private DateTime? _selectedDay;

        public DateTime? SelectedDay
        {
            get { return _selectedDay; }
            set { _selectedDay = value; OnPropertyChanged("SelectedDay"); }
        }

        //Geselecteerde line up
        private LineUp _selectedLineUp;

        public LineUp SelectedLineUp
        {
            get { return _selectedLineUp; }
            set { _selectedLineUp = value; OnPropertyChanged("SelectedLineUp"); }
        }     
   
        //Geselecteerde band in line up property
        private Band _selectedBand;

        public Band SelectedBand
        {
            get { return _selectedBand; }
            set { _selectedBand = value; OnPropertyChanged("SelectedBand"); }
        }
        
        
        //Property voor line up
        private ObservableCollection<LineUp> _lineUps;

        public ObservableCollection<LineUp> LineUps
        {
            get { return _lineUps; }
            set { _lineUps = value; OnPropertyChanged("LineUps"); }
        }

        //Property voor nieuwe line up
        private LineUp _newLineUp;

        public LineUp NewLineUp
        {
            get { return _newLineUp; }
            set { _newLineUp = value; OnPropertyChanged("NewLineUp"); }
        }

        //Property voor het uur (from)
        private DateTime _hourFrom;

        public DateTime HourFrom
        {
            get { return _hourFrom; }
            set { _hourFrom = value; OnPropertyChanged("HourFrom"); }
        }

        //Property voor het uur (until)
        private DateTime _hourUntil;

        public DateTime HourUntil
        {
            get { return _hourUntil; }
            set { _hourUntil = value; OnPropertyChanged("HourUntil"); }
        }
        
        
        
        //Constructor
        public LRoosterVM()
        {
            Stages = Stage.GetStages();
            Festivals = Festival.GetFestivals();
            Dagen = BerekenData();
            Bands = Band.GetBands();
            NewLineUp = new LineUp();
        }


        //Methode om aantal dagen te berekenen tussen begin en einddata
        private ObservableCollection<DateTime> BerekenData() 
        {
            ObservableCollection<DateTime> lijstDagen = new ObservableCollection<DateTime>();

            foreach (Festival festival in Festivals) 
            {
                DateTime start = festival.StartDate;
                DateTime eind = festival.EndDate;

                for (var date = start; date <= eind; date = date.AddDays(1))
                { lijstDagen.Add(date); }

                return lijstDagen;
            }
            return null;
        }

        //Command om geselecteerde band te zetten
        public ICommand BindGeselecteerdeBand 
        {
            get 
            {
                return new RelayCommand(SelectedBandBinden);
            }
        }

        //Method om de band uit de lineup te binden aan de property selectedBand
        private void SelectedBandBinden() 
        {
            if (SelectedLineUp != null)
            {
                SelectedBand = SelectedLineUp.Band;
            }
        }

        //Command om line up te tonen
        public ICommand ToonLineUpCommand 
        {
            get 
            {
                return new RelayCommand(ToonLineUp);
            }
        }

        //Method om line up te tonen
        private void ToonLineUp() 
        {
            if (SelectedDay != null && SelectedStage!=null)
            {
                LineUps = LineUp.GetLineUpByStageAndDay(SelectedStage, SelectedDay);
            }
        }

        //Command om nieuwe line up op te slaan
        public ICommand SaveLineUpCommand
        {
            get
            {
                return new RelayCommand(AddLineUp, NewLineUp.IsValid);
            }
        }

        //Command om line up te bewerken
        public ICommand EditLineUpCommand
        {
            get
            {
                return new RelayCommand(EditLineUp);
            }
        }

        //method om line up te bewerken
        private void EditLineUp()
        {
            if (SelectedLineUp.Id != null)
            {

            }
        }

        //Method om een nieuwe line up toe te voegen
        private void AddLineUp()
        {
            //controle of dag en stage wel geselecteerd zijn
            if (SelectedStage != null && SelectedDay != null)
            {
                NewLineUp.Date = Convert.ToDateTime(SelectedDay);
                NewLineUp.Stage = SelectedStage;

                //controles van data en uren (niet op zelfde moment optreden)
                NewLineUp.From = HourFrom.ToShortTimeString();
                NewLineUp.Until = HourUntil.ToShortTimeString();
                int affected = LineUp.AddLineUp(NewLineUp);
                if (affected == 1)
                {
                    LineUps.Add(NewLineUp);
                    NewLineUp = new LineUp();
                    int LastIndex = Bands.Count - 1;
                    SelectedBand = Bands[LastIndex];
                    Console.WriteLine("Line up werd succesvol toegevoegd in de database.");
                    ModernDialog.ShowMessage("Het optreden werd toegevoegd.", "Toevoegen", MessageBoxButton.OK);
                }
            }
            else
            {
                ModernDialog.ShowMessage("Gelieve een dag en stage te selecteren", "Optreden", MessageBoxButton.OK);
            }
        }

        public ICommand DeleteLineUpCommand
        {
            get
            {
                return new RelayCommand(DeleteLineUp);
            }
        }

        //Method om een volledige lijn in line up te verwijderen
        private void DeleteLineUp() 
        {
            try { 
            //Controle van bestaande line up
                if (SelectedLineUp.Id != null)
                {
                    if (ModernDialog.ShowMessage("Het optreden van " + SelectedLineUp.Band.Name + " zal worden verwijderd. Bent u zeker?", "Verwijderen", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        int affected = LineUp.DeleteLineUp(SelectedLineUp);
                        ToonLineUp();
                    }
                    else
                    {
                        //doe niets en keer terug
                    }
                }
            }
                catch(NullReferenceException ex)
            {
                ModernDialog.ShowMessage("Gelieve een optreden te selecteren", "Optreden", MessageBoxButton.OK);
                Console.WriteLine(ex.Message);
                }
        }

    }
}
