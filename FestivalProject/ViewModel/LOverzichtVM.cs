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

        private Stage _selectedStage;

        public Stage SelectedStage
        {
            get { return _selectedStage; }
            set { _selectedStage = value; OnPropertyChanged("SelectedStage"); }
        }
        
        private ObservableCollection<Stage> _stages;

        public ObservableCollection<Stage> Stages
        {
            get { return _stages; }
            set { _stages = value; OnPropertyChanged("Stages"); }
        }

        public LOverzichtVM()
        {
            Stages = Stage.GetStages();
            SelectedStage = Stages[0];
        }

    }
}
