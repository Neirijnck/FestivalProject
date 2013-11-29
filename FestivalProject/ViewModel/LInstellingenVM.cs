using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalProject.ViewModel
{
    class LInstellingenVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Instellingen"; }
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
        
        //Constructor
        public LInstellingenVM()
        {
            Stages = Stage.GetStages();
            Genres = Genre.GetGenres();
        }

    }
}
