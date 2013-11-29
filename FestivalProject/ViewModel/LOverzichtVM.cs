using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //Constructor
        public LOverzichtVM()
        {
            Stages = Stage.GetStages();
            SelectedStage = Stages[0];
        }

    }
}
