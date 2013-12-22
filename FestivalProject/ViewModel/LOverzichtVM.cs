using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        //Property voor alle dagoverzichten
        private ObservableCollection<ObservableCollection<LineUp>> _dagOverzichten;

        public ObservableCollection<ObservableCollection<LineUp>> DagOverzichten
        {
            get { return _dagOverzichten; }
            set { _dagOverzichten = value; OnPropertyChanged("DagOverzichten"); }
        }

        //VOOR DYNAMISCH LINKS TOE TE VOEGEN AAN MODERN UI MENU:
        //BRON: DISCUSSIONS MUI.CODEPLEX.COM
        private LinkCollection _links;

        public LinkCollection Links
        {
            get { return _links; }
            set { _links = value; OnPropertyChanged("Links"); }
        }

        public Link NewLink { get; set; }

        private LinkCollection GenerateLinks()
        {
            LinkCollection links = new LinkCollection();
            for (int i = 0; i <= Dagen.Count; i++)
            {
                //Nieuwe usercontrol voor de link
                UserControl dag = new UserControl();
                dag.Name = "LODag" + (i + 1);
                

                //Nieuwe Link
                NewLink = new Link();
                NewLink.DisplayName = "Dag " + (i + 1);
                NewLink.Source = new Uri("/View/"+ dag.Name + ".xaml", UriKind.Relative);
                links.Add(NewLink);

           
                
            }
            return links;
        }
        
        private Uri _selectedSource;

        public Uri SelectedSource
        {
            get { return _selectedSource; }
            set 
            {
                if (this._selectedSource != value)
                {
                    this._selectedSource = value; OnPropertyChanged("SelectedSource");
                }
            }
        }


        //Constructor
        public LOverzichtVM()
        {
            Festivals = Festival.GetFestivals();
            Stages = Stage.GetStages();
            SelectedStage = Stages[0];
            Dagen = BerekenData();
            Links = GenerateLinks();
            SelectedSource = Links[0].Source;
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
