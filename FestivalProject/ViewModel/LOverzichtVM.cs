using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FestivalProject.ViewModel
{
    class LOverzichtVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Overzicht";  }
        }

        //Property voor de geselecteerde stage
        private Stage _selectedStage;

        public Stage SelectedStage
        {
            get { return _selectedStage; }
            set { _selectedStage = value; OnPropertyChanged("SelectedStage"); }
        }
        
        //Property voor alle stages in te lezen
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

        //Property om alle data in te lezen
        private ObservableCollection<DateTime> _dagen;

        public ObservableCollection<DateTime> Dagen
        {
            get { return _dagen; }
            set { _dagen = value; OnPropertyChanged("Dagen"); }
        }

        //Geselecteerde dag
        private DateTime? _selectedDay;

        public DateTime? SelectedDay
        {
            get { return _selectedDay; }
            set { _selectedDay = value; OnPropertyChanged("SelectedDay"); }
        }

        //Property voor line up
        private ObservableCollection<LineUp> _lineUps;

        public ObservableCollection<LineUp> LineUps
        {
            get { return _lineUps; }
            set { _lineUps = value; OnPropertyChanged("LineUps"); }
        }

        //Constructor
        public LOverzichtVM()
        {
            Festivals = Festival.GetFestivals();
            Stages = Stage.GetStages();
            SelectedStage = Stages[0];
            Dagen = BerekenData();
        }

        //Command om line up te tonen
        public ICommand ToonLineUpCommand 
        {
            get 
            {
                return new RelayCommand(ToonLineUp);
            }
        }

        private void ToonLineUp(object obj)
        {
            if (SelectedDay != null && SelectedStage != null)
            {
                LineUps = LineUp.GetLineUpByStageAndDay(SelectedStage, SelectedDay);
            }
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

    }
}
